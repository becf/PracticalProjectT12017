/*******************************************************************************************************
* SIT782 - PRACTICAL PROJECT T1 2017
*
* GROUP 13:
*           1. REBECCA FRITH (ID: 213582268)
*           2. ERIC GRIGSON (ID: 212415996)
*           3. BENJAMIN FRIEBE (ID: 217109315)    
*
* ------------------------------------------------------------------------------------------------------
* FILE NAME:        ORDERRETRIEVALFACILITATOR.CS
* FILE DESCRIPTION: Facilitates the retrieval of existing orders from the database.
*                   Allows user to get orders or customers by IDs or get all order line items for
*                   an existing order.
********************************************************************************************************/
using EmbrOnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EmbrOnlineStore.Controllers.Utilities
{
    public abstract class OrderRetrievalFacilicator
    {
        /// <summary>
        /// Retrieve order by its ID. 
        /// 
        /// First, get the order, ensuring it exists in the database.
        /// 
        /// Secondly, if order exists get all corresponding line items.
        /// 
        /// 
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns>returns null if not existent.</returns>
        public static Order GetOrderByID(int orderid)
        {
            Order order = null;

            // Initialise database connection

            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect
            // firstly, get order.
            string query = "SELECT * FROM [DBO].[Order] WHERE [ORDER_ID] = @ORDER_ID"; //sql query
            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {
                cmd.Parameters.Add("@ORDER_ID", SqlDbType.Int, 50).Value = orderid;

                using (SqlDataReader reader = cmd.ExecuteReader()) // execute query
                {
                    while (reader.Read()) // iterate through results - should only be 1.
                    {
                        order = new Order();
                        order.orderID = Int32.Parse(reader[0].ToString()); // get order ID
                        order.customer = new Customer();
                        order.customer.customerID = Int32.Parse(reader[1].ToString()); // get customer ID, populate rest later.
                        order.date = DateTime.Parse(reader[2].ToString());
                        // TODO; CONVERT TO SWITCH STATEMENT
                        if (reader[3].ToString() == "Pending")
                        {
                            order.status = OrderStatus.Pending;
                        }
                        else if (reader[3].ToString() == "Delivered")
                        {
                            order.status = OrderStatus.Delivered;
                        }
                        else if (reader[3].ToString() == "New")
                        {
                            order.status = OrderStatus.New;
                        }
                        else if (reader[3].ToString() == "Shipped")
                        {
                            order.status = OrderStatus.Shipped;
                        }
                        else if (reader[3].ToString() == "Closed")
                        {
                            order.status = OrderStatus.Closed;
                        }
                        order.deliveryAddress = reader[4].ToString();
                    }
                }
            }
            // Close database connection

            database.CloseDatabaseConnection();

            // Get customer using id from order
            order.customer = GetCustomerByID(order.customer.customerID);

            // get order line items
            order.orderLineItems = GetOrderLineItemsByOrderID(order.orderID);
            return order;
        }
        /// <summary>
        /// Gets all order line items associated with an order ID.
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        private static List<OrderLine> GetOrderLineItemsByOrderID(int orderid)
        {
            List<OrderLine> lineItems = new List<OrderLine>();
            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect
            // firstly, get order.
            string query = "SELECT * FROM [DBO].[ORDER_LINE_ITEM] WHERE [ORDER_ID] = @ORDER_ID"; //sql query
            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {
                cmd.Parameters.Add("@ORDER_ID", SqlDbType.Int, 50).Value = orderid;

                using (SqlDataReader reader = cmd.ExecuteReader()) // execute query
                {
                    while (reader.Read()) // iterate through results - should only be 1.
                    {
                        OrderLine ol = new OrderLine();
                        ol.item = ItemCatalogFacilitator.GetItemByID(Int32.Parse(reader[2].ToString()));
                        ol.quantity = Int32.Parse(reader[3].ToString());
                        ol.price = Double.Parse(reader[4].ToString());
                        lineItems.Add(ol);
                    }
                }
            }
            // Close database connection

            database.CloseDatabaseConnection();
            return lineItems;
        }
        /// <summary>
        /// Retrieves a customer from the database, using the customer's id.
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        private static Customer GetCustomerByID(int customerid)
        {
            Customer customer = null;
            // Initialise database connection

            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect
            // firstly, get order.
            string query = "SELECT * FROM [DBO].[CUSTOMER] WHERE [CUSTOMER_ID] = @CUSTOMER_ID"; //sql query
            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {
                cmd.Parameters.Add("@CUSTOMER_ID", SqlDbType.Int, 50).Value = customerid;

                using (SqlDataReader reader = cmd.ExecuteReader()) // execute query
                {
                    while (reader.Read()) // iterate through results - should only be 1.
                    {
                        customer = new Customer();
                        customer.customerID = Int32.Parse(reader[0].ToString()); // get customer ID                                                                      
                        customer.name = reader[1].ToString();
                        customer.address = reader[2].ToString();
                        switch (reader[3].ToString())
                        {
                            case "PayPal":
                                customer.paymentMethod = PayentMethodEnum.PayPal; break;
                            case "CreditCard":
                                customer.paymentMethod = PayentMethodEnum.CreditCard; break;
                            default:
                                customer.paymentMethod = PayentMethodEnum.PayPal; break;
                        }
                        customer.email = reader[4].ToString();
                        customer.phone = reader[5].ToString();
                    }
                }
            }
            // Close database connection

            database.CloseDatabaseConnection();
            return customer;
        }
    }
}
