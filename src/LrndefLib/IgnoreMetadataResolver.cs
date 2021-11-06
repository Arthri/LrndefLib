using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace LrndefLib
{
    /// <summary>
    /// A contract resolver that removes the "metadata" property.
    /// </summary>
    public class IgnoreMetadataResolver : DefaultContractResolver
    {
        /// <summary>
        /// Gets the default instance, usable anywhere.
        /// </summary>
        public static readonly IgnoreMetadataResolver Default = new IgnoreMetadataResolver();

        /// <inheritdoc />
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (property.PropertyName == "metadata")
            {
                property.ShouldSerialize = (obj) => false;
            }

            return property;
        }
    }
}
