using EmbrOnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Controllers.Utilities
{
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

            // create a receipt
            return new Receipt(order, customer);

        }
    }
}
