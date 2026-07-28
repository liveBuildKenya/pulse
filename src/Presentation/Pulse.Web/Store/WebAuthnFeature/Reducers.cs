using Fluxor;

namespace Pulse.Web.Store.WebAuthnFeature
{
    public static class Reducers
    {
        [ReducerMethod(typeof(FetchClientCapabilitiesAction))]
        public static WebAuthnState ReduceFetchClientAuthenticationAction(WebAuthnState webAuthenticationState)
        {
            return webAuthenticationState with
            {
                IsLoading = true,
                ClientCapabilitiesModel = webAuthenticationState.ClientCapabilitiesModel
            };
        }

        [ReducerMethod]
        public static WebAuthnState ReduceFetchClientAuthenticationResultAction(WebAuthnState webAuthenticationState, FetchClientCapabilitiesResultAction fetchClientCapabilitiesResultAction)
        {
            return webAuthenticationState with
            {
                IsLoading = false,
                ClientCapabilitiesModel = fetchClientCapabilitiesResultAction.ClientCapabilitiesModel
            };
        }
    }
}
