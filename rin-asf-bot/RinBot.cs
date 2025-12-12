/// <summary>
/// ASF RinBot Plugin by @chitsanfei
/// A plugin for ArchiSteamFarm that provides various entertainment features
/// </summary>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Composition;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using SteamKit2;
using ArchiSteamFarm.CustomPlugins.Bot.Rin.Localization;
using ArchiSteamFarm.CustomPlugins.Rin.Api;

/// <summary>
/// Main plugin class that implements various ASF interfaces for bot functionality
/// </summary>
namespace ArchiSteamFarm.CustomPlugins.Rin {
    [Export(typeof(IPlugin))]
    internal sealed class RinBot : IASF, IBot, IBotCommand2, IBotConnection, IBotFriendRequest, IBotMessage, IBotModules {
        
        /// <summary>
        /// Gets the name of the plugin
        /// </summary>
        [JsonInclude]
        [Required]
        public string Name => nameof(RinBot);

        /// <summary>
        /// Gets the version of the plugin
        /// </summary>
        [JsonInclude]
        [Required]
        public Version Version => typeof(RinBot).Assembly.GetName().Version ?? throw new InvalidOperationException(nameof(Version));

        /// <summary>
        /// Gets or sets whether the plugin is enabled
        /// </summary>
        [JsonInclude]
        [Required]
        public bool CustomIsEnabledField { get; private init; } = false;

        /// <summary>
        /// Dictionary to track and limit user requests per minute
        /// </summary>
        private readonly Dictionary<ulong, (int count, DateTime lastRequestTime)> UserRequestLimits = new Dictionary<ulong, (int, DateTime)>();

