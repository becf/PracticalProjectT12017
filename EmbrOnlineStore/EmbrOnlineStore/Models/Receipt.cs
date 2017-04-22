using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Models
{
    public class Receipt
    {
        int receiptID { get; set; }
        Order order { get; set; }
        Customer customer { get; set; }
        int totalPrice { get; set; }
        DateTime billingDate { get; set; }

        /// <summary>
        /// Empty constructor for Receipt object.
        /// </summary>
        Receipt() { }
        /// <summary>
        /// Constructor that takes an order and a customer and generates a receipt. 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="customer"></param>
        Receipt(Order order, Customer customer)
        {
            billingDate = DateTime.Now; // billing date is the date of the creation of the object.
            totalPrice = 0;
            // calculate total price by adding together all order line items
            foreach (OrderLine l in order.orderLineItems)
            {
                totalPrice += (int)(l.price * l.quantity);
            }
            this.order = order;
            this.customer = customer;
        }
    }
}
