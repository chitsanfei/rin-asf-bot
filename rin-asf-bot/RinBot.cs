using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.CustomPlugins.Bot.Rin.Localization;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using ArchiSteamFarm.CustomPlugins.Rin.Api;
using SteamKit2;

namespace ArchiSteamFarm.CustomPlugins.Rin
{
	
	[Export(typeof(IPlugin))]
	internal sealed class RinBot : IASF, IBot, IBotCommand2, IBotConnection, IBotFriendRequest, IBotMessage, IBotModules, IBotTradeOffer
	{
		public string Name => nameof(RinBot);

		public Version Version => typeof(RinBot).Assembly.GetName().Version ?? throw new InvalidOperationException(nameof(Version));

		[JsonInclude]
		public bool CustomIsEnabledField { get; private set; } = true;

		private readonly Dictionary<ulong, (int count, DateTime lastRequestTime)> UserRequestLimits = new Dictionary<ulong, (int, DateTime)>();

		public Task OnASFInit(IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties = null) 
		{
			if (additionalConfigProperties == null) {
				return Task.CompletedTask;
			}
			
			return Task.CompletedTask;
		}
		
		public Task OnLoaded()
		{
			ASF.ArchiLogger.LogGenericWarning(Langs.InitRinLoaded);
			return Task.CompletedTask;
		}

		
		public async Task OnBotInitModules(Steam.Bot bot, IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties = null)
		{
			bot.ArchiLogger.LogGenericInfo("Pausing this bot as asked from the plugin");
			await bot.Actions.Pause(true).ConfigureAwait(false);
		}

		public Task OnBotInit(Steam.Bot bot)
		{
			ASF.ArchiLogger.LogGenericWarning(Langs.InitNotice + Langs.VersionASF);
			ASF.ArchiLogger.LogGenericWarning(Langs.InitProgramUnstable);
			return Task.CompletedTask;
		}
		
		public Task OnBotLoggedOn(Steam.Bot bot) => Task.CompletedTask;
		
		public async Task<string?> OnBotCommand(Steam.Bot bot, EAccess access, string message, string[] args, ulong steamId = 0)
		{
			const int maxRequestsPerMinute = 30;
			if (UserRequestLimits.TryGetValue(steamId, out var userLimit) && (DateTime.Now - userLimit.lastRequestTime).TotalMinutes < 1)
			{
				if (userLimit.count >= maxRequestsPerMinute)
				{
					return Langs.WarningRateLimit;
				}
				UserRequestLimits[steamId] = (userLimit.count + 1, DateTime.Now);
			}
			else
			{
				UserRequestLimits[steamId] = (1, DateTime.Now);
			}

			Func<Task<string?>, string, Task<string?>> getUrlOrErrorMessage = async (getUrlTask, errorMessage) =>
			{
				string? url = await getUrlTask.ConfigureAwait(false);
				return !string.IsNullOrEmpty(url) ? url : errorMessage;
			};

			Func<Task<Uri?>, string, Task<string?>> getUriOrErrorMessage = async (getUriTask, errorMessage) =>
			{
				Uri? uri = await getUriTask.ConfigureAwait(false);
				return uri != null ? uri.ToString() : errorMessage;
			};

			switch (args[0])
			{
				case { } arg when string.Equals(arg, "SETU", StringComparison.OrdinalIgnoreCase):
					return await getUrlOrErrorMessage(SetuAPI.GetRandomSetuUrl(bot.ArchiWebHandler.WebBrowser), Langs.WarningSetuLost).ConfigureAwait(false);
				case { } arg when string.Equals(arg, "R18", StringComparison.OrdinalIgnoreCase) && access >= EAccess.Operator:
					return await getUrlOrErrorMessage(SetuAPI.GetRandomSetuR18Url(bot.ArchiWebHandler.WebBrowser), Langs.WarningSetuLost).ConfigureAwait(false);
				case { } arg when string.Equals(arg, "R18", StringComparison.OrdinalIgnoreCase):
					if (Utils.CheckFileExists())
					{
						return await getUrlOrErrorMessage(SetuAPI.GetRandomSetuR18Url(bot.ArchiWebHandler.WebBrowser), Langs.WarningSetuLost).ConfigureAwait(false);
					}
					return Langs.WarningNoPermission;
				case { } arg when string.Equals(arg, "ANIME", StringComparison.OrdinalIgnoreCase):
					return await getUrlOrErrorMessage(AnimePicAPI.GetRandomAnimePic(bot.ArchiWebHandler.WebBrowser), Langs.WarningAnimePicLost).ConfigureAwait(false);
				case { } arg when string.Equals(arg, "CAT", StringComparison.OrdinalIgnoreCase):
					return await getUriOrErrorMessage(CatAPI.GetRandomCatUrl(bot.ArchiWebHandler.WebBrowser), Langs.WarningCatLost).ConfigureAwait(false);
				case { } arg when string.Equals(arg, "H", StringComparison.OrdinalIgnoreCase):
					return Langs.HelpMenu;
				case { } arg when string.Equals(arg, "ABT", StringComparison.OrdinalIgnoreCase):
					return Langs.About;
				default:
					return Langs.WarningNoCommand;
			}
		}

		public Task OnBotDisconnected(Steam.Bot bot, EResult reason)
		{
			ASF.ArchiLogger.LogGenericWarning(Langs.WarningBotDisconnected);
			return Task.CompletedTask;
		}

		public Task<string?> OnBotMessage(Steam.Bot bot, ulong steamId, string message)
		{
			if (Steam.Bot.BotsReadOnly == null)
			{
				throw new InvalidOperationException(nameof(Steam.Bot.BotsReadOnly));
			}

			if (Steam.Bot.BotsReadOnly.Values.Any(existingBot => existingBot.SteamID == steamId))
			{
				return Task.FromResult<string?>(null);
			}
			
			if (message.Contains('.', StringComparison.OrdinalIgnoreCase) && message.Length > 4)
			{
				HashSet<string> webDomainList = new HashSet<string> 
				{ 
					"http", ".top", ".com", ".cat", ".mba", ".cn", ".xyz", ".cc", ".co", ".icu", ".uk", ".us", ".ca", ".sh", ".sk", ".st", ".au",
					".net", ".org", ".info", ".biz", ".name", ".museum", ".edu", ".gov", ".int", ".eu", ".aero", ".pro", ".travel", ".tel", ".jobs", ".coop", ".mobi", ".asia", ".post", ".xxx", ".global",
					".de", ".fr", ".it", ".es", ".pl", ".ru", ".br", ".in", ".jp", ".kr", ".vn", ".mx", ".ar", ".za", ".ch", ".se", ".no", ".fi", ".dk", ".be", ".at", ".ir", ".il", ".pt", ".nz", ".hk", ".my", ".sg", ".th", ".ph", ".id"
				};
				if (webDomainList.Any(s => message.Contains(s, StringComparison.OrdinalIgnoreCase)))
				{
					string reply = $"/pre 🤔 -> SteamUser64ID:{steamId}\n{Langs.WarningWebLink}";
					return Task.FromResult((string?)reply);
				}
			}

			return Task.FromResult((string?)"");
		}

		public Task<bool> OnBotTradeOffer(Steam.Bot bot, TradeOffer tradeOffer) => Task.FromResult(false);
		public Task<bool> OnBotFriendRequest(Steam.Bot bot, ulong steamId) => Task.FromResult(false);
		public Task OnBotDestroy(Steam.Bot bot) => Task.CompletedTask;

	}
}
