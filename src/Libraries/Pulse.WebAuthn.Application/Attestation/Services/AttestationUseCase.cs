using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Http;
using Pulse.WebAuthn.Application.Attestation.Models;
using Pulse.WebAuthn.Application.Credentials;
using Pulse.WebAuthn.Application.Customers;
using Pulse.WebAuthn.Domain.Credentials;
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
            var username = attestationOptionsRequestModel.Username;

            if (string.IsNullOrEmpty(username))
            {
                username = $"Usernameless created at {created}";
            }

            var customerModel = customerFactory.GetCustomerWithCredentials(username);

            //Create a new Fido2User object
            var fidoUser = new Fido2User
            {
                DisplayName = "",
                Name = customerModel.Customer.Name,
                Id = Encoding.UTF8.GetBytes(customerModel.Customer.Name)
            };

            //Get the users existing keys by username
            var existingKeys = credentialFactory.BuildPublicKeyCredentialDescriptors(customerModel.Credentials);

            //Create options
            var authenticatorSelection = AuthenticatorSelection.Default;
            if (attestationOptionsRequestModel.AuthenticatorAttachment != null)
            {
                authenticatorSelection.AuthenticatorAttachment = attestationOptionsRequestModel.AuthenticatorAttachment;
            }

            if (attestationOptionsRequestModel.UserVerificationRequirement != null)
            {
                authenticatorSelection.UserVerification = attestationOptionsRequestModel.UserVerificationRequirement.Value;
            }
            else
            {
                authenticatorSelection.UserVerification = UserVerificationRequirement.Preferred;
            }

            if (attestationOptionsRequestModel.ResidentKeyRequirement != null)
            {
                authenticatorSelection.ResidentKey = attestationOptionsRequestModel.ResidentKeyRequirement.Value;
            }
            else
            {
                authenticatorSelection.ResidentKey = ResidentKeyRequirement.Preferred;
            }

            // 4. Create options
            var options = fido2.RequestNewCredential(new RequestNewCredentialParams
            {
                User = fidoUser,
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
            httpContextAccessor.HttpContext?.Session?.SetString("fido2.attestationOptions", options.ToString());

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
            var jsonOptions = httpContextAccessor.HttpContext?.Session?.GetString("fido2.attestationOptions");
            if (string.IsNullOrEmpty(jsonOptions))
            {
                return Results.BadRequest(new { message = "Attestation options expired or not found in session." });
            }

            var options = CredentialCreateOptions.FromJson(jsonOptions);

            var makeNewCredentialsParams = new MakeNewCredentialParams
            {
                AttestationResponse = authenticatorAttestationRawResponse,
                IsCredentialIdUniqueToUserCallback = CredentialIdUniqueToUserAsync,
                OriginalOptions = options
            };

            var registeredPublicKeyCredential = await fido2.MakeNewCredentialAsync(makeNewCredentialsParams, cancellationToken: cancellationToken);

            var credential = credentialFactory.InsertCredential(options.User, new Credential
            {
                AttestationFormat = registeredPublicKeyCredential.AttestationFormat,
                Id = registeredPublicKeyCredential.Id,
                PublicKey = registeredPublicKeyCredential.PublicKey,
                UserHandle = registeredPublicKeyCredential.User.Id,
                SignCount = registeredPublicKeyCredential.SignCount,
                RegDate = DateTime.UtcNow,
                AaGuid = registeredPublicKeyCredential.AaGuid,
                Transports = registeredPublicKeyCredential.Transports,
                IsBackupEligible = registeredPublicKeyCredential.IsBackupEligible,
                IsBackedUp = registeredPublicKeyCredential.IsBackedUp,
                AttestationObject = registeredPublicKeyCredential.AttestationObject,
                AttestationClientDataJson = registeredPublicKeyCredential.AttestationClientDataJson,
            });

            return Results.Ok();
        }

        private async Task<bool> CredentialIdUniqueToUserAsync(IsCredentialIdUniqueToUserParams args, CancellationToken cancellationToken)
        {
            return await credentialFactory.IsCredentialUniqueToCustomer(args.CredentialId);
        }

        #endregion
    }
}
