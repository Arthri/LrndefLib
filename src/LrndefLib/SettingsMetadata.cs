using Newtonsoft.Json;

namespace LrndefLib
{
    /// <summary>
    /// Represents settings' metadata, such as what version it is.
    /// </summary>
    public sealed class SettingsMetadata
    {
        /// <summary>
        /// Represents the current metadata version.
        /// </summary>
        public static readonly SimpleVersion CurrentMetadataVersion = new SimpleVersion(1, 0, 0, 0);

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

        /// <summary>
        /// Initializes a new instance of <see cref="SettingsMetadata"/>.
        /// </summary>
        /// <param name="metadataVersion">The metadata version.</param>
        /// <param name="settingsVersion">The settings version.</param>
        public SettingsMetadata(
            SimpleVersion metadataVersion,
            SimpleVersion settingsVersion)
        {
            MetadataVersion = metadataVersion;
            SettingsVersion = settingsVersion;
        }
    }
}
