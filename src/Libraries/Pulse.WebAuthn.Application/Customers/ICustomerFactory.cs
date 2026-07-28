using Pulse.WebAuthn.Application.Shared;

namespace Pulse.WebAuthn.Application.Customers
{
    /// <summary>
    /// Represents the customer factory interface.
    /// </summary>
    public interface ICustomerFactory
    {
        /// <summary>
        /// Gets a customer with credentials
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Customer model</returns>
        CustomerModel GetCustomerWithCredentials(string username);
    }
}
