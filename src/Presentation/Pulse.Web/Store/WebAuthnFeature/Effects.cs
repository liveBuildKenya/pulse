using Fluxor;
using Pulse.Web.WebAuthentication.Services;

namespace Pulse.Web.Store.WebAuthnFeature
{
    public class Effects
    {
        private readonly WebAuthnApiService _webAuthnApi;

        public Effects(WebAuthnApiService webAuthnApi,
                       UserService userService)
        {
            _webAuthnApi = webAuthnApi;
        }

        [EffectMethod(typeof(FetchClientCapabilitiesAction))]
        public async Task HandleGetWebAuthenticationClientCapabilities(IDispatcher dispatcher)
        {
            await _webAuthnApi.Initialize();

            var clientCapabilities = await _webAuthnApi.GetClientCapabilitiesAsync();

            var action = new FetchClientCapabilitiesResultAction(clientCapabilities);
            dispatcher.Dispatch(action);
        }
    }
}
