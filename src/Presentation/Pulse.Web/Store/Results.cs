using Pulse.Web.WebAuthentication.Models;

namespace Pulse.Web.Store
{
    public record FetchClientCapabilitiesResultAction(ClientCapabilitiesModel ClientCapabilitiesModel);
    public record FetchWebAuthnAssertionOptionsResultAction(WebAuthnStageModel WebAuthnStageModel);
    public record FetchWebAuthnAssertionResultAction(WebAuthnStageModel WebAuthnStageModel);
    public record FetchWebAuthnAttestationOptionsResultAction(WebAuthnStageModel WebAuthnStageModel);
    public record FetchWebAuthnAttestationResultAction(WebAuthnStageModel WebAuthnStageModel);
}
