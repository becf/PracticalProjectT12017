/*******************************************************************************************************
* SIT782 - PRACTICAL PROJECT T1 2017
*
* GROUP 13:
*           1. REBECCA FRITH (ID: 213582268)
*           2. ERIC GRIGSON (ID: 212415996)
*           3. BENJAMIN FRIEBE (ID: 217109315)    
*
* ------------------------------------------------------------------------------------------------------
* FILE NAME:        ORDER.CS
* FILE DESCRIPTION: Data structure for Order object. Closely mimics the database structure.
********************************************************************************************************/
using System;
using System.Collections.Generic;

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
    /// <summary>
    /// Structure representing an order line item. 
    /// 
    /// Each contains one item, the item's price and the qty requested.
    /// </summary>
    public struct OrderLine
    {
        public Item item;
        public double price;
        public int quantity;
    }
    public class Order
    {
        public int orderID { get; set; } // order id-  auto populated by db
        public Customer customer { get; set; } // customer which ordered the items
        public DateTime date { get; set; } // date of order
        public OrderStatus status { get; set; } // status of order
        public List<OrderLine> orderLineItems { get; set; } // a list of order line items associated with this order
        public string deliveryAddress { get; set; } // delivery address

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
