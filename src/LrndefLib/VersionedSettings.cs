using Newtonsoft.Json;
using System;

namespace LrndefLib
{
    public abstract class VersionedSettings
    {
        [JsonProperty(
            PropertyName = "version",
            Order = int.MinValue,
            Required = Required.Always)]
        public Version SettingsVersion { get; }

        [JsonIgnore]
        public abstract Version CurrentVersion { get; }
    }
}
