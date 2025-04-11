using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArchiSteamFarm.CustomPlugins.Rin
{
    public static class PluginLogger
    {
        public static void LogGenericException(Exception e)
        {
            Console.WriteLine($"[ERROR] {e}");
        }
    }

    public sealed class RinConfig
    {
        private const string ConfigFileName = "RinConfig.json";
        private static RinConfig? _instance;
        private static readonly string ConfigDirectory = Path.Combine(Directory.GetCurrentDirectory(), "config");
        private static readonly string ConfigFilePath = Path.Combine(ConfigDirectory, ConfigFileName);

        public static RinConfig Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                try
                {
                    if (!Directory.Exists(ConfigDirectory))
                    {
                        Directory.CreateDirectory(ConfigDirectory);
                    }

                    if (File.Exists(ConfigFilePath))
                    {
                        string json = File.ReadAllText(ConfigFilePath);
                        _instance = JsonSerializer.Deserialize<RinConfig>(json) ?? new RinConfig();
                    }
                    else
                    {
                        _instance = new RinConfig();
                        SaveConfig();
                    }
                }
                catch (Exception ex)
                {
                    PluginLogger.LogGenericException(ex);
                    _instance = new RinConfig();
                }

                return _instance;
            }
        }

        public static void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(_instance, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception e)
            {
                PluginLogger.LogGenericException(e);
            }
        }

        [JsonInclude]
        public bool SetuAllowR18 { get; init; }

        [JsonInclude]
        public int SetuMaxCount { get; init; }

        [JsonInclude]
        public bool SetuEnableMixedMode { get; init; }

        [JsonInclude]
        public double SetuR18Ratio { get; init; }

        [JsonInclude]
        public bool SetuEnableTimeRestriction { get; init; }

        [JsonInclude]
        public DateTime SetuAllowedTimeStart { get; init; }

        [JsonInclude]
        public DateTime SetuAllowedTimeEnd { get; init; }

        [JsonInclude]
        public int SetuMaxRequestsPerMinute { get; init; }

        [JsonConstructor]
        private RinConfig()
        {
            SetuAllowR18 = false;
            SetuMaxCount = 10;
            SetuEnableMixedMode = false;
            SetuR18Ratio = 0.2;
            SetuEnableTimeRestriction = false;
            SetuAllowedTimeStart = DateTime.Now.AddDays(-7);
            SetuAllowedTimeEnd = DateTime.Now.AddDays(7);
            SetuMaxRequestsPerMinute = 30;
        }

        public static void ReloadConfig()
        {
            _instance = null;
            _ = Instance;
        }
    }
}
