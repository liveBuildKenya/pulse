using Fido2NetLib;
using Microsoft.JSInterop;
using Pulse.Web.WebAuthentication.Models;

namespace Pulse.Web.WebAuthentication.Services
{
    public class WebAuthnApiService
    {
        private IJSObjectReference _jsObjectReference = null!;
        private readonly Task _initializer;

        public WebAuthnApiService(IJSRuntime jSRuntime)
        {
            _initializer = Task.Run(async () =>
                _jsObjectReference = await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/WebAuthentication.js"));
        }

        /// <summary>
        /// Wait this to ensure the object reference is initialized
        /// </summary>
        public Task Initialize() => _initializer;

        public async Task<ClientCapabilitiesModel> GetClientCapabilitiesAsync() => await _jsObjectReference.InvokeAsync<ClientCapabilitiesModel>("getClientCapabilities");

        /// <summary>
        /// Creates a new credential.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<AuthenticatorAttestationRawResponse> CreateCredsAsync(CredentialCreateOptions options) =>
            await _jsObjectReference.InvokeAsync<AuthenticatorAttestationRawResponse>("createCreds", options);

        /// <summary>
        /// Verifies a credential for login.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<AuthenticatorAssertionRawResponse> VerifyAsync(AssertionOptions options) =>
            await _jsObjectReference.InvokeAsync<AuthenticatorAssertionRawResponse>("verify", options);
    }
}
