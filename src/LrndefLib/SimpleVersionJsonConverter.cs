using Newtonsoft.Json;
using System;

namespace LrndefLib
{
    /// <summary>
    /// Converts <see cref="SimpleVersion"/> to and from JSON.
    /// </summary>
    public class SimpleVersionJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEquivalentTo(typeof(SimpleVersion));
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;

            if (int.TryParse(value, out var packedVersion))
            {
                return new SimpleVersion(packedVersion);
            }
            else
            {
                if (objectType.IsGenericType
                    && objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return null;
                }
                else
                {
                    return new SimpleVersion(0);
                }
            }
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var version = (SimpleVersion)value;
            writer.WriteValue(version.PackedValue);
        }
    }
}
