using Fido2NetLib;
using Fido2NetLib.Objects;
using Pulse.WebAuthn.Domain.Credentials;
using Pulse.WebAuthn.Domain.Credentials.Services;
using Pulse.WebAuthn.Domain.Customers;
using Pulse.WebAuthn.Domain.Customers.Services;

namespace Pulse.WebAuthn.Application.Credentials
{
    /// <summary>
    /// Represents the credential factory implementation.
    /// </summary>
    public class CredentialFactory : ICredentialFactory
    {
        #region Fields

        private readonly ICredentialService _credentialService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Constructors

        public CredentialFactory(ICredentialService credentialService,
            ICustomerService customerService)
        {
            _credentialService = credentialService;
            _customerService = customerService;
        }

        #endregion

        #region Methods

        public List<PublicKeyCredentialDescriptor> BuildPublicKeyCredentialDescriptors(ICollection<Credential> credentials)
        {
            // If the credentials list is null or empty, return an empty list
            if (credentials == null || credentials.Count == 0)
                return new List<PublicKeyCredentialDescriptor>();
            // Initialize a list to hold the PublicKeyCredentialDescriptors
            var publicKeyCredentialDescriptors = new List<PublicKeyCredentialDescriptor>();
            // Iterate through each credential and create a PublicKeyCredentialDescriptor
            foreach (var credential in credentials)
            {
                // Deserialize the descriptor from the credential descrtiptor string
                var descriptor = credential.Descriptor;

                if (descriptor is not null)
                    publicKeyCredentialDescriptors.Add(descriptor);
            }

            return publicKeyCredentialDescriptors;
        }

        public (PublicKeyCredentialDescriptor descriptor, uint signatureCount, byte[] pubKey) GetCredentialByCredentialId(byte[] credentialId)
        {
            var credentials = _credentialService.GetCredentialsByCredentialId(credentialId);
            var credential = credentials.FirstOrDefault();

            if (credential == null)
            {
                return (null!, 0, null!);
            }

            var publicKeyCredentialDescriptor = BuildPublicKeyCredentialDescriptors(credentials).FirstOrDefault();

            return (publicKeyCredentialDescriptor, credential.SignCount, credential.PublicKey);
        }

        public Task<List<Customer>> GetUsersByCredentialId(byte[] credentialId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a credential to storage.
        /// </summary>
        /// <param name = "user" > User </ param >
        /// < param name="credential">Credential</param>
        /// <returns>Stored Credential</returns>
        public Credential InsertCredential(Fido2User user, Credential credential)
        {
            var customer = _customerService.GetCustomerByName(user.Name);
            credential.CustomerId = customer.Id;

            _credentialService.InsertCredential(credential);

            return credential;
        }

        /// <summary>
        /// Checks if a credential is unique to a customer.
        /// </summary>
        /// <param name="credentialId">Credential identifier</param>
        /// <returns>True, if unique false otherwise</returns>
        public async Task<bool> IsCredentialUniqueToCustomer(byte[] credentialId)
        {
            var credentials = _credentialService.GetCredentialsByCredentialId(credentialId);

            return (credentials.Count <= 0);
        }

        public async Task<bool> IsUserHandleOwnerOfCredentialId(byte[] credentialId, byte[] userHandle)
        {
            var credentials = _credentialService.GetCredentialsByUserHandle(userHandle);

            if (credentials == null || credentials.Count == 0)
                return false;

            List<PublicKeyCredentialDescriptor> storedCredential = BuildPublicKeyCredentialDescriptors(credentials);

            return storedCredential.Exists(credential => credential.Id.SequenceEqual(credentialId));
        }

        public void UpdateCounter(byte[] credentialId, uint counter)
        {
            var credential = _credentialService.GetCredentialsByCredentialId(credentialId).FirstOrDefault();

            credential.SignCount = counter;

            _credentialService.UpdateCredential(credential);
        }

        #endregion
    }
}
