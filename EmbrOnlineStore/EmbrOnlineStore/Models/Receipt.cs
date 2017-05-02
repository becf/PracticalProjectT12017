/*******************************************************************************************************
* SIT782 - PRACTICAL PROJECT T1 2017
*
* GROUP 13:
*           1. REBECCA FRITH (ID: 213582268)
*           2. ERIC GRIGSON (ID: 212415996)
*           3. BENJAMIN FRIEBE (ID: 217109315)    
*
* ------------------------------------------------------------------------------------------------------
* FILE NAME:        RECEIPT.CS
* FILE DESCRIPTION: Data structure for generating a receipt. This is pretty close to the Order class.
********************************************************************************************************/
using System;

namespace EmbrOnlineStore.Models
{
    public class Receipt
    {
        public int receiptID { get; set; } // an ID for the receipt
        public Order order { get; set; } // order associated with receipt
        public Customer customer { get; set; } // customer for the order
        public int totalPrice { get; set; } // total price of order
        public DateTime billingDate { get; set; } // billing date of order

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
