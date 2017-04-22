using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Models
{
    /// <summary>
    /// Order Status enumeration - indicates the status of an Order.
    /// </summary>
   public enum OrderStatus
    {
        New, // Newly created order
        Pending, // Waiting for something to happen
        Shipped, // Items have been sent
        Delivered, // Items have been delivered
        Closed // Order complete
    }
    public struct OrderLine
    {
      public  Item item;
       public double price;
        public int quantity;
    }
    public class Order
    {
        public int orderID { get; set; }
        public Customer customer {get;set;}
        public DateTime date { get; set; }
        public OrderStatus status { get; set; }
        public List<OrderLine> orderLineItems { get; set; }
        public string deliveryAddress { get; set; }

        /// <summary>
        /// Constructor that creates an empty new Order.
        /// </summary>
        public Order()
        {
            this.orderLineItems = new List<OrderLine>();
            this.status = OrderStatus.New;
            this.date = DateTime.Now;
        }

        /// <summary>
        /// Constructor for Order that takes existing Customer, delivery address and a list of 
        /// the items ordered.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="deliveryAddress"></param>
        /// <param name="orderLineItems"></param>
        public Order(Customer customer, string deliveryAddress, List<OrderLine> orderLineItems)
        {
            this.customer = customer;
            this.deliveryAddress = deliveryAddress;
            this.orderLineItems = orderLineItems;
            this.date = DateTime.Now;
            this.status = OrderStatus.New;
        }
    }
}
