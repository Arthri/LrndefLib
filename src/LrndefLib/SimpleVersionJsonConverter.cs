using Newtonsoft.Json;
using System;

namespace LrndefLib
{
    /// <summary>
    /// Converts <see cref="SimpleVersion"/> to and from JSON.
    /// </summary>
    public class SimpleVersionJsonConverter : JsonConverter
    {
        /// <summary>
        /// Gets the default instance of <see cref="SimpleVersionJsonConverter"/> usable anywhere.
        /// </summary>
        public static readonly SimpleVersionJsonConverter Default = new SimpleVersionJsonConverter();

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEquivalentTo(typeof(SimpleVersion));
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is long value))
            {
                throw new ArgumentException("current value of reader is not of type Int64", nameof(reader));
            }

            if (value < uint.MaxValue && value > uint.MinValue)
            {
                var packedVersion = (int)value;
                return new SimpleVersion(packedVersion);
            }
            else
            {
                throw new ArgumentException("current value of reader is not within bounds of UInt32", nameof(reader));
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
