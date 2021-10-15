using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using TShockAPI.Configuration;

namespace LrndefLib
{
    public class VersionedConfigFile<TSettings> : IConfigFile<TSettings>
        where TSettings : VersionedSettings
    {
        /// <summary>
        /// Represents the current metadata version.
        /// </summary>
        public static readonly SimpleVersion CurrentMetadataVersion = new SimpleVersion(0, 0, 1);

        public SimpleVersion CurrentVersion { get; }

        public TSettings Settings { get; set; }

        /// <summary>
        /// Converts the specified <see cref="JObject"/> instance to <see cref="TSettings"/>.
        /// </summary>
        /// <param name="metadata">The settings' metadata.</param>
        /// <param name="jObject">The parsed <see cref="JObject"/>.</param>
        /// <param name="incompleteSettings">Whether or not the version has changed.</param>
        /// <returns></returns>
        public delegate TSettings BindJObject(SettingsMetadata metadata, JObject jObject, ref bool incompleteSettings);

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

        public VersionedConfigFile(Version currentVersion)
        {
            CurrentVersion = currentVersion;
            _bindDelegate = (SettingsMetadata metadata, JObject jObject, ref bool incompleteSettings) =>
            {
                var jsonSerializer = CreateSerializer();
                return jObject.ToObject<TSettings>(jsonSerializer);
            };
        }

        TSettings IConfigFile<TSettings>.ConvertJson(string json, out bool incompleteSettings)
        {
            return FromJSON(json, out incompleteSettings);
        }

        public TSettings FromJSON(string json, out bool incompleteSettings)
        {
            var jObject = JObject.Parse(json);

            var jsonSerializer = CreateSerializer();

            var metadataToken = jObject[VersionedSettings.PROPNAME_Metadata];
            var metadata = metadataToken.ToObject<SettingsMetadata>(jsonSerializer);
            jObject.Remove(VersionedSettings.PROPNAME_Metadata);

            if (metadata.MetadataVersion != CurrentMetadataVersion)
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

        public TSettings Read(string path, out bool incompleteSettings)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Read(stream, out incompleteSettings);
            }
        }

        public TSettings Read(Stream stream, out bool incompleteSettings)
        {
            using (var reader = new StreamReader(stream))
            {
                return FromJSON(reader.ReadToEnd(), out incompleteSettings);
            }
        }

        public void Write(string path)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream);
            }
        }

        public void Write(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                var json = JsonConvert.SerializeObject(Settings);
                writer.Write(json);
            }
        }

        /// <summary>
        /// Creates a <see cref="JsonSerializer"/> for parsing settings.
        /// </summary>
        /// <returns>A <see cref="JsonSerializer"/> instance for parsing settings.</returns>
        private JsonSerializer CreateSerializer()
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new SimpleVersionJsonConverter());

            return JsonSerializer.CreateDefault(jsonSettings);
        }
    }
}
