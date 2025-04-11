using System;
using System.IO;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.CustomPlugins.Bot.Rin.Localization;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using System.Collections.Generic;

namespace ArchiSteamFarm.CustomPlugins.Rin;

public class Utils
{
    /// <summary>
    /// Check if R18 content is allowed based on configuration
    /// </summary>
    /// <returns>True if R18 content is allowed, false otherwise</returns>
    public static bool IsR18Allowed()
    {
        return RinConfig.Instance.SetuAllowR18;
    }

    /// <summary>
    /// Check if the current time is within the allowed time range
    /// </summary>
    /// <returns>True if current time is allowed, false otherwise</returns>
    public static bool IsTimeAllowed()
    {
        if (!RinConfig.Instance.SetuEnableTimeRestriction)
        {
            return true;
        }

        var currentTime = DateTime.Now;
        var start = RinConfig.Instance.SetuAllowedTimeStart;
        var end = RinConfig.Instance.SetuAllowedTimeEnd;
        
        return currentTime >= start && currentTime <= end;
    }

    /// <summary>
    /// Check if user has sufficient access level and R18 permission
    /// </summary>
    /// <param name="access">User's access level</param>
    /// <returns>True if user has sufficient permissions, false otherwise</returns>
    public static bool HasSufficientPermission(EAccess access)
    {
        return access >= EAccess.Operator && RinConfig.Instance.SetuAllowR18;
    }

    /// <summary>
    /// Check if mixed mode is enabled and determine R18 content based on ratio
    /// </summary>
    /// <returns>True if R18 content should be included, false otherwise</returns>
    public static bool ShouldIncludeR18InMixedMode()
    {
        return RinConfig.Instance.SetuEnableMixedMode && Random.Shared.NextDouble() < RinConfig.Instance.SetuR18Ratio;
    }

    /// <summary>
    /// Check if user has exceeded request rate limit
    /// </summary>
    /// <param name="userLimits">Dictionary tracking user request limits</param>
    /// <param name="steamId">Steam ID of the user</param>
    /// <returns>True if rate limit exceeded, false otherwise</returns>
    public static bool IsRateLimitExceeded(Dictionary<ulong, (int count, DateTime lastRequestTime)> userLimits, ulong steamId)
    {
        if (userLimits.TryGetValue(steamId, out var userLimit) && (DateTime.Now - userLimit.lastRequestTime).TotalMinutes < 1)
        {
            return userLimit.count >= RinConfig.Instance.SetuMaxRequestsPerMinute;
        }
        return false;
    }

    /// <summary>
    /// Update user request count in rate limiting dictionary
    /// </summary>
    /// <param name="userLimits">Dictionary tracking user request limits</param>
    /// <param name="steamId">Steam ID of the user</param>
    public static void UpdateUserRequestCount(Dictionary<ulong, (int count, DateTime lastRequestTime)> userLimits, ulong steamId)
    {
        if (userLimits.TryGetValue(steamId, out var userLimit) && (DateTime.Now - userLimit.lastRequestTime).TotalMinutes < 1)
        {
            userLimits[steamId] = (userLimit.count + 1, DateTime.Now);
        }
        else
        {
            userLimits[steamId] = (1, DateTime.Now);
        }
    }
}