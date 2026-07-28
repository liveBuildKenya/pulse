namespace Pulse.WebAuthn.Domain.Customers.Services
{
    /// <summary>
    /// Represents a customer service interface.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Gets a customer by identifier
        /// </summary>
        /// <param name="customerId">Customer Identifer</param>
        /// <returns>Customer</returns>
        Customer GetCustomerById(byte[] customerId);

        /// <summary>
        /// Gets a customer by name
        /// </summary>
        /// <param name="name">Customer unique name eg email, phone number, username etc</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByName(string name);

        /// <summary>
        /// Inserts a new customer
        /// </summary>
        /// <param name="customer"></param>
        void InsertCustomer(Customer customer);

    }
}
