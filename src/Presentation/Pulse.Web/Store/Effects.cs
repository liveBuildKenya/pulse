using Fido2NetLib;
using Fluxor;
using Microsoft.FluentUI.AspNetCore.Components;
using Pulse.Web.Common;
using Pulse.Web.Pages;
using Pulse.Web.Store.States;
using Pulse.Web.WebAuthentication.Models;
using Pulse.Web.WebAuthentication.Services;

namespace Pulse.Web.Store
{
    public class Effects
    {
        private readonly WebAuthnApiService _webAuthnApi;
        private readonly UserService _userService;
        private readonly IState<WebAuthnState> _webAuthnState;
        private readonly IDialogService _dialogService;

        public Effects(WebAuthnApiService webAuthnApi,
                       UserService userService,
                       IState<WebAuthnState> webAuthnState,
                       IDialogService dialogService)
        {
            _webAuthnApi = webAuthnApi;
            _userService = userService;
            _webAuthnState = webAuthnState;
            _dialogService = dialogService;
        }

        [EffectMethod(typeof(FetchClientCapabilitiesAction))]
        public async Task HandleGetWebAuthenticationClientCapabilities(IDispatcher dispatcher)
        {
            await _webAuthnApi.Initialize();

            var clientCapabilities = await _webAuthnApi.GetClientCapabilitiesAsync();

            var action = new FetchClientCapabilitiesResultAction(clientCapabilities);
            dispatcher.Dispatch(action);
        }

        [EffectMethod]
        public async Task HandleGetWebAuthnAssertionOptions(FetchWebAuthnAssertionOptionsAction action, IDispatcher dispatcher)
        {
            var assertionOptions = await _userService.GetAssertionOptions(action.Username);

            var assertionRequestOptions = new WebAuthnStageModel
            {

                Key = WebAuthnStage.AssertionOptions,
                Value = assertionOptions
            };

            var assertionOptionsResultAction = new FetchWebAuthnAssertionOptionsResultAction(assertionRequestOptions);
            dispatcher.Dispatch(assertionOptionsResultAction);
        }

        [EffectMethod]
        public async Task HandleGetWebAuthnAssertion(FetchWebAuthnAssertionOptionsResultAction action, IDispatcher dispatcher)
        {
            var webAuthnStage = _webAuthnState.Value.WebAuthnStage
                .Where(webAuthnStage => webAuthnStage.Key == WebAuthnStage.AssertionOptions)
                .Select(webAuthnStage => webAuthnStage.Value)
                .FirstOrDefault();

            var assertionOptions = webAuthnStage as AssertionOptions;

            var assertionResult = await _userService.MakeAssertion(assertionOptions);

            if (assertionResult != null && assertionResult.CredentialId != null)
            {
                var webAuthnStageModel = new WebAuthnStageModel
                {
                    Key = WebAuthnStage.SuccessfulAssertion,
                    Value = "User Assertion Successfull"
                };
                var assertionResultAction = new FetchWebAuthnAssertionResultAction(webAuthnStageModel);
                dispatcher.Dispatch(assertionResultAction);
            }
            else
            {
                var webAuthnStageModel = new WebAuthnStageModel
                {
                    Key = WebAuthnStage.FailedAssertion,
                    Value = "User Assertion Failed"
                };
                var assertionResultAction = new FetchWebAuthnAssertionResultAction(webAuthnStageModel);
                dispatcher.Dispatch(assertionResultAction);
            }
        }

        [EffectMethod(typeof(FetchWebAuthnAssertionResultAction))]
        public async Task HandleWebAuthnAssertionsResult(IDispatcher dispatcher)
        {
            var webAuthnStage = _webAuthnState.Value.WebAuthnStage
                .Where(webAuthnStage => webAuthnStage.Key == WebAuthnStage.SuccessfulAssertion)
                .Select(webAuthnStage => webAuthnStage.Value)
                .FirstOrDefault();

            if (webAuthnStage == null)
            {
                var result = await _dialogService.ShowDialogAsync<FormDialog>(options =>
                {
                    options.Modal = true;
                });
            }
        }

        [EffectMethod]
        public async Task HandleWebAuthnAttestationOptions(FetchWebAuthnAttestationOptionsAction action, IDispatcher dispatcher)
        {
            var attestationOptions = await _userService.GetAttestationOptions(action.Username);

            var webAuthnStageModel = new WebAuthnStageModel
            {
                Key = WebAuthnStage.AttestationOptions,
                Value = attestationOptions
            };

            var attestationOptionsResult = new FetchWebAuthnAttestationOptionsResultAction(webAuthnStageModel);
            dispatcher.Dispatch(attestationOptionsResult);
        }

        [EffectMethod(typeof(FetchWebAuthnAttestationOptionsResultAction))]
        public async Task HandleWebAuthnAttestationOptionsResult(IDispatcher dispatcher)
        {
            var attestationOptions = _webAuthnState.Value.WebAuthnStage
                .Where(webAuthnStage => webAuthnStage.Key == WebAuthnStage.AttestationOptions)
                .Select(webAuthnStage => webAuthnStage.Value)
                .FirstOrDefault();

            var credentialCreateOptions = attestationOptions as CredentialCreateOptions;

            var attestationResult = await _userService.MakeAttestation(credentialCreateOptions);

            var webAuthnStageModel = new WebAuthnStageModel
            {
                Key = WebAuthnStage.Attestation,
                Value = attestationResult
            };

            var attestationResultAction = new FetchWebAuthnAttestationResultAction(webAuthnStageModel);
            dispatcher.Dispatch(attestationResultAction);
        }

        [EffectMethod(typeof(FetchWebAuthnAttestationResultAction))]
        public async Task HandleWebAuthnAttestationResult(IDispatcher dispatcher)
        {
            var attestation = _webAuthnState.Value.WebAuthnStage
                .Where(webAuthnStage => webAuthnStage.Key == WebAuthnStage.Attestation)
                .Select(webAuthnStage => webAuthnStage.Value)
                .FirstOrDefault();

            var attestationResult = attestation as AttestationResponse;


        }
    }
}
