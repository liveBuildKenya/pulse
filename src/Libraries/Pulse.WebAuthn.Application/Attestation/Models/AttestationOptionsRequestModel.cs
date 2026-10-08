using Fido2NetLib.Objects;

namespace Pulse.WebAuthn.Application.Attestation.Models
{
    public class AttestationOptionsRequestModel : IParsable<AttestationOptionsRequestModel>
    {
        public string Username { get; set; }
        public AttestationConveyancePreference? AttestationConveyancePreference { get; set; }
        public AuthenticatorAttachment? AuthenticatorAttachment { get; set; }
        public UserVerificationRequirement? UserVerificationRequirement { get; set; }
        public ResidentKeyRequirement? ResidentKeyRequirement { get; set; }

        public static AttestationOptionsRequestModel Parse(string s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out var result))
                return result;

            throw new FormatException($"Cannot parse '{s}' into AttestationOptionsRequestModel");
        }

        public static bool TryParse(string s, IFormatProvider? provider, out AttestationOptionsRequestModel result)
        {
            result = null!;
            if (string.IsNullOrWhiteSpace(s)) return false;

            var parts = s.Split(',');
            var dict = parts.Select(p => p.Split(':'))
                            .Where(p => p.Length == 2)
                            .ToDictionary(p => p[0].Trim(), p => p[1].Trim(), StringComparer.OrdinalIgnoreCase);

            result = new AttestationOptionsRequestModel
            {
                Username = dict.GetValueOrDefault("username"),
                AttestationConveyancePreference = TryParseEnum<AttestationConveyancePreference>(dict.GetValueOrDefault("AttestationConveyancePreference")),
                AuthenticatorAttachment = TryParseEnum<AuthenticatorAttachment>(dict.GetValueOrDefault("AuthenticatorAttachment")),
                UserVerificationRequirement = TryParseEnum<UserVerificationRequirement>(dict.GetValueOrDefault("UserVerificationRequirement")),
                ResidentKeyRequirement = TryParseEnum<ResidentKeyRequirement>(dict.GetValueOrDefault("ResidentKeyRequirement"))
            };

            return true;
        }

        private static TEnum? TryParseEnum<TEnum>(string? value) where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return Enum.TryParse<TEnum>(value, true, out var parsed) ? parsed : null;
        }
    }
}
