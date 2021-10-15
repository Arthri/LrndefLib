using Newtonsoft.Json;

namespace LrndefLib
{
    /// <summary>
    /// Provides the basic structure of settings used by <see cref="VersionedConfigFile{TSettings}"/>.
    /// </summary>
    public abstract class VersionedSettings
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
            public SimpleVersion MetadataVersion { get; }

            /// <summary>
            /// Represents the JSON property name of <see cref="SettingsVersion"/>.
            /// </summary>
            public const string PROPNAME_SettingsVersion = "version";

            /// <summary>
            /// Represents the settings version it was written in.
            /// </summary>
            [JsonProperty(PROPNAME_SettingsVersion)]
            public SimpleVersion SettingsVersion { get; }

            public SettingsMetadata(
                SimpleVersion metadataVersion,
                SimpleVersion settingsVersion)
            {
                MetadataVersion = metadataVersion;
                SettingsVersion = settingsVersion;
            }
        }

        /// <summary>
        /// Represents the JSON property name of <see cref="Metadata"/>.
        /// </summary>
        public const string PROPNAME_Metadata = "metadata";

        /// <summary>
        /// Represents this settings' <see cref="SettingsMetadata"/>.
        /// </summary>
        [JsonProperty(
            Order = int.MinValue,
            PropertyName = PROPNAME_Metadata,
            Required = Required.Always)]
        public SettingsMetadata Metadata { get; }

        /// <summary>
        /// Initializes new settings with the specified metadata.
        /// </summary>
        /// <param name="metadata"><inheritdoc cref="Metadata" /></param>
        public VersionedSettings(
            SettingsMetadata metadata)
        {
            Metadata = metadata;
        }
    }
}
