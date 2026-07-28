using Microsoft.EntityFrameworkCore;
using Pulse.WebAuthn.Domain.Infrastructure.Data;
using Pulse.WebAuthn.Domain.Shared.Utilities;

namespace Pulse.WebAuthn.Domain.Credentials.Services
{
    /// <summary>
    /// Represents a credential service implementation.
    /// </summary>
    public class CredentialService : ICredentialService
    {
        #region Field

        private readonly IRepository<Credential> credentialRepository;

        #endregion

        #region Constructor

        public CredentialService(IRepository<Credential> credentialRepository)
        {
            this.credentialRepository = credentialRepository;
        }

        #endregion

        #region Methods

        public List<Credential> GetCredentialsByCredentialId(byte[] credentialId)
        {
            var base64Id = Helpers.Base64UrlEncode(credentialId);
            var jsonFragment = $@"{{""id"":""{base64Id}""}}";

            return (from credential in credentialRepository.Table
                    where EF.Functions.JsonContains(credential.Descriptor, jsonFragment)
                    select credential)
                    .ToList();
        }

        /// <summary>
        /// Gets credentials by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Credentials</returns>
        public List<Credential> GetCredentialsByCustomerId(byte[] customerId)
        {
            return (from credential in credentialRepository.Table
                    where credential.CustomerId == customerId
                    select credential).ToList();
        }

        public List<Credential> GetCredentialsByUserHandle(byte[] userHandle)
        {
            return (from credential in credentialRepository.Table
                    where credential.UserHandle == userHandle
                    select credential).ToList();
        }

        /// <summary>
        /// Inserts a new credential
        /// </summary>
        /// <param name="credential">Credential</param>
        /// <exception cref="ArgumentNullException">Throw null exception if Credential is null</exception>
        public void InsertCredential(Credential credential)
        {
            if (credential == null)
                throw new ArgumentNullException(nameof(Credential));

            credentialRepository.Insert(credential);
        }

        public void UpdateCredential(Credential credential)
        {
            if (credential == null)
                throw new ArgumentNullException(nameof(Credential));

            credentialRepository.Update(credential);
        }

        #endregion
    }
}
