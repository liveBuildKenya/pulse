namespace Pulse.WebAuthn.Domain.Shared.Utilities
{
    public static class Helpers
    {
        public static string Base64UrlEncode(byte[] data)
        {
            return Convert.ToBase64String(data)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
