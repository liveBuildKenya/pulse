using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Http;
using Pulse.WebAuthn.Application.Assertion.Models;
using Pulse.WebAuthn.Application.Credentials;
using Pulse.WebAuthn.Application.Customers;
using Pulse.WebAuthn.Application.Shared;
using System.Text.Json;

namespace Pulse.WebAuthn.Application.Assertion.Services
{
    /// <summary>
    /// Represents an assertion use case implementation.
    /// </summary>
    public class AssertionUseCase : IAssertionUseCase
    {
        #region Fields

        private readonly ICustomerFactory customerFactory;
        private readonly ICredentialFactory credentialFactory;
        private readonly IFido2 fido2;
        private readonly IHttpContextAccessor httpContextAccessor;

        #endregion

        #region Constructor

        public AssertionUseCase(ICustomerFactory customerFactory,
            ICredentialFactory credentialFactory,
            IFido2 fido2,
            IHttpContextAccessor httpContextAccessor)
        {
            this.customerFactory = customerFactory;
            this.credentialFactory = credentialFactory;
            this.fido2 = fido2;
            this.httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Makes assertion options based on the provided request model.
        /// </summary>
        /// <param name="assertionOptionsRequestModel">Assertion options request modlel</param>
        /// <returns>Assertion options</returns>
        /// <exception cref="ArgumentException"></exception>
        public IResult MakeAssertionOptions(AssertionOptionsRequestModel assertionOptionsRequestModel)
        {
            var extensions = new AuthenticationExtensionsClientInputs()
            {
                UserVerificationMethod = true,
            };

            var assertionOptionsParams = new GetAssertionOptionsParams
            {
                AllowedCredentials = null,
                Extensions = extensions,
                UserVerification = UserVerificationRequirement.Preferred
            };

            var options = fido2.GetAssertionOptions(assertionOptionsParams);

            httpContextAccessor.HttpContext.Session.SetString("fido2.assertionOptions", options.ToJson());

            return Results.Ok(options);
        }

        /// <summary>
        /// Makes assertion based on the provided authenticator assertion raw response.
        /// </summary>
        /// <param name="authenticatorAssertionRawResponse">Authenticator assertion raw response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Assertion result</returns>
        public async Task<IResult> MakeAssertion(AuthenticatorAssertionRawResponse authenticatorAssertionRawResponse, CancellationToken cancellationToken)
        {
            var options = AssertionOptions.FromJson(httpContextAccessor.HttpContext.Session.GetString("fido2.assertionOptions"));

            var authenticatorResponse = JsonSerializer.Deserialize<AuthenticatorResponse>(authenticatorAssertionRawResponse.Response.ClientDataJson);

            //TODO: Get Credentials in db
            var makeAssertionParams = new MakeAssertionParams
            {
                AssertionResponse = authenticatorAssertionRawResponse,
                OriginalOptions = options,
                StoredPublicKey = null, // would be the stored public key
                StoredSignatureCounter = 0, //would be the stored signed count
                IsUserHandleOwnerOfCredentialIdCallback = UserHandleOwnerOfCredentialIdAsync
            };

            var response = await fido2.MakeAssertionAsync(makeAssertionParams, cancellationToken);

            //TODO: Handle JWT

            return Results.Ok(options);
        }

        private static async Task<bool> UserHandleOwnerOfCredentialIdAsync(IsUserHandleOwnerOfCredentialIdParams args, CancellationToken cancellationToken)
        {
            //var storedCreds = await _demoStorage.GetCredentialsByUserHandleAsync(args.UserHandle, cancellationToken);
            //return storedCreds.Exists(c => c.Descriptor.Id.SequenceEqual(args.CredentialId));
            return false;
        }

        #endregion
    }
}
