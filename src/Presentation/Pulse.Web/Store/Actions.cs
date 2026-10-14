using Fido2NetLib;

namespace Pulse.Web.Store
{
    public readonly struct FetchClientCapabilitiesAction;
    public record struct FetchWebAuthnAssertionOptionsAction(string Username = "", string UserVerification = "");
    public record struct FetchWebAuthnAssertionAction();
    public record struct FetchWebAuthnAttestationOptionsAction(string Username);
    public record struct FetchWebAuthnAttestationAction();
}
