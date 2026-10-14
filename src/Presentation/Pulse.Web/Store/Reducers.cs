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
            var stages = webAuthenticationState.WebAuthnStage.ToList();

            var index = stages.FindIndex(
                x => x.Key == fetchWebAuthnAssertionOptionsResultAction.WebAuthnStageModel.Key);

            if (index >= 0)
                stages[index] = fetchWebAuthnAssertionOptionsResultAction.WebAuthnStageModel;
            else
                stages.Add(fetchWebAuthnAssertionOptionsResultAction.WebAuthnStageModel);

            return webAuthenticationState with
            {
                IsLoading = false,
                WebAuthnStage = [.. stages]
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
            var stages = webAuthenticationState.WebAuthnStage.ToList();

            var index = stages.FindIndex(
                x => x.Key == fetchWebAuthnAssertionOptionsResultAction.WebAuthnStageModel.Key);

            if (index >= 0)
                stages[index] = fetchWebAuthnAssertionOptionsResultAction.WebAuthnStageModel;
            else
                stages.Add(fetchWebAuthnAssertionOptionsResultAction.WebAuthnStageModel);

            return webAuthenticationState with
            {
                IsLoading = false,
                WebAuthnStage = [.. stages]
            };
        }

        #endregion

        #region WebAuthn Attestation Options

        [ReducerMethod(typeof(FetchWebAuthnAttestationOptionsAction))]
        public static WebAuthnState ReduceFecthWebAuthnAttestationOptionsAction(WebAuthnState webAuthenticationState)
        {
            return webAuthenticationState with
            {
                IsLoading = true,
                WebAuthnStage = webAuthenticationState.WebAuthnStage
            };
        }

        [ReducerMethod]
        public static WebAuthnState ReduceFecthWebAuthnAttestationOptionsActionResultAction(WebAuthnState webAuthenticationState, FetchWebAuthnAttestationOptionsResultAction fetchWebAuthnAttestationOptionsResultAction)
        {
            var stages = webAuthenticationState.WebAuthnStage.ToList();

            var index = stages.FindIndex(
                x => x.Key == fetchWebAuthnAttestationOptionsResultAction.WebAuthnStageModel.Key);

            if (index >= 0)
                stages[index] = fetchWebAuthnAttestationOptionsResultAction.WebAuthnStageModel;
            else
                stages.Add(fetchWebAuthnAttestationOptionsResultAction.WebAuthnStageModel);

            return webAuthenticationState with
            {
                IsLoading = false,
                WebAuthnStage = [.. stages]
            };
        }

        #endregion

        #region WebAuthn Attestation

        [ReducerMethod(typeof(FetchWebAuthnAttestationAction))]
        public static WebAuthnState ReduceFetchWebAuthnAttestationAction(WebAuthnState webAuthenticationState)
        {
            return webAuthenticationState with
            {
                IsLoading = true,
                WebAuthnStage = webAuthenticationState.WebAuthnStage
            };
        }

        [ReducerMethod]
        public static WebAuthnState ReduceFetchWebAuthnAttestationResultAction(WebAuthnState webAuthenticationState, FetchWebAuthnAttestationResultAction fetchWebAuthnAttestationResultAction)
        {
            return webAuthenticationState with
            {
                IsLoading = false,
                WebAuthnStage = [.. webAuthenticationState.WebAuthnStage, fetchWebAuthnAttestationResultAction.WebAuthnStageModel],
            };
        }

        #endregion
    }
}
