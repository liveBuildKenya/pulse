using Pulse.WebAuthn.Domain.Shared;

namespace Pulse.WebAuthn.Domain.Customers
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    public class Customer : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        public byte[] Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer account.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name of the customer account
        /// </summary>
        public string DisplayName { get; set; }

    }
}
