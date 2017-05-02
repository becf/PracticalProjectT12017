/*******************************************************************************************************
* SIT782 - PRACTICAL PROJECT T1 2017
*
* GROUP 13:
*           1. REBECCA FRITH (ID: 213582268)
*           2. ERIC GRIGSON (ID: 212415996)
*           3. BENJAMIN FRIEBE (ID: 217109315)    
*
* ------------------------------------------------------------------------------------------------------
* FILE NAME:        CHECKOUTFACILITATOR.CS
* FILE DESCRIPTION: This class contains functionality to assist the checkout process for the online shop.
*                   It allows the system to add items to a cart and facilitate a full checkout process.
*
********************************************************************************************************/
using EmbrOnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EmbrOnlineStore.Controllers.Utilities
{
    /// <summary>
    /// Exception class for any check out errors.
    /// </summary>
    public class CheckoutException : Exception
    {
        public CheckoutException()
        {
        }
    }
    public abstract class CheckoutFacilitator
    {
        // todo: Testing
        /// <summary>
        /// Get total price of items currently in cart.
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public static double GetCartTotal(Dictionary<Item, int> cart)
        {
            double cartTotal = 0.0;

            foreach (var v in cart)
            {
                cartTotal += (v.Key.sellingPrice * v.Value);

            }

            return cartTotal;
        }
        // todo: Testing
        /// <summary>
        /// Add an item to the cart. Takes an item and its quantity and adds it to the cart object in the model.
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="quantity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Dictionary<Item, int> AddItemToCart(Item currentItem, int quantity, ShopModel model)
        {
            // if our shopping cart already contains the item we don't want to
            // duplicate it. so we will add the new quantity to the old quantity.
            if (model.shoppingCart.ContainsKey(currentItem))
            {
                model.shoppingCart[currentItem] += (quantity);
            }
            else
            {
                model.shoppingCart.Add(currentItem, (quantity)); // doesn't exist, add new entry.
            }
            return model.shoppingCart; // return updated shopping cart.
        }

        // todo: Testing
        /// <summary>
        /// Remove an item from the shopping cart. Uses an item as input to identify and remove.
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Dictionary<Item, int> RemoveItemFromCart(Item currentItem, ShopModel model)
        {
            // search our shopping cart and remove if present.
            if (model.shoppingCart.ContainsKey(currentItem))
            {
                model.shoppingCart.Remove(currentItem);
            }
            else { /* item doesnt exist, do nothing */}

            return model.shoppingCart;// return updated shopping cart.
        }

        // todo: Testing
        /// <summary>
        /// Edit the quantity of an item in the shopping cart. Takes the new quantity and replaces the old item 
        /// quantity in the shopping cart.
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="quantity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Dictionary<Item, int> EditQuantity(Item currentItem, int quantity, ShopModel model)
        {
            // Find item in shopping cart and assign it the new quantity.
            if (model.shoppingCart.ContainsKey(currentItem))
            {
                if (quantity < 0)
                {
                    model.shoppingCart[currentItem] = 0; // we don't want it to go into negative numbers since you can't buy a negative qty...
                }
                else
                {
                    model.shoppingCart[currentItem] = (quantity);
                }
            }

            return model.shoppingCart; // return updated shopping cart.
        }

        /// <summary>
        /// Creates an order in the model and database. Also creates a customer object or retrieves an existing on from the database.
        ///  Returns a receipt object.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="shippingStreet"></param>
        /// <param name="shippingCity"></param>
        /// <param name="shippingPostcode"></param>
        /// <param name="shippingState"></param>
        /// <param name="billingStreet"></param>
        /// <param name="billingCity"></param>
        /// <param name="billingPostcode"></param>
        /// <param name="billingState"></param>
        /// <param name="billingMethod"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Receipt CreateOrder(string firstName, string lastName, string email, string phone, string shippingStreet, string shippingCity,
           string shippingPostcode, string shippingState, string billingStreet, string billingCity, string billingPostcode, string billingState, string billingMethod, ShopModel model)
        {
            // first, create customer object
            Customer customer = new Customer();
            customer.name = lastName + ", " + firstName;
            customer.phone = phone;
            customer.address = billingStreet + ", " + billingCity + ", " + billingState + ", " + billingPostcode;
            customer.email = email;

            customer.paymentMethod = PayentMethodEnum.PayPal; //make dynamic

            customer.customerID = GetExistingCustomerID(customer);
            if (customer.customerID == -1)
                customer.customerID = CreateCustomerInDatabase(customer); // if customer doesn't exist, create it in the db!

            // create initial order object
            Order order = new Order();
            order.deliveryAddress = shippingStreet + ", " + shippingCity + ", " + shippingState + ", " + shippingPostcode;
            order.date = DateTime.Now;
            order.status = OrderStatus.Pending;

            // add order line items to order
            order.orderLineItems = new System.Collections.Generic.List<OrderLine>();
            foreach (var cartItem in model.shoppingCart)
            {
                OrderLine ol = new OrderLine();
                ol.item = cartItem.Key;
                ol.price = cartItem.Key.sellingPrice;
                ol.quantity = cartItem.Value;
                order.orderLineItems.Add(ol);
            }

            //TODO:  Commit to database -- customer and order, then read back order id  / customer id
            order.orderID = CreateOrderInDatabase(order, customer.customerID);
            if (order.orderID != -1)
            {
                // create order line items
                CreateOrderLineItemsInDatabase(order.orderLineItems, order.orderID);
            }
            else
            {
                throw new CheckoutException();
            }
            // create a receipt
            return new Receipt(order, customer);

        }
        /// <summary>
        /// Check if customer exists in database, if so, return existing customer id.
        /// </summary>
        /// <returns></returns>
        private static int GetExistingCustomerID(Customer customer)
        {
            int customerid = -1; ///-1 indicates not found.
            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect

            string query = "SELECT [CUSTOMER_ID] FROM [DBO].[Customer] WHERE LOWER([NAME]) = @NAME AND  LOWER([EMAIL]) = @EMAIL AND  LOWER([PHONE]) = @PHONE"; //sql query
            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {
                cmd.Parameters.Add("@NAME", SqlDbType.VarChar, 100).Value = customer.name.ToLower();
                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = customer.email.ToLower();
                cmd.Parameters.Add("@PHONE", SqlDbType.VarChar, 50).Value = customer.phone.ToLower();

                using (SqlDataReader reader = cmd.ExecuteReader()) // execute query
                {
                    while (reader.Read()) // iterate through results
                    {
                        // catch any exception arising from force parse
                        try
                        {
                            customerid = Int32.Parse(reader[0].ToString());
                        }
                        catch
                        {
                            customerid = -1;
                        }
                       
                    }
                }
            }
            // Close database connection

            database.CloseDatabaseConnection();

            return customerid; // customer exists!
        }

        /// <summary>
        /// Create customer entry in database and retun the newly created custome id.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private static int CreateCustomerInDatabase(Customer customer)
        {
            int customerid = -1;

            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect
            string query = "INSERT INTO [DBO].[CUSTOMER] (NAME, ADDRESS,BILLING_METHOD, EMAIL, PHONE) " +
                "VALUES (@NAME, @ADDRESS,@BILLING_METHOD, @EMAIL, @PHONE)";

            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {
                cmd.Parameters.Add("@NAME", SqlDbType.VarChar, 100).Value = customer.name;
                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 50).Value = customer.email;
                cmd.Parameters.Add("@PHONE", SqlDbType.VarChar, 50).Value = customer.phone;
                cmd.Parameters.Add("@BILLING_METHOD", SqlDbType.VarChar, 50).Value = customer.paymentMethod;
                cmd.Parameters.Add("@ADDRESS", SqlDbType.VarChar, 150).Value = customer.address;

                int result = cmd.ExecuteNonQuery();
            }
            // Close database connection

            database.CloseDatabaseConnection();

            return GetExistingCustomerID(customer);
        }

        /// <summary>
        /// Creates a new order in the database, returning the newly created Order ID.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        private static int CreateOrderInDatabase(Order order, int customerID)
        {
            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect

            string query = "INSERT INTO [DBO].[ORDER] (DATE, STATUS,DELIVERY_ADDRESS, CUSTOMER_ID) " +
                "VALUES (@DATE, @STATUS,@DELIVERY_ADDRESS, @CUSTOMER_ID)";

            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {
                cmd.Parameters.Add("@DATE", SqlDbType.DateTime, 100).Value = order.date;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar, 25).Value = order.status;
                cmd.Parameters.Add("@DELIVERY_ADDRESS", SqlDbType.VarChar, 150).Value = order.deliveryAddress;
                cmd.Parameters.Add("@CUSTOMER_ID", SqlDbType.VarChar, 50).Value = customerID;

                int result = cmd.ExecuteNonQuery();
            }
            // Close database connection

            database.CloseDatabaseConnection();

            return GetExistingOrderID(order, customerID);
        }

        /// <summary>
        /// Check if order exists in database, if so, return existing order id.
        /// </summary>
        /// <returns></returns>
        private static int GetExistingOrderID(Order order, int customerID)
        {
            int orderid = -1; ///-1 indicates not found.
            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect

            string query = "SELECT [ORDER_ID] FROM [DBO].[order] WHERE [DATE] = @DATE AND  LOWER([STATUS]) = @STATUS AND  LOWER([DELIVERY_ADDRESS]) = @DELIVERY_ADDRESS AND CUSTOMER_ID = @CUSTOMER_ID"; //sql query
            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {
                cmd.Parameters.Add("@DATE", SqlDbType.DateTime, 100).Value = order.date;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar, 25).Value = order.status;
                cmd.Parameters.Add("@DELIVERY_ADDRESS", SqlDbType.VarChar, 150).Value = order.deliveryAddress;
                cmd.Parameters.Add("@CUSTOMER_ID", SqlDbType.VarChar, 50).Value = customerID;

                using (SqlDataReader reader = cmd.ExecuteReader()) // execute query
                {
                    while (reader.Read()) // iterate through results
                    {
                        // catch any exception arising from force parse
                        try
                        {
                            orderid = Int32.Parse(reader[0].ToString());
                        }
                        catch
                        {
                            orderid = -1;
                        }
                    }
                }
            }
            // Close database connection

            database.CloseDatabaseConnection();

            return orderid; // customer exists!
        }

        /// <summary>
        /// Using the existing order line items and newly created order id, insert
        /// the order line items into the database. Force references the item ID / order ID
        /// foreign key
        /// </summary>
        /// <param name="lineItems"></param>
        /// <param name="orderID"></param>
        private static void CreateOrderLineItemsInDatabase(List<OrderLine> lineItems, int orderID)
        {
            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect

            // foreach line item in order line items, add to database one by one.
            foreach (var line in lineItems)
            {
                string query = "INSERT INTO [DBO].[ORDER_LINE_ITEM] (ORDER_ID, ITEM_ID,QUANTITY, PRICE) " +
                    "VALUES (@ORDER_ID, @ITEM_ID,@QUANTITY, @PRICE)";

                using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
                {
                    cmd.Parameters.Add("@ORDER_ID", SqlDbType.Int, 50).Value = orderID;
                    cmd.Parameters.Add("@ITEM_ID", SqlDbType.Int, 50).Value = line.item.itemID;
                    cmd.Parameters.Add("@QUANTITY", SqlDbType.Int, 50).Value = line.quantity;
                    cmd.Parameters.Add("@PRICE", SqlDbType.Decimal, 50).Value = line.price;
                    int result = cmd.ExecuteNonQuery();
                }
                // Close database connection
            }
            database.CloseDatabaseConnection();

        }
    }
}
