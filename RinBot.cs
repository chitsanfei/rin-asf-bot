using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Data;
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
			ASF.ArchiLogger.LogGenericWarning(LocalizationZHCN.OnRinLoaded);
			return Task.CompletedTask;
		}

		public Task OnBotInit(Bot bot)
		{
			ASF.ArchiLogger.LogGenericWarning(LocalizationZHCN.InitWarning);
			ASF.ArchiLogger.LogGenericWarning(LocalizationZHCN.InitProgramUnstableWarning);
			return Task.CompletedTask;
		}
		
		public Task OnBotLoggedOn(Bot bot) => Task.CompletedTask;
		
		public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamID = 0)
		{
			switch (args[0].ToUpperInvariant())
			{
				case "SETU":
					string? randomSetuURL = await SetuAPI.GetRandomSetuURL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(randomSetuURL) ? randomSetuURL : LocalizationZHCN.SetuNotFound;
				case "R18" when access >= EAccess.Operator:
					string? randomSetuR18URL = await SetuAPI.GetRandomSetuR18URL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(randomSetuR18URL) ? randomSetuR18URL : LocalizationZHCN.SetuNotFound;
				case "HITO":
					string? hitokoto = await HitokotoAPI.GetHitokotoText(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return !string.IsNullOrEmpty(hitokoto) ? hitokoto : LocalizationZHCN.HitokotoNotFound;
				case "CAT":
					Uri? randomCatURL = await CatAPI.GetRandomCatURL(bot.ArchiWebHandler.WebBrowser).ConfigureAwait(false);
					return randomCatURL != null ? randomCatURL.ToString() : LocalizationZHCN.CatNotFoundOrLost;
				case "H": return LocalizationZHCN.HelpMenu;
				default:
					return null;
			}
		}

		public Task OnBotDisconnected(Bot bot, EResult reason)
		{
			ASF.ArchiLogger.LogGenericWarning(LocalizationZHCN.BotDisconnectedWarning);
			return Task.CompletedTask;
		}
		
		public Task<bool> OnBotFriendRequest(Bot bot, ulong steamID) => Task.FromResult(true);

		public Task OnBotDestroy(Bot bot) => Task.CompletedTask;

		public async Task OnBotInitModules(Bot bot, IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
		{
			await bot.Actions.Pause(true).ConfigureAwait(false);
		}

		public Task<string?> OnBotMessage(Bot bot, ulong steamID, string message)
		{
			if (Bot.BotsReadOnly == null)
			{
				throw new InvalidOperationException(nameof(Bot.BotsReadOnly));
			}

			if (Bot.BotsReadOnly.Values.Any(existingBot => existingBot.SteamID == steamID))
			{
				return Task.FromResult<string?>(null);
			}
			return Task.FromResult((string?)"");
		}

		public Task<bool> OnBotTradeOffer(Bot bot, TradeOffer tradeOffer) => Task.FromResult(false);

	}
}
