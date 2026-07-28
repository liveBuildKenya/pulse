using Fido2NetLib.Objects;
using Pulse.WebAuthn.Domain.Credentials;
using Pulse.WebAuthn.Domain.Credentials.Services;
using System.Text.Json;

namespace Pulse.WebAuthn.Application.Credentials
{
    /// <summary>
    /// Represents the credential factory implementation.
    /// </summary>
    public class CredentialFactory : ICredentialFactory
    {
        #region Fields

        private readonly ICredentialService credentialService;

        #endregion

        #region Constructors

        public CredentialFactory(ICredentialService credentialService)
        {
            this.credentialService = credentialService;
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
                var descriptor = JsonSerializer.Deserialize<PublicKeyCredentialDescriptor>(credential.Descriptor);

                if (descriptor is not null)
                    publicKeyCredentialDescriptors.Add(descriptor);
            }

            return publicKeyCredentialDescriptors;
        }

        public (PublicKeyCredentialDescriptor descriptor, uint signatureCount, byte[] pubKey) GetCredentialByCredentialIdAndSignatureCount(byte[] credentialId)
        {
            var credentials = credentialService.GetCredentialsByCredentialId(credentialId);
            var credential = credentials.FirstOrDefault();

            var publicKeyCredentialDescriptor = BuildPublicKeyCredentialDescriptors(credentials).FirstOrDefault();


            return (publicKeyCredentialDescriptor, credential.SignatureCounter, credential.PublicKey);
        }

        /// <summary>
        /// Inserts a credential to storage.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="storedCredential">Credential</param>
        /// <returns>Stored Credential</returns>
        //public StoredCredential InsertCredential(Fido2User user, StoredCredential storedCredential)
        //{
        //    var credential = new Credential
        //    {
        //        UserHandle = storedCredential.UserHandle,
        //        PublicKey = storedCredential.PublicKey,
        //        Descriptor = JsonSerializer.Serialize(storedCredential.Descriptor),
        //        AaGuid = storedCredential.AaGuid,
        //        CredType = storedCredential.CredType,
        //        CustomerId = user.Id,
        //        RegDate = storedCredential.RegDate,
        //        SignatureCounter = storedCredential.SignatureCounter
        //    };

        //    credentialService.InsertCredential(credential);

        //    return storedCredential;
        //}

        /// <summary>
        /// Checks if a credential is unique to a customer.
        /// </summary>
        /// <param name="credentialId">Credential identifier</param>
        /// <returns>True, if unique false otherwise</returns>
        public Task<bool> IsCredentialUniqueToCustomer(byte[] credentialId)
        {
            var credentials = credentialService.GetCredentialsByCredentialId(credentialId);

            return Task.FromResult(!(credentials.Count > 0));
        }

        public Task<bool> IsUserHandleOwnerOfCredentialId(byte[] credentialId, byte[] userHandle)
        {
            var credentials = credentialService.GetCredentialsByUserHandle(userHandle);

            if (credentials == null || credentials.Count == 0)
                return Task.FromResult(false);

            List<PublicKeyCredentialDescriptor> storedCredential = BuildPublicKeyCredentialDescriptors(credentials);

            //return storedCredential.Exists(credential => credential.Id.SequenceEqual(credentialId));
            return Task.FromResult(true);
        }

        public void UpdateCounter(byte[] credentialId, uint counter)
        {
            var credential = credentialService.GetCredentialsByCredentialId(credentialId).FirstOrDefault();

            credential.SignatureCounter = counter;

            credentialService.UpdateCredential(credential);
        }

        #endregion
    }
}
