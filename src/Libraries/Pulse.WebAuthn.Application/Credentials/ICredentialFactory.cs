using Fido2NetLib.Objects;
using Pulse.WebAuthn.Domain.Credentials;

namespace Pulse.WebAuthn.Application.Credentials
{
    /// <summary>
    /// Represents the credentials factory interface.
    /// </summary>
    public interface ICredentialFactory
    {
        /// <summary>
        /// Builds a list of PublicKeyCredentialDescriptor from a list of Credentials.
        /// </summary>
        /// <param name="credentials">Credentials</param>
        /// <returns>A list of public key credential descriptors</returns>
        List<PublicKeyCredentialDescriptor> BuildPublicKeyCredentialDescriptors(ICollection<Credential> credentials);

        /// <summary>
        /// Checks if a credential is unique to a customer.
        /// </summary>
        /// <param name="credentialId">Credential identifier</param>
        /// <returns></returns>
        Task<bool> IsCredentialUniqueToCustomer(byte[] credentialId);

        /// <summary>
        /// Checks if a user handle is the owner of a specific credential ID.
        /// </summary>
        /// <param name="credentialId">Credential Identifier</param>
        /// <param name="userHandle">User Handle</param>
        /// <returns></returns>
        Task<bool> IsUserHandleOwnerOfCredentialId(byte[] credentialId, byte[] userHandle);

        /// <summary>
        /// Inserts a credential to storage
        /// </summary>
        /// <param name="user">Fido User</param>
        /// <param name="credential">Credential</param>
        /// <returns>Stored Credential</returns>
        //StoredCredential InsertCredential(Fido2User user, StoredCredential credential);

        (PublicKeyCredentialDescriptor descriptor, uint signatureCount, byte[] pubKey) GetCredentialByCredentialIdAndSignatureCount(byte[] credentialId);
        void UpdateCounter(byte[] credentialId, uint counter);
    }
}
