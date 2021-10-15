using Newtonsoft.Json;

namespace LrndefLib
{
    /// <summary>
    /// Provides the basic structure of settings used by <see cref="VersionedConfigFile{TSettings}"/>.
    /// </summary>
    public abstract class VersionedSettings
    {
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
