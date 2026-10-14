namespace Pulse.Web.Common
{
    /// <summary>
    /// Represents an attestation response
    /// </summary>
    public class AttestationResponse
    {
        /// <summary>
        /// Gets or sets the attestation GUID
        /// </summary>
        public Guid AttestationGuid { get; set; }

        /// <summary>
        /// Gets or sets the sign counts
        /// </summary>
        public uint SignCount { get; set; }
    }
}
