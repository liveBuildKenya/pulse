using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pulse.WebApi.Utilities
{
    public class PaddedBase64ByteArrayConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var base64 = reader.GetString();
            return base64 is null ? null : Convert.FromBase64String(base64);
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            // Always write padded Base64
            var padded = Convert.ToBase64String(value); // preserves padding
            writer.WriteStringValue(padded);
        }
    }
}
