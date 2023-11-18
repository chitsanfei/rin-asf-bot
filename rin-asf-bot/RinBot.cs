using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.CustomPlugins.Bot.Rin.Localization;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
using ArchiSteamFarm.CustomPlugins.Rin.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamKit2;

namespace ArchiSteamFarm.CustomPlugins.Rin
{
	
	[Export(typeof(IPlugin))]
	internal sealed class RinBot : IASF, IBot, IBotCommand2, IBotConnection, IBotFriendRequest, IBotMessage, IBotModules, IBotTradeOffer
	{
		public string Name => nameof(RinBot);

		public Version Version => typeof(RinBot).Assembly.GetName().Version ?? throw new InvalidOperationException(nameof(Version));

		[JsonProperty]
		public bool CustomIsEnabledField { get; private set; } = true;

		public Task OnASFInit(IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
		{
			if (additionalConfigProperties == null)
			{
				return Task.CompletedTask;
			}

			foreach (KeyValuePair<string, JToken> configProperty in additionalConfigProperties)
			{
				switch (configProperty.Key)
				{
					case nameof(RinBot) + "TestProperty" when configProperty.Value.Type == JTokenType.Boolean:
						bool exampleBooleanValue = configProperty.Value.Value<bool>();
						break;
				}
			}
			return Task.CompletedTask;
		}

		public Task OnLoaded()
		{
			ASF.ArchiLogger.LogGenericWarning(Langs.OnRinLoaded);
			return Task.CompletedTask;
		}

		public Task OnBotInit(Steam.Bot bot)
		{
			ASF.ArchiLogger.LogGenericWarning(Langs.InitWarning + Langs.DebugASFVersion);
			ASF.ArchiLogger.LogGenericWarning(Langs.InitProgramUnstableWarning);
			return Task.CompletedTask;
		}
		
		public Task OnBotLoggedOn(Steam.Bot bot) => Task.CompletedTask;
		
		public async Task<string?> OnBotCommand(Steam.Bot bot, EAccess access, string message, string[] args, ulong steamID = 0)
		{
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
				case string arg when string.Equals(arg, "SETU", StringComparison.OrdinalIgnoreCase):
					return await getUrlOrErrorMessage(SetuAPI.GetRandomSetuURL(bot.ArchiWebHandler.WebBrowser), Langs.SetuNotFound);
				case string arg when string.Equals(arg, "R18", StringComparison.OrdinalIgnoreCase) && access >= EAccess.Operator:
					return await getUrlOrErrorMessage(SetuAPI.GetRandomSetuR18URL(bot.ArchiWebHandler.WebBrowser), Langs.SetuNotFound);
				case string arg when string.Equals(arg, "R18", StringComparison.OrdinalIgnoreCase):
					return Langs.NoPermissionWarning;
				case string arg when string.Equals(arg, "ANIME", StringComparison.OrdinalIgnoreCase):
					return await getUrlOrErrorMessage(AnimePicAPI.GetRandomAnimePic(bot.ArchiWebHandler.WebBrowser), Langs.AnimePicNotFound);
				case string arg when string.Equals(arg, "HITO", StringComparison.OrdinalIgnoreCase):
					return await getUrlOrErrorMessage(HitokotoAPI.GetHitokotoText(bot.ArchiWebHandler.WebBrowser), Langs.HitokotoNotFound);
				case string arg when string.Equals(arg, "CAT", StringComparison.OrdinalIgnoreCase):
					return await getUriOrErrorMessage(CatAPI.GetRandomCatURL(bot.ArchiWebHandler.WebBrowser), Langs.CatNotFoundOrLost);
				case string arg when string.Equals(arg, "DOG", StringComparison.OrdinalIgnoreCase):
					return await getUriOrErrorMessage(DogAPI.GetRandomDogURL(bot.ArchiWebHandler.WebBrowser), Langs.DogNotFoundOrLost);
				case string arg when string.Equals(arg, "H", StringComparison.OrdinalIgnoreCase):
					return Langs.HelpMenu;
				case string arg when string.Equals(arg, "ABT", StringComparison.OrdinalIgnoreCase):
					return Langs.About;
				default:
					return Langs.OutOfOrderList;
			}
		}

		public Task OnBotDisconnected(Steam.Bot bot, EResult reason)
		{
			ASF.ArchiLogger.LogGenericWarning(Langs.BotDisconnectedWarning);
			return Task.CompletedTask;
		}
		
		public async Task OnBotInitModules(Steam.Bot bot, IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
		{
			await bot.Actions.Pause(true).ConfigureAwait(false);
		}

		public Task<string?> OnBotMessage(Steam.Bot bot, ulong steamID, string message)
		{
			if (Steam.Bot.BotsReadOnly == null)
			{
				throw new InvalidOperationException(nameof(Steam.Bot.BotsReadOnly));
			}

			if (Steam.Bot.BotsReadOnly.Values.Any(existingBot => existingBot.SteamID == steamID))
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
					string reply = string.Format("/pre 🤔 -> SteamUser64ID:{0}\n{1}", steamID, Langs.WebLinkWarning);
					return Task.FromResult((string?)reply);
				}
			}

			return Task.FromResult((string?)"");
		}

		public Task<bool> OnBotTradeOffer(Steam.Bot bot, TradeOffer tradeOffer) => Task.FromResult(false);
		public Task<bool> OnBotFriendRequest(Steam.Bot bot, ulong steamID) => Task.FromResult(false);
		public Task OnBotDestroy(Steam.Bot bot) => Task.CompletedTask;

	}
}
