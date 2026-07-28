using Pulse.WebAuthn.Application.Shared;
using Pulse.WebAuthn.Domain.Credentials.Services;
using Pulse.WebAuthn.Domain.Customers;
using Pulse.WebAuthn.Domain.Customers.Services;
using System.Text;

namespace Pulse.WebAuthn.Application.Customers
{
    /// <summary>
    /// Represents the customer factory implementation.
    /// </summary>
    public class CustomerFactory : ICustomerFactory
    {
        #region Fields

        private readonly ICustomerService customerService;
        private readonly ICredentialService credentialService;

        #endregion

        #region Constructor

        public CustomerFactory(ICustomerService customerService,
            ICredentialService credentialService)
        {
            this.customerService = customerService;
            this.credentialService = credentialService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a customer with credentials by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Customer model with a customer and credentials property</returns>
        /// <exception cref="ArgumentException">Checks the presence of a username parameter</exception>
        public CustomerModel GetCustomerWithCredentials(string username)
        {
            //Check if the username parameter is null or empty
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("Username cannot be null or empty.", nameof(username));
            }
            // Initialize the customer model
            var customerModel = new CustomerModel();
            // Retrieve the customer by username
            customerModel.Customer = customerService.GetCustomerByName(username);
            // Check it the customer is not found
            if (customerModel.Customer is null)
            {
                var customer = new Customer
                {
                    Name = username,
                    DisplayName = username,
                    Id = Encoding.UTF8.GetBytes(username)
                };

                customerService.InsertCustomer(customer);

                customerModel.Customer = customer;

                return customerModel;
            }
            //Retrieve the customers credentials
            customerModel.Credentials = credentialService.GetCredentialsByCustomerId(customerModel.Customer.Id);

            return customerModel;
        }

        #endregion
    }
}
