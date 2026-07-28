using Fido2NetLib.Objects;

namespace Pulse.WebAuthn.Application.Attestation.Models
{
    /// <summary>
    /// Represents the attestation options request model
    /// </summary>
    public class AttestationOptionsRequestModel
    {
        /// <summary>
        /// Gets or sets the attestation conveyance preference
        /// </summary>
        public AttestationConveyancePreference? AttestationConveyancePreference { get; set; }

        public AuthenticatorAttachment? AuthenticatorAttachment { get; set; }
        public UserVerificationRequirement? UserVerificationRequirement { get; set; }
        public ResidentKeyRequirement? ResidentKeyRequirement { get; set; }
    }
}
