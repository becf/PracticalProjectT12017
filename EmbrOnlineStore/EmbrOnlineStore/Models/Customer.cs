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
        public int customerID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public PayentMethodEnum paymentMethod { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

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