namespace Pulse.Web.WebAuthentication.Models
{
    /// <summary>
    /// Represents the web authentication stages
    /// </summary>
    public static class WebAuthnStage
    {
        /// <summary>
        /// Represents the assertion options stage
        /// </summary>
        public static string AssertionOptions => "AssertionOptions";

        /// <summary>
        /// Represents the successful assertion stage
        /// </summary>
        public static string SuccessfulAssertion => "SuccessfulAssertion";
        /// <summary>
        /// Represents the failed assertion stage
        /// </summary>
        public static string FailedAssertion => "FailedAssertion";

        /// <summary>
        /// Represents the attestation options stage
        /// </summary>
        public static string AttestationOptions => "AttestationOptions";

        /// <summary>
        /// Represents the attestation stage
        /// </summary>
        public static string Attestation => "Attestation";

    }
}
