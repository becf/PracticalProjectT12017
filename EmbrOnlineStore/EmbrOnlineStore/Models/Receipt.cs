using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Models
{
    public class Receipt
    {
        public int receiptID { get; set; }
        public Order order { get; set; }
        public Customer customer { get; set; }
        public int totalPrice { get; set; }
        public DateTime billingDate { get; set; }

        /// <summary>
        /// Empty constructor for Receipt object.
        /// </summary>
        public Receipt() { }
        /// <summary>
        /// Constructor that takes an order and a customer and generates a receipt. 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="customer"></param>
        public Receipt(Order order, Customer customer)
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
