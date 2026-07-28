using Fluxor;
using Pulse.Web.WebAuthentication.Models;

namespace Pulse.Web.Store.WebAuthnFeature
{
    /// <summary>
    /// Represents the web authentication state
    /// </summary>
    [FeatureState]
    public record WebAuthnState
    {
        public bool IsLoading { get; set; }
        public ClientCapabilitiesModel ClientCapabilitiesModel { get; init; }
    }
}
