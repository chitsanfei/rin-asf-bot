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
			switch (args[0].ToUpperInvariant())
			{
				case "SETU":
					string? randomSetuURL = await SetuAPI.GetRandomSetuURL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(randomSetuURL) ? randomSetuURL : Langs.SetuNotFound;
				case "R18" when access >= EAccess.Operator:
					string? randomSetuR18URL = await SetuAPI.GetRandomSetuR18URL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(randomSetuR18URL) ? randomSetuR18URL : Langs.SetuNotFound;
				case "R18" when access < EAccess.Operator:
					return Langs.NoPermissionWarning;
				case "HITO":
					string? hitokoto = await HitokotoAPI.GetHitokotoText(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(hitokoto) ? hitokoto : Langs.HitokotoNotFound;
				case "CAT":
					Uri? randomCatURL = await CatAPI.GetRandomCatURL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return randomCatURL != null ? randomCatURL.ToString() : Langs.CatNotFoundOrLost;
				case "H": return Langs.HelpMenu;
				case "ABT": return Langs.About;
				default:
					return Langs.OutOfOrderList;
			}
		}

		public Task OnBotDisconnected(Steam.Bot bot, EResult reason)
		{
			ASF.ArchiLogger.LogGenericWarning(Langs.BotDisconnectedWarning);
			return Task.CompletedTask;
		}
		
		public Task<bool> OnBotFriendRequest(Steam.Bot bot, ulong steamID) => Task.FromResult(true);

		public Task OnBotDestroy(Steam.Bot bot) => Task.CompletedTask;

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
			
			if (message.ToUpperInvariant().Contains('.') & message.Length > 4)
			{
				string[] webDomainList = { "http", ".top", ".com", ".cat", ".mba", ".cn", ".xyz", ".cc", ".co", ".icu", ".uk", ".us", ".ca", ".sh", ".sk", ".st", ".au" };
				if (webDomainList.Any(s => s.ToLowerInvariant().Contains(s)))
				{
					string reply = "/pre " + $"🤔 -> SteamUser64ID:{steamID}\n" + Langs.WebLinkWarning;
					return Task.FromResult((string?)reply);
				}
			}
			
			return Task.FromResult((string?)"");
		}

		public Task<bool> OnBotTradeOffer(Steam.Bot bot, TradeOffer tradeOffer) => Task.FromResult(false);

	}
}
