using System;
using System.Text.Json.Serialization;
using ArchiSteamFarm.Core;

namespace ArchiSteamFarm.CustomPlugins.Rin;

public sealed class RinConfig {
    private static RinConfig? _instance;
    public static RinConfig Instance => _instance ??= new RinConfig();

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
    private RinConfig() {
        SetuAllowR18 = false;
        SetuMaxCount = 10;
        SetuEnableMixedMode = false;
        SetuR18Ratio = 0.2;
        SetuEnableTimeRestriction = false;
        SetuAllowedTimeStart = DateTime.Now.AddDays(-7);
        SetuAllowedTimeEnd = DateTime.Now.AddDays(7);
        SetuMaxRequestsPerMinute = 30;
    }
}