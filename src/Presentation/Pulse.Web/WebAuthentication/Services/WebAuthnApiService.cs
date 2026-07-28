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
    }

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAuthnApi(this IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<WebAuthnApiService>();
    }
}
