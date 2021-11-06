using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using TShockAPI.Configuration;

namespace LrndefLib
{
    /// <summary>
    /// Represents a version-based config file.
    /// </summary>
    /// <typeparam name="TSettings">The type of settings.</typeparam>
    public class VersionedConfigFile<TSettings> : IConfigFile<TSettings>
    {
        /// <summary>
        /// Represents the current settings version.
        /// </summary>
        public SimpleVersion CurrentVersion { get; }

        /// <summary>
        /// Represents the settings metadata.
        /// </summary>
        public SettingsMetadata Metadata { get; set; }

        /// <summary>
        /// Represents the settings.
        /// </summary>
        public TSettings Settings { get; set; }

        /// <summary>
        /// Converts the specified <see cref="JObject"/> instance to <typeparamref name="TSettings"/>.
        /// </summary>
        /// <param name="metadata">The settings' metadata.</param>
        /// <param name="jObject">The parsed <see cref="JObject"/>.</param>
        /// <param name="incompleteSettings"><inheritdoc cref="FromJSON(string, out bool)" path="/param[@name='incompleteSettings']"/></param>
        /// <returns>The convereted settings.</returns>
        public delegate TSettings BindJObject(SettingsMetadata metadata, JObject jObject, ref bool incompleteSettings);

        /// <inheritdoc cref="BindDelegate"/>
        private BindJObject _bindDelegate;

        /// <summary>
        /// Represents the delegate that converts a settings' <see cref="JObject"/> to the settings' type.
        /// </summary>
        public BindJObject BindDelegate
        {
            get => _bindDelegate;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                else
                {
                    _bindDelegate = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="VersionedConfigFile{TSettings}"/>.
        /// </summary>
        /// <param name="currentVersion">The current settings version.</param>
        public VersionedConfigFile(Version currentVersion)
        {
            CurrentVersion = currentVersion;
            _bindDelegate = (SettingsMetadata metadata, JObject jObject, ref bool incompleteSettings) =>
            {
                var jsonSerializer = CreateSerializer();
                return jObject.ToObject<TSettings>(jsonSerializer);
            };
        }

        /// <inheritdoc />
        TSettings IConfigFile<TSettings>.ConvertJson(string json, out bool incompleteSettings)
        {
            return FromJSON(json, out incompleteSettings);
        }

        /// <summary>
        /// Parses the settings from the specified JSON.
        /// </summary>
        /// <param name="json">The JSON.</param>
        /// <param name="incompleteSettings">Whether or not the version has changed.</param>
        /// <returns>The parsed settings.</returns>
        public TSettings FromJSON(string json, out bool incompleteSettings)
        {
            var jObject = JObject.Parse(json);

            var jsonSerializer = CreateSerializer();

            var metadataToken = jObject["metadata"];
            var metadata = metadataToken.ToObject<SettingsMetadata>(jsonSerializer);
            Metadata = metadata;

            if (metadata.MetadataVersion != SettingsMetadata.CurrentMetadataVersion)
            {
                // Migrate metadata
            }

            if (metadata.SettingsVersion != CurrentVersion)
            {
                incompleteSettings = true;
            }
            else
            {
                incompleteSettings = false;
            }

            var deserialized = _bindDelegate(metadata, jObject, ref incompleteSettings);
            Settings = deserialized;

            return deserialized;
        }

        /// <summary>
        /// Reads the settings from the specified file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="incompleteSettings"><inheritdoc cref="FromJSON(string, out bool)" path="/param[@name='incompleteSettings']"/></param>
        /// <returns><inheritdoc cref="FromJSON(string, out bool)"/></returns>
        public TSettings Read(string path, out bool incompleteSettings)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Read(stream, out incompleteSettings);
            }
        }

        /// <summary>
        /// Reads the settings from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="incompleteSettings"><inheritdoc cref="FromJSON(string, out bool)" path="/param[@name='incompleteSettings']"/></param>
        /// <returns><inheritdoc cref="FromJSON(string, out bool)"/></returns>
        public TSettings Read(Stream stream, out bool incompleteSettings)
        {
            using (var reader = new StreamReader(stream))
            {
                return FromJSON(reader.ReadToEnd(), out incompleteSettings);
            }
        }

        /// <summary>
        /// Writes the settings to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Write(string path)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream);
            }
        }

        /// <summary>
        /// Writes the settings to the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Write(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                var jsonSerializer = CreateSerializer();
                var jObject = JObject.FromObject(Settings, jsonSerializer);

                // Append metadata
                var metadataJObject = JObject.FromObject(Metadata, jsonSerializer);
                var metadataProperty = new JProperty("metadata", Metadata);
                jObject.AddFirst(metadataProperty);

                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jObject.WriteTo(
                        jsonWriter,
                        SimpleVersionJsonConverter.Default);
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="JsonSerializer"/> for parsing settings.
        /// </summary>
        /// <returns>A <see cref="JsonSerializer"/> instance for parsing settings.</returns>
        private JsonSerializer CreateSerializer()
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(SimpleVersionJsonConverter.Default);
            jsonSettings.ContractResolver = IgnoreMetadataResolver.Default;

            return JsonSerializer.CreateDefault(jsonSettings);
        }
    }
}
