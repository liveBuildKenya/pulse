using Fido2NetLib;
using Fluxor;
using Pulse.Web.Common;
using Pulse.Web.Store.States;
using Pulse.Web.WebAuthentication.Models;
using Pulse.Web.WebAuthentication.Services;
using System.Text.Json;

namespace Pulse.Web.Store
{
    public class Effects
    {
        private readonly WebAuthnApiService _webAuthnApi;
        private readonly UserService _userService;
        private readonly TemporaryStorage _temporaryStorage;
        private readonly IState<WebAuthnState> _webAuthnState;

        public Effects(WebAuthnApiService webAuthnApi,
                       UserService userService,
                       TemporaryStorage temporaryStorage,
                       IState<WebAuthnState> webAuthnState)
        {
            _webAuthnApi = webAuthnApi;
            _userService = userService;
            _temporaryStorage = temporaryStorage;
            _webAuthnState = webAuthnState;
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

                Key = "Assertion",
                Value = $"{assertionOptions}"
            };

            var assertionOptionsResultAction = new FetchWebAuthnAssertionOptionsResultAction(assertionRequestOptions);
            dispatcher.Dispatch(assertionOptionsResultAction);
        }

        [EffectMethod]
        public async Task HandleGetWebAuthnAssertion(FetchWebAuthnAssertionOptionsResultAction action, IDispatcher dispatcher)
        {
            var assertionOptions = _webAuthnState.Value.WebAuthnStage
                .Where(webAuthnStage => webAuthnStage.Key == "Assertion")
                .Select(webAuthnStage => webAuthnStage.Value)
                .FirstOrDefault();
            var assertionOptionsModel = JsonSerializer.Deserialize<AssertionOptions>(assertionOptions);
            var assertionAction = await _userService.MakeAssertion(assertionOptionsModel);

            var webAuthnStageModel = new WebAuthnStageModel
            {
                Key = "AssertionResult",
                Value = $"{assertionAction}"
            };

            var assertionResultAction = new FetchWebAuthnAssertionResultAction(webAuthnStageModel);
            dispatcher.Dispatch(assertionResultAction);
        }
    }
}
