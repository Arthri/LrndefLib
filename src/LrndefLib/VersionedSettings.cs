using Newtonsoft.Json;

namespace LrndefLib
{
    public abstract class VersionedSettings
    {
        [JsonProperty(
            PropertyName = "version",
            Order = int.MinValue,
            Required = Required.Always)]
        public SimpleVersion SettingsVersion { get; }
    }
}
