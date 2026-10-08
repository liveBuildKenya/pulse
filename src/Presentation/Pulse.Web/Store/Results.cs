using Pulse.Web.WebAuthentication.Models;

namespace Pulse.Web.Store
{
    public record FetchClientCapabilitiesResultAction(ClientCapabilitiesModel ClientCapabilitiesModel);
    public record FetchWebAuthnAssertionOptionsResultAction(WebAuthnStageModel WebAuthnStageModel);
    public record FetchWebAuthnAssertionResultAction(WebAuthnStageModel WebAuthnStageModel);
}
