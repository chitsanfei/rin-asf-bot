using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

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

        // 保存到 ASF config 目录
        private static readonly string ConfigDirectory = Path.Combine(AppContext.BaseDirectory, "config");
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
                    Console.WriteLine($"[RinConfig] Loading configuration from: {ConfigFilePath}");
                    Console.WriteLine($"[RinConfig] Config directory exists: {Directory.Exists(ConfigDirectory)}");

                    if (!Directory.Exists(ConfigDirectory))
                    {
                        Console.WriteLine($"[RinConfig] Creating config directory: {ConfigDirectory}");
                        Directory.CreateDirectory(ConfigDirectory);
                    }

                    if (File.Exists(ConfigFilePath))
                    {
                        Console.WriteLine($"[RinConfig] Reading existing config file");
                        using (var stream = File.OpenRead(ConfigFilePath))
                        using (var reader = new StreamReader(stream))
                        {
                            string json = reader.ReadToEnd();
                            Console.WriteLine($"[RinConfig] JSON content length: {json.Length}");
                            _instance = JsonSerializer.Deserialize<RinConfig>(json, GetJsonOptions()) ?? new RinConfig();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[RinConfig] Config file not found, creating new one");
                        _instance = new RinConfig();
                        SaveConfig();
                    }

                    Console.WriteLine($"[RinConfig] Configuration loaded successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[RinConfig] ERROR: {ex.Message}");
                    Console.WriteLine($"[RinConfig] StackTrace: {ex.StackTrace}");
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
                Console.WriteLine($"[RinConfig] Saving configuration to: {ConfigFilePath}");
                string json = JsonSerializer.Serialize(_instance, GetJsonOptions());
                Console.WriteLine($"[RinConfig] JSON serialized, length: {json.Length}");

                // 确保目录存在
                if (!Directory.Exists(ConfigDirectory))
                {
                    Console.WriteLine($"[RinConfig] Creating directory: {ConfigDirectory}");
                    Directory.CreateDirectory(ConfigDirectory);
                }

                // 写入文件
                using (var stream = File.OpenWrite(ConfigFilePath))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                    writer.Flush();
                }

                Console.WriteLine($"[RinConfig] Configuration saved successfully");
                Console.WriteLine($"[RinConfig] File exists: {File.Exists(ConfigFilePath)}");
                Console.WriteLine($"[RinConfig] File size: {new FileInfo(ConfigFilePath).Length} bytes");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[RinConfig] Save ERROR: {e.Message}");
                Console.WriteLine($"[RinConfig] Save StackTrace: {e.StackTrace}");
                PluginLogger.LogGenericException(e);
            }
        }

        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions(JsonSerializerDefaults.Strict)
            {
                WriteIndented = true,
                PropertyNamingPolicy = null,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };
        }

        public bool SetuAllowR18 { get; init; }

        public int SetuMaxCount { get; init; }

        public bool SetuEnableMixedMode { get; init; }

        public double SetuR18Ratio { get; init; }

        public bool SetuEnableTimeRestriction { get; init; }

        public DateTime SetuAllowedTimeStart { get; init; }

        public DateTime SetuAllowedTimeEnd { get; init; }

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
