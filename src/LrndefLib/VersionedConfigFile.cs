using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using TShockAPI.Configuration;

namespace LrndefLib
{
    public class VersionedConfigFile<TSettings> : IConfigFile<TSettings>
        where TSettings : VersionedSettings
    {
        public SimpleVersion CurrentVersion { get; }

        public TSettings Settings { get; set; }

        public VersionedConfigFile(Version currentVersion)
        {
            CurrentVersion = currentVersion;
        }

        TSettings IConfigFile<TSettings>.ConvertJson(string json, out bool incompleteSettings)
        {
            return FromJSON(json, out incompleteSettings);
        }

        public TSettings FromJSON(string json, out bool incompleteSettings)
        {
            TSettings deserialized = JsonConvert.DeserializeObject<TSettings>(json);

            if (deserialized.SettingsVersion != CurrentVersion)
            {
                incompleteSettings = true;
                throw new NotImplementedException();
            }
            else
            {
                incompleteSettings = false;
            }

            Settings = deserialized;

            return deserialized;
        }

        public TSettings Read(string path, out bool incompleteSettings)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Read(stream, out incompleteSettings);
            }
        }

        public TSettings Read(Stream stream, out bool incompleteSettings)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return FromJSON(reader.ReadToEnd(), out incompleteSettings);
            }
        }

        public void Write(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream);
            }
        }

        public void Write(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string json = JsonConvert.SerializeObject(Settings);
                writer.Write(json);
            }
        }
    }
}
