using System.Text.Json.Serialization;

namespace Pulse.Web.WebAuthentication.Models
{
    /// <summary>
    /// Represents the Web Authentication Browser API client capabilities
    /// </summary>
    public class ClientCapabilitiesModel
    {
        /// <summary>
        /// Indicates whether the client is capable of creating discoverable credentials.
        /// </summary>
        [JsonPropertyName("conditionalCreate")]
        public bool ConditionalCreate { get; set; }

        /// <summary>
        /// Indicates whether the client supports conditional mediation (auto-filled discoverable credentials).
        /// </summary>
        [JsonPropertyName("conditionalGet")]
        public bool ConditionalGet { get; set; }

        /// <summary>
        /// Indicates support for the legacy FIDO U2F AppID extension.
        /// </summary>
        [JsonPropertyName("extension:appid")]
        public bool ExtensionAppId { get; set; }

        /// <summary>
        /// Allows a relying party to exclude authenticators containing specified credentials previously registered using the legacy FIDO U2F API.
        /// </summary>
        [JsonPropertyName("extension:appidExclude")]
        public bool ExtensionAppIdExclude { get; set; }

        /// <summary>
        /// Indicates support for the CMTG key extension.
        /// </summary>
        [JsonPropertyName("extension:cmtgKey")]
        public bool ExtensionCmtgKey { get; set; }

        /// <summary>
        /// Indicates support for the credential blob extension.
        /// </summary>
        [JsonPropertyName("extension:credBlob")]
        public bool ExtensionCredBlob { get; set; }

        /// <summary>
        /// Indicates support for the credential properties extension.
        /// </summary>
        [JsonPropertyName("extension:credProps")]
        public bool ExtensionCredProps { get; set; }

        /// <summary>
        /// Indicates support for enforcing credential protection policies.
        /// </summary>
        [JsonPropertyName("extension:credentialProtectionPolicy")]
        public bool ExtensionCredentialProtectionPolicy { get; set; }

        /// <summary>
        /// Indicates support for cross-device fallback URLs.
        /// </summary>
        [JsonPropertyName("extension:crossDeviceFallbackUrl")]
        public bool ExtensionCrossDeviceFallbackUrl { get; set; }

        /// <summary>
        /// Indicates support for enforcing credential protection policies at the client level.
        /// </summary>
        [JsonPropertyName("extension:enforceCredentialProtectionPolicy")]
        public bool ExtensionEnforceCredentialProtectionPolicy { get; set; }

        /// <summary>
        /// Indicates support for retrieving credential blobs.
        /// </summary>
        [JsonPropertyName("extension:getCredBlob")]
        public bool ExtensionGetCredBlob { get; set; }

        /// <summary>
        /// Indicates support for creating HMAC secrets.
        /// </summary>
        [JsonPropertyName("extension:hmacCreateSecret")]
        public bool ExtensionHmacCreateSecret { get; set; }

        /// <summary>
        /// Indicates support for large blob storage.
        /// </summary>
        [JsonPropertyName("extension:largeBlob")]
        public bool ExtensionLargeBlob { get; set; }

        /// <summary>
        /// Indicates support for enforcing minimum PIN length.
        /// </summary>
        [JsonPropertyName("extension:minPinLength")]
        public bool ExtensionMinPinLength { get; set; }

        /// <summary>
        /// Indicates support for payment extensions.
        /// </summary>
        [JsonPropertyName("extension:payment")]
        public bool ExtensionPayment { get; set; }

        /// <summary>
        /// Indicates support for PRF (Pseudo-Random Function) extension.
        /// </summary>
        [JsonPropertyName("extension:prf")]
        public bool ExtensionPrf { get; set; }

        /// <summary>
        /// Indicates whether the client supports hybrid transport (Bluetooth, NFC, USB).
        /// </summary>
        [JsonPropertyName("hybridTransport")]
        public bool HybridTransport { get; set; }

        /// <summary>
        /// Indicates whether the client supports immediate credential retrieval.
        /// </summary>
        [JsonPropertyName("immediateGet")]
        public bool ImmediateGet { get; set; }

        /// <summary>
        /// Indicates support for passkey platform authenticators (PIN/biometric).
        /// </summary>
        [JsonPropertyName("passkeyPlatformAuthenticator")]
        public bool PasskeyPlatformAuthenticator { get; set; }

        /// <summary>
        /// Indicates support for related origin requests (passkeys usable across multiple sites with the same origin).
        /// </summary>
        [JsonPropertyName("relatedOrigins")]
        public bool RelatedOrigins { get; set; }

        /// <summary>
        /// Indicates support for signaling all accepted credentials.
        /// </summary>
        [JsonPropertyName("signalAllAcceptedCredentials")]
        public bool SignalAllAcceptedCredentials { get; set; }

        /// <summary>
        /// Indicates support for signaling current user details.
        /// </summary>
        [JsonPropertyName("signalCurrentUserDetails")]
        public bool SignalCurrentUserDetails { get; set; }

        /// <summary>
        /// Indicates support for signaling unknown credentials.
        /// </summary>
        [JsonPropertyName("signalUnknownCredential")]
        public bool SignalUnknownCredential { get; set; }

        /// <summary>
        /// Indicates whether the client has a user-verifying platform authenticator (PIN/biometric).
        /// </summary>
        [JsonPropertyName("userVerifyingPlatformAuthenticator")]
        public bool UserVerifyingPlatformAuthenticator { get; set; }
    }


}