        /// <summary>
        /// Initializes ASF plugin
        /// </summary>
        /// <param name="additionalConfigProperties">Additional configuration properties</param>
        public Task OnASFInit(IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties = null) {
            _ = RinConfig.Instance;
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// Called when plugin is loaded
        /// </summary>
        public Task OnLoaded() {
            ASF.ArchiLogger.LogGenericWarning(Langs.InitRinLoaded);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Initializes bot modules
        /// </summary>
        /// <param name="bot">The bot instance</param>
        /// <param name="additionalConfigProperties">Additional configuration properties</param>
        public Task OnBotInitModules(ArchiSteamFarm.Steam.Bot bot, IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties = null) => Task.CompletedTask;

        /// <summary>
        /// Initializes the bot
        /// </summary>
        /// <param name="bot">The bot instance</param>
        public Task OnBotInit(ArchiSteamFarm.Steam.Bot bot) {
            ASF.ArchiLogger.LogGenericWarning($"{Langs.InitNotice}{Langs.VersionASF}\n{Langs.InitProgramUnstable}");
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// Called when bot logs on to Steam
        /// </summary>
        /// <param name="bot">The bot instance</param>
        public Task OnBotLoggedOn(ArchiSteamFarm.Steam.Bot bot) => Task.CompletedTask;
        
        /// <summary>
        /// Processes bot commands
        /// </summary>
        /// <param name="bot">The bot instance</param>
        /// <param name="access">Access level of the user</param>
        /// <param name="message">The message content</param>
        /// <param name="args">Command arguments</param>
        /// <param name="steamID">Steam ID of the user</param>
        /// <returns>Response message or null</returns>
        public async Task<string?> OnBotCommand(ArchiSteamFarm.Steam.Bot bot, EAccess access, string message, string[] args, ulong steamID = 0) {
            // Rate Limiting
            if (Utils.IsRateLimitExceeded(UserRequestLimits, steamID)) {
                return Langs.WarningRateLimit;
            }
            Utils.UpdateUserRequestCount(UserRequestLimits, steamID);

            // Helper Functions to Get URL or Error Message
            Func<Task<string?>, string, Task<string?>> getUrlOrErrorMessage = async (getUrlTask, errorMessage) => {
                string? url = await getUrlTask.ConfigureAwait(false);
                return !string.IsNullOrEmpty(url) ? url : errorMessage;
            };

            Func<Task<Uri?>, string, Task<string?>> getUriOrErrorMessage = async (getUriTask, errorMessage) => {
                Uri? uri = await getUriTask.ConfigureAwait(false);
                return uri != null ? uri.ToString() : errorMessage;
            };

            // Command Handling
            switch (args[0].ToUpperInvariant()) {
                case "SETU":
                    if (args.Length == 1)
                    {
                        int r18Flag = Utils.ShouldIncludeR18InMixedMode() ? 1 : 0;
                        return await getUrlOrErrorMessage(SetuAPI.GetRandomSetu(bot.ArchiWebHandler.WebBrowser, new List<int> {r18Flag, 1, 1}), Langs.WarningSetuLost).ConfigureAwait(false);
                    }
                    else if (args[1].Length > 0)
                    {
                        if (!int.TryParse(args[1], out int requestCount))
                        {
                            return Langs.WarningParamIllegal;
                        }

                        if (requestCount > RinConfig.Instance.SetuMaxCount)
                        {
                            return Langs.WarningParamOutrage;
                        }

                        int r18Flag = Utils.ShouldIncludeR18InMixedMode() ? 1 : 0;
                        return await getUrlOrErrorMessage(SetuAPI.GetRandomSetu(bot.ArchiWebHandler.WebBrowser, new List<int> {r18Flag, requestCount, 1}), Langs.WarningSetuLost).ConfigureAwait(false);
                    }
                    else
                    {
                        return Langs.WarningWorkflow;
                    }
                case "R18":
                    // Check permissions and time restrictions
                    if (!Utils.HasSufficientPermission(access) || !Utils.IsTimeAllowed())
                    {
                        return Langs.WarningNoPermission;
                    }

                    // Process the command
                    if (args.Length == 1)
                    {
                        return await getUrlOrErrorMessage(SetuAPI.GetRandomSetu(bot.ArchiWebHandler.WebBrowser, new List<int> {1, 1, 1}), Langs.WarningSetuLost).ConfigureAwait(false);
                    }
                    
                    if (args[1].Length > 0)
                    {
                        if (!int.TryParse(args[1], out int requestCount))
                        {
                            return Langs.WarningParamIllegal;
                        }

                        if (requestCount > RinConfig.Instance.SetuMaxCount)
                        {
                            return Langs.WarningParamOutrage;
                        }

                        return await getUrlOrErrorMessage(SetuAPI.GetRandomSetu(bot.ArchiWebHandler.WebBrowser, new List<int> {1, requestCount, 1}), Langs.WarningSetuLost).ConfigureAwait(false);
                    }
                    
                    return Langs.WarningWorkflow;
                case "ANIME":
                    return await getUrlOrErrorMessage(AnimePicAPI.GetRandomAnimePic(bot.ArchiWebHandler.WebBrowser), Langs.WarningAnimePicLost).ConfigureAwait(false);
                case "CAT":
                    return await getUriOrErrorMessage(CatAPI.GetRandomCatUrl(bot.ArchiWebHandler.WebBrowser), Langs.WarningCatLost).ConfigureAwait(false);
                case "H":
                    return Langs.HelpMenu;
                case "ABT":
                    return $"{Langs.About}\n" +
                           $"插件版本：{Langs.VersionPlugin}\n" +
                           $"建构时间：{Langs.VersionDate}\n" +
                           $"ASF 测试版本：{Langs.VersionASF}";
                default:
                    return Langs.WarningNoCommand;
            }
        }

        // Bot Disconnected Event
        /// <summary>
        /// Called when bot disconnects from Steam
        /// </summary>
        /// <param name="bot">The bot instance</param>
        /// <param name="reason">Reason for disconnection</param>
        public Task OnBotDisconnected(ArchiSteamFarm.Steam.Bot bot, EResult reason) {
            ASF.ArchiLogger.LogGenericWarning(Langs.WarningBotDisconnected);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Processes messages sent to the bot
        /// </summary>
        /// <param name="bot">The bot instance</param>
        /// <param name="steamId">Steam ID of the sender</param>
        /// <param name="message">The message content</param>
        /// <returns>Response message or null</returns>
        public Task<string?> OnBotMessage(ArchiSteamFarm.Steam.Bot bot, ulong steamId, string message) {
            if (ArchiSteamFarm.Steam.Bot.BotsReadOnly == null) {
                throw new InvalidOperationException(nameof(ArchiSteamFarm.Steam.Bot.BotsReadOnly));
            }

            // Check if Message Contains a Web Link
            if (ArchiSteamFarm.Steam.Bot.BotsReadOnly.Values.Any(existingBot => existingBot.SteamID == steamId)) {
                return Task.FromResult<string?>(null);
            }

            if (message.Contains('.', StringComparison.OrdinalIgnoreCase) && message.Length > 4) {
                HashSet<string> webDomainList = new HashSet<string> {
                    "http", ".top", ".com", ".cat", ".mba", ".cn", ".xyz", ".cc", ".co", ".icu", ".uk", ".us", ".ca", ".sh", ".sk", ".st", ".au",
                    ".net", ".org", ".info", ".biz", ".name", ".museum", ".edu", ".gov", ".int", ".eu", ".aero", ".pro", ".travel", ".tel", ".jobs", ".coop", ".mobi", ".asia", ".post", ".xxx", ".global",
                    ".de", ".fr", ".it", ".es", ".pl", ".ru", ".br", ".in", ".jp", ".kr", ".vn", ".mx", ".ar", ".za", ".ch", ".se", ".no", ".fi", ".dk", ".be", ".at", ".ir", ".il", ".pt", ".nz", ".hk", ".my", ".sg", ".th", ".ph", ".id"
                };
                if (webDomainList.Any(s => message.Contains(s, StringComparison.OrdinalIgnoreCase))) {
                    string reply = $"/pre 🤔 -> SteamUser64ID:{steamId}\n{Langs.WarningWebLink}";
                    return Task.FromResult((string?)reply);
                }
            }

            return Task.FromResult((string?)"");
        }

        // Bot Friend Request Event
        public Task<bool> OnBotFriendRequest(ArchiSteamFarm.Steam.Bot bot, ulong steamId) => Task.FromResult(false);

        // Bot Destroy Event
        /// <summary>
        /// Called when bot is being destroyed
        /// </summary>
        /// <param name="bot">The bot instance</param>
        public Task OnBotDestroy(ArchiSteamFarm.Steam.Bot bot) => Task.CompletedTask;
    }
}
