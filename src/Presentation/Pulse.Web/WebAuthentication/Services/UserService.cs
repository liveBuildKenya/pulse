using Fido2NetLib;
using Fido2NetLib.Objects;
using Pulse.Web.Common;
using System.Net.Http.Json;

namespace Pulse.Web.WebAuthentication.Services
{
    /// <summary>
    /// Represents a user service
    /// </summary>
    public class UserService
    {
        private const string _assertionOptionsRoute = "/assertion/options";
        private const string _assertionRoute = "/assertion";
        private const string _attestationOptionRoute = "/attestation/options";
        private const string _attestationRoute = "/attestation";

        private readonly HttpClient _httpClient;
        private readonly WebAuthnApiService _webAuthnApiService;
        private readonly TemporaryStorage _temporaryStorage;

        public UserService(IHttpClientFactory httpClientFactory, WebAuthnApiService webAuthnApiService, TemporaryStorage temporaryStorage)
        {
            _httpClient = httpClientFactory.CreateClient("WebAuthnApi");
            _webAuthnApiService = webAuthnApiService;
            _temporaryStorage = temporaryStorage;
        }

        public async Task<CredentialCreateOptions> GetAttestationOptions(string username)
        {
            await _webAuthnApiService.Initialize();

            var routeOpts = (string.IsNullOrEmpty(username) ? string.Empty : $"?username={username}");

            return await _httpClient.GetFromJsonAsync<CredentialCreateOptions>(_attestationOptionRoute + routeOpts);
        }

        public async Task<AttestationResponse> MakeAttestation(CredentialCreateOptions options)
        {
            await _webAuthnApiService.Initialize();

            var routeCreds = _attestationRoute;

            // Present options to user and get response
            var credential = await _webAuthnApiService.CreateCredsAsync(options);

            // Send response to server
            return await (await _httpClient.PostAsJsonAsync(routeCreds, credential)).Content.ReadFromJsonAsync<AttestationResponse>();
        }

        public async Task<AssertionOptions> GetAssertionOptions(string username, string userVerification = null)
        {
            // Build the route to get options
            var assertionOptionsRoute = _assertionOptionsRoute +
                (string.IsNullOrEmpty(username) ? string.Empty : $"?{username}") +
                (string.IsNullOrEmpty(userVerification) ? string.Empty : $"&{userVerification}");

            // Get options from server
            var options = await _httpClient.GetFromJsonAsync<AssertionOptions>(assertionOptionsRoute);

            return options;

        }

        public async Task<VerifyAssertionResult> MakeAssertion(AssertionOptions assertionOptions)
        {
            // Make sure the WebAuthn API is initialized (although that should happen almost immediately after startup)
            await _webAuthnApiService.Initialize();

            // Now the magic happens so stuff can go wrong
            try
            {
                // Present options to user and get response (usernameless users will be asked by their authenticator, which credential they want to use to sign the challenge)
                var assertion = await _webAuthnApiService.VerifyAsync(assertionOptions);
                // Send response to server
                return await (await _httpClient.PostAsJsonAsync($"{_assertionRoute}", assertion)).Content.ReadFromJsonAsync<VerifyAssertionResult>();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
