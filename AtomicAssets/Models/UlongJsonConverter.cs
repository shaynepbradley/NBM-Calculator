using System.Text.Json;
using System.Text.Json.Serialization;

namespace UpgradeCalculator.Classes
{
    public class UlongJsonConverter : JsonConverter<ulong>
    {
        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (s == null)
                return 0;

            return ulong.TryParse(s, out var b) ? b : 0;
        }

        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
    }
}
