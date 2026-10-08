using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Pulse.WebAuthn.Application.Assertion.Models;
using Pulse.WebAuthn.Application.Credentials;
using Pulse.WebAuthn.Application.Customers;
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
        private readonly IDistributedCache distributedCache;
        private string assertionOptionsKey = "fido2.assertionOptions";

        #endregion

        #region Constructor

        public AssertionUseCase(ICustomerFactory customerFactory,
            ICredentialFactory credentialFactory,
            IFido2 fido2,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache distributedCache)
        {
            this.customerFactory = customerFactory;
            this.credentialFactory = credentialFactory;
            this.fido2 = fido2;
            this.httpContextAccessor = httpContextAccessor;
            this.distributedCache = distributedCache;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Makes assertion options based on the provided request model.
        /// </summary>
        /// <param name="assertionOptionsRequestModel">Assertion options request modlel</param>
        /// <returns>Assertion options</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IResult> MakeAssertionOptions(AssertionOptionsRequestModel assertionOptionsRequestModel)
        {
            var existingKeys = new List<PublicKeyCredentialDescriptor>();

            if (string.IsNullOrEmpty(assertionOptionsRequestModel.Username))
            {
                assertionOptionsKey = assertionOptionsKey + $".{Guid.NewGuid()}";
            }

            if (!string.IsNullOrEmpty(assertionOptionsRequestModel.Username))
            {
                var user = customerFactory.GetCustomerWithCredentials(assertionOptionsRequestModel.Username);

                existingKeys = credentialFactory.BuildPublicKeyCredentialDescriptors(user.Credentials);

                assertionOptionsKey = assertionOptionsKey + "." + user.Customer.Name;
            }

            var authenticationExtensionsClientInputs = new AuthenticationExtensionsClientInputs()
            {
                Extensions = true,
                UserVerificationMethod = true,
            };

            var assertionOptionsParams = new GetAssertionOptionsParams
            {
                AllowedCredentials = existingKeys,
                Extensions = authenticationExtensionsClientInputs,
                UserVerification = UserVerificationRequirement.Preferred
            };

            var options = fido2.GetAssertionOptions(assertionOptionsParams);

            await distributedCache.SetStringAsync(assertionOptionsKey, JsonSerializer.Serialize(options), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            httpContextAccessor.HttpContext.Response.Headers.TryAdd("X-Assertion-Options-Key", assertionOptionsKey);

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
            var assertionOptionsKey = httpContextAccessor.HttpContext.Request.Headers["X-Assertion-Options-Key"].ToString();
            var sessionJson = await distributedCache.GetStringAsync(assertionOptionsKey, cancellationToken);

            if (string.IsNullOrEmpty(sessionJson))
            {
                return Results.BadRequest(new { message = "Assertion options expired or not found in session." });
            }

            var options = AssertionOptions.FromJson(sessionJson);

            var authenticatorResponse = JsonSerializer.Deserialize<AuthenticatorResponse>(authenticatorAssertionRawResponse.Response.ClientDataJson);

            var credential = credentialFactory.GetCredentialByCredentialId(authenticatorAssertionRawResponse.RawId);
            if (credential.pubKey == null || credential.descriptor == null)
            {
                return Results.NotFound(new { message = "Credential not found in relying party." });
            }

            var makeAssertionParams = new MakeAssertionParams
            {
                AssertionResponse = authenticatorAssertionRawResponse,
                OriginalOptions = options,
                StoredPublicKey = credential.pubKey,
                StoredSignatureCounter = credential.signatureCount,
                IsUserHandleOwnerOfCredentialIdCallback = UserHandleOwnerOfCredentialIdAsync
            };

            var response = await fido2.MakeAssertionAsync(makeAssertionParams, cancellationToken);

            //TODO: Handle JWT

            return Results.Ok(response);
        }

        private async Task<bool> UserHandleOwnerOfCredentialIdAsync(IsUserHandleOwnerOfCredentialIdParams args, CancellationToken cancellationToken)
        {
            return await credentialFactory.IsUserHandleOwnerOfCredentialId(args.CredentialId, args.UserHandle);
        }

        #endregion
    }
}
