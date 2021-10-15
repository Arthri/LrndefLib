using Newtonsoft.Json;

namespace LrndefLib
{
    /// <summary>
    /// Represents settings' metadata, such as what version it is.
    /// </summary>
    public sealed class SettingsMetadata
    {
        /// <summary>
        /// Represents the metadata version it was written in.
        /// </summary>
        [JsonProperty("metadataVersion")]
        public SimpleVersion MetadataVersion { get; internal set; }

        /// <summary>
        /// Represents the settings version it was written in.
        /// </summary>
        [JsonProperty("version")]
        public SimpleVersion SettingsVersion { get; internal set; }

        public SettingsMetadata(
            SimpleVersion metadataVersion,
            SimpleVersion settingsVersion)
        {
            MetadataVersion = metadataVersion;
            SettingsVersion = settingsVersion;
        }
    }
}
