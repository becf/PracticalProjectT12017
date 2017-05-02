﻿/*******************************************************************************************************
* SIT782 - PRACTICAL PROJECT T1 2017
*
* GROUP 13:
*           1. REBECCA FRITH (ID: 213582268)
*           2. ERIC GRIGSON (ID: 212415996)
*           3. BENJAMIN FRIEBE (ID: 217109315)    
*
* ------------------------------------------------------------------------------------------------------
* FILE NAME:        CUSTOMER.CS
* FILE DESCRIPTION: Data structure for Customer object. Closely mimics the database structure.
********************************************************************************************************/
using System.Collections.Generic;

namespace EmbrOnlineStore.Models
{
    public enum PayentMethodEnum
    {
        PayPal,
        CreditCard
    }
    public class Customer
    {
        public int customerID { get; set; } // the customer's identifier - auto generated by DB
        public string name { get; set; } // customer name
        public string address { get; set; } // customer's address
        public PayentMethodEnum paymentMethod { get; set; } // the customer's payment method - either PayPal or CreditCard 
        public string email { get; set; } // customer email address
        public string phone { get; set; } // customer phone number

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Customer()
        {

        }
        /// <summary>
        /// Retrieves a customer using their customerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer GetCustomer(int customerID)
        {
            return new Customer();
        }
        /// <summary>
        /// Retrieves a customer using their email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Customer GetCustomer_Email(string email)
        {
            return new Customer();
        }

        /// <summary>
        /// Retrieves a customer using their telephone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Customer GetCustomer_Phone(string phoneNumber)
        {
            return new Customer();
        }
        /// <summary>
        /// Gets a list containing all customers.
        /// </summary>
        /// <returns>A list of customers</returns>
        protected List<Customer> GetAllCustomers()
        {
            //populate from DB
            return new List<Customer>();
        }
        /// <summary>
        /// Deletes a customer by their ID.
        /// </summary>
        /// <param name="customerID"></param>
        protected void DeleteCustomer(int customerID)
        {
            // populate later if required.
        }
    }
}