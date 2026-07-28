namespace Pulse.WebAuthn.Application.Assertion.Models
{
    /// <summary>
    /// Represents the assertion options request model
    /// </summary>
    public class AssertionOptionsRequestModel
    {
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user verification method
        /// </summary>
        public string UserVerification { get; set; }
    }
}
