using Pulse.WebAuthn.Domain.Infrastructure.Data;

namespace Pulse.WebAuthn.Domain.Customers.Services
{
    /// <summary>
    /// Represents the customer service implementation.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        #region Fields

        private readonly IRepository<Customer> customerRepository;

        #endregion

        #region Constructor

        public CustomerService(IRepository<Customer> customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a customer by identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Customer</returns>
        public Customer GetCustomerById(byte[] customerId)
        {
            return customerRepository.GetById(customerId);
        }

        /// <summary>
        /// Gets a customer by unique name eg emain, phone number or username
        /// </summary>
        /// <param name="name">customer unique name</param>
        /// <returns>Customer</returns>
        public Customer GetCustomerByName(string name)
        {
            return (from customer in customerRepository.Table
                    where customer.Name.ToLower() == name.ToLower()
                    select customer).FirstOrDefault();
        }

        /// <summary>
        /// Inserts a new customer
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <exception cref="ArgumentNullException">Thrown if customer argument is null</exception>
        public void InsertCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(Customer));

            customerRepository.Insert(customer);
        }

        #endregion
    }
}
