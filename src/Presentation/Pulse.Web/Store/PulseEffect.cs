using Fluxor;
using Pulse.Web.WebAuthentication.Models;
using Pulse.Web.WebAuthentication.Services;

namespace Pulse.Web.Store
{
    /// <summary>
    /// Represents an effect class
    /// </summary>
    public class PulseEffect
    {
        private readonly WebAuthnApiService webAuthnApiService;

        public PulseEffect(WebAuthnApiService webAuthnApiService)
        {
            this.webAuthnApiService = webAuthnApiService;
        }

        [EffectMethod]
        public async Task HandlePulseActionAsync(PulseAction action, IDispatcher dispatcher)
        {
            await webAuthnApiService.Initialize();

            if (string.Equals(action.Name, "WebAuthn.FetchClientCapabilities") &&
                action.ActionType == ApplicationActionType.Command)
            {
                ClientCapabilitiesModel clientCapabilities = await webAuthnApiService.GetClientCapabilitiesAsync();

                dispatcher.Dispatch(new PulseAction
                {
                    ActionType = ApplicationActionType.CommandEvent,
                    Name = action.Name,
                    Payload = clientCapabilities
                });
            }
        }
    }
}
