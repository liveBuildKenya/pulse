namespace Pulse.WebAuthn.Domain.Shared
{
    /// <summary>
    /// Represents the base class
    /// </summary>
    public class BaseEntity
    {
        public BaseEntity()
        {
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the date created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date updated
        /// </summary>
        public DateTime DateUpdated { get; set; }
    }
}
