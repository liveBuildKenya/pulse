namespace Pulse.WebAuthn.Domain.Credentials.Services
{
    /// <summary>
    /// Represents a credential service interface.
    /// </summary>
    public interface ICredentialService
    {
        /// <summary>
        /// Gets credentials by customer identifier
        /// </summary>
        /// <param name="customerId">Customer Identifier</param>
        /// <returns>Credentials</returns>
        List<Credential> GetCredentialsByCustomerId(byte[] customerId);

        /// <summary>
        /// Gets credentials by credential identifier
        /// </summary>
        /// <param name="credentialId">Credential id</param>
        /// <returns>Credentials</returns>
        List<Credential> GetCredentialsByCredentialId(byte[] credentialId);

        /// <summary>
        /// Inserts a credential
        /// </summary>
        /// <param name="credential">Credential</param>
        void InsertCredential(Credential credential);
        List<Credential> GetCredentialsByUserHandle(byte[] userHandle);
        void UpdateCredential(Credential credential);
    }
}
