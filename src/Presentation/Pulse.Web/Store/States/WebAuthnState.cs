using Fluxor;
using Pulse.Web.WebAuthentication.Models;

namespace Pulse.Web.Store.States
{
    /// <summary>
    /// Represents the web authentication state
    /// </summary>
    [FeatureState]
    public record WebAuthnState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the webauthn state is loading
        /// </summary>
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets the client webauthn capabilities
        /// </summary>
        public ClientCapabilitiesModel ClientCapabilitiesModel { get; init; }

        /// <summary>
        /// Gets or sets the webauthn stage
        /// </summary>
        public List<WebAuthnStageModel> WebAuthnStage { get; init; } = new();
    }
}
