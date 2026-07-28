using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Http;
using Pulse.WebAuthn.Application.Attestation.Models;
using Pulse.WebAuthn.Application.Credentials;
using Pulse.WebAuthn.Application.Customers;
using System.Text;

namespace Pulse.WebAuthn.Application.Attestation.Services
{
    /// <summary>
    /// Represents the attestation service implementation
    /// </summary>
    public class AttestationUseCase : IAttestationUseCase
    {
        #region Fields

        private readonly IFido2 fido2;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICustomerFactory customerFactory;
        private readonly ICredentialFactory credentialFactory;

        #endregion

        #region Constructors

        public AttestationUseCase(IFido2 fido2,
            IHttpContextAccessor httpContextAccessor,
            ICustomerFactory customerFactory,
            ICredentialFactory credentialFactory)
        {
            this.fido2 = fido2;
            this.httpContextAccessor = httpContextAccessor;
            this.customerFactory = customerFactory;
            this.credentialFactory = credentialFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Makes the attestation options for a new credential registration
        /// </summary>
        /// <param name="attestationOptionsRequestModel">Attestation options request model</param>
        /// <returns>Attestation options</returns>
        public IResult MakeAttestationOptions(AttestationOptionsRequestModel attestationOptionsRequestModel)
        {
            var created = DateTime.UtcNow;
            var username = $"User Created at {created}";

            //Create a new Fido2User object
            var user = new Fido2User
            {
                DisplayName = "",
                Name = username,
                Id = Encoding.UTF8.GetBytes(username)
            };
            //Get the users existing keys by username
            var existingKeys = new List<PublicKeyCredentialDescriptor>();
            //Create options
            var authenticatorSelection = AuthenticatorSelection.Default;
            if (attestationOptionsRequestModel.AuthenticatorAttachment != null)
            {
                authenticatorSelection.AuthenticatorAttachment = attestationOptionsRequestModel.AuthenticatorAttachment;
            }

            if (attestationOptionsRequestModel.UserVerificationRequirement != null)
            {
                authenticatorSelection.UserVerification = UserVerificationRequirement.Preferred;
            }

            if (attestationOptionsRequestModel.ResidentKeyRequirement != null)
            {
                authenticatorSelection.ResidentKey = ResidentKeyRequirement.Preferred;
            }

            // 4. Create options
            var options = fido2.RequestNewCredential(new RequestNewCredentialParams
            {
                User = user,
                ExcludeCredentials = existingKeys,
                AuthenticatorSelection = authenticatorSelection,
                AttestationPreference = attestationOptionsRequestModel.AttestationConveyancePreference ?? AttestationConveyancePreference.None,
                Extensions = new AuthenticationExtensionsClientInputs
                {
                    Extensions = true,
                    UserVerificationMethod = true,
                    CredProps = true
                }
            });

            //Store the options temporarily
            httpContextAccessor.HttpContext.Session.SetString("fido2.attestationOptions", options.ToString());

            return Results.Ok(options);
        }

        /// <summary>
        /// Makes the attestation for a new credential registration
        /// </summary>
        /// <param name="authenticatorAttestationRawResponse">Authenticator attestation raw response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Attestation Result</returns>
        public async Task<IResult> MakeAttestation(AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse, CancellationToken cancellationToken)
        {
            var jsonOptions = httpContextAccessor.HttpContext.Session.GetString("fido2.attestationOptions");
            var options = CredentialCreateOptions.FromJson(jsonOptions);
            //Create callback to check if the credential is unique to the customer
            IsCredentialIdUniqueToUserAsyncDelegate callback = async (args, cancellationToken) =>
            {
                var response = await credentialFactory.IsCredentialUniqueToCustomer(args.CredentialId);
                return response;
            };
            //Request for a new credential
            //var success = await fido2.MakeNewCredentialAsync(authenticatorAttestationRawResponse, options, callback, cancellationToken: cancellationToken);

            //var storedCredential = credentialFactory.InsertCredential(options.User, new StoredCredential
            //{
            //    Descriptor = new PublicKeyCredentialDescriptor(success.Result.CredentialId),
            //    PublicKey = success.Result.PublicKey,
            //    UserHandle = success.Result.User.Id,
            //    SignatureCounter = success.Result.Counter,
            //    CredType = success.Result.CredType,
            //    RegDate = DateTime.UtcNow,
            //    AaGuid = success.Result.Aaguid
            //});

            return Results.Ok(callback);
        }

        #endregion
    }
}
