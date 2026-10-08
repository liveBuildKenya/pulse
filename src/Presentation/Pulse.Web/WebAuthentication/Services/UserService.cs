using Fido2NetLib;
using Fido2NetLib.Objects;
using Pulse.Web.Common;
using System.Net.Http.Json;
using System.Text;

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

        public async Task<string> RegisterAsync(string? username, string? displayName = null,
            AttestationConveyancePreference? attestationType = null, AuthenticatorAttachment? authenticator = null,
            UserVerificationRequirement? userVerification = null, ResidentKeyRequirement? residentKey = null)
        {
            // Make sure the WebAuthn API is initialized (although that should happen almost immediately after startup)
            await _webAuthnApiService.Initialize();

            // Build the route to get options
            var routeOpts = (string.IsNullOrEmpty(username) ? string.Empty : $"/{username}");

            // Add optional parameters if set
            var optionalParams = new List<string>();
            if (!string.IsNullOrEmpty(displayName))
            {
                optionalParams.Add($"{nameof(displayName)}={displayName}");
            }

            if (attestationType.HasValue)
            {
                optionalParams.Add($"{nameof(attestationType)}={attestationType}");
            }

            if (authenticator.HasValue)
            {
                optionalParams.Add($"{nameof(authenticator)}={authenticator}");
            }

            if (userVerification.HasValue)
            {
                optionalParams.Add($"{nameof(userVerification)}={userVerification}");
            }

            if (residentKey.HasValue)
            {
                optionalParams.Add($"{nameof(residentKey)}={residentKey}");
            }

            var query = "";
            if (optionalParams.Any())
            {
                query = "?" + string.Join("&", optionalParams);
            }

            // Now the magic happens so stuff can go wrong
            CredentialCreateOptions? options;
            try
            {
                // Get options from server
                options = await _httpClient.GetFromJsonAsync<CredentialCreateOptions>(routeOpts + query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.Message;
            }

            if (options == null)
            {
                return "No options received";
            }

            // Build the route to register the credentials
            var routeCreds = $"/{username ?? Convert.ToBase64String(Encoding.UTF8.GetBytes(options.User.Name))}";

            try
            {
                // Present options to user and get response
                var credential = await _webAuthnApiService.CreateCredsAsync(options);

                // Send response to server
                return await (await _httpClient.PutAsJsonAsync(routeCreds, credential)).Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var errorMessage = e.Message;
                if (options.ExcludeCredentials?.Count > 0)
                {
                    errorMessage += " (You may have already registered this device)";
                }
                return errorMessage;
            }
        }

        public async Task<string> GetAssertionOptions(string username, string userVerification = null)
        {
            // Build the route to get options
            // Build route
            var assertionOptionsRoute = _assertionOptionsRoute +
                (string.IsNullOrEmpty(username) ? string.Empty : $"?{username}") +
                (string.IsNullOrEmpty(userVerification) ? string.Empty : $"&{userVerification}");

            // Get options from server
            var options = await _httpClient.GetFromJsonAsync<AssertionOptions>(assertionOptionsRoute);
            if (options is null)
            {
                return "No options received";
            }


            // Return JSON for debugging
            return options.ToJson();

        }

        public async Task<string> MakeAssertion(AssertionOptions assertionOptions)
        {
            // Make sure the WebAuthn API is initialized (although that should happen almost immediately after startup)
            await _webAuthnApiService.Initialize();

            // Now the magic happens so stuff can go wrong
            try
            {
                if (assertionOptions is null)
                {
                    return "No options received";
                }

                // Present options to user and get response (usernameless users will be asked by their authenticator, which credential they want to use to sign the challenge)
                var assertion = await _webAuthnApiService.VerifyAsync(assertionOptions);


                // Send response to server
                return await (await _httpClient.PostAsJsonAsync($"{_assertionRoute}", assertion)).Content.ReadAsStringAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
