using Fluxor;
using Pulse.Web.Store.States;

namespace Pulse.Web.Store
{
    public static class Reducers
    {
        #region Client Capabilities

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

        #endregion

        #region WebAuthn Assertion Options

        [ReducerMethod(typeof(FetchWebAuthnAssertionOptionsAction))]
        public static WebAuthnState ReduceFetchWebAuthnAssertionOptionsAction(WebAuthnState webAuthenticationState)
        {
            return webAuthenticationState with
            {
                IsLoading = true,
                WebAuthnStage = webAuthenticationState.WebAuthnStage
            };
        }

        [ReducerMethod]
        public static WebAuthnState ReduceFetchWebAuthnAssertionOptionsResultAction(WebAuthnState webAuthenticationState, FetchWebAuthnAssertionOptionsResultAction fetchWebAuthnAssertionOptionsResultAction)
        {
            return webAuthenticationState with
            {
                IsLoading = false,
                WebAuthnStage = [.. webAuthenticationState.WebAuthnStage, fetchWebAuthnAssertionOptionsResultAction.WebAuthnStageModel],
            };
        }

        #endregion

        #region WebAuthn Assertion

        [ReducerMethod(typeof(FetchWebAuthnAssertionAction))]
        public static WebAuthnState ReduceFetchWebAuthnAssertionAction(WebAuthnState webAuthenticationState)
        {
            return webAuthenticationState with
            {
                IsLoading = true,
                WebAuthnStage = webAuthenticationState.WebAuthnStage
            };
        }

        [ReducerMethod]
        public static WebAuthnState ReduceFetchWebAuthnAssertionResultAction(WebAuthnState webAuthenticationState, FetchWebAuthnAssertionResultAction fetchWebAuthnAssertionOptionsResultAction)
        {
            return webAuthenticationState with
            {
                IsLoading = false,
                WebAuthnStage = [.. webAuthenticationState.WebAuthnStage, fetchWebAuthnAssertionOptionsResultAction.WebAuthnStageModel],
            };
        }

        #endregion
    }
}
