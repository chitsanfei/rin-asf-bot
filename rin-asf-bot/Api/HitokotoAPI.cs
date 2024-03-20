using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ArchiSteamFarm;
using Newtonsoft.Json;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;

namespace ArchiSteamFarm.CustomPlugins.Rin.Api
{
	internal static class HitokotoApi
	{
		/// <summary>
		/// URL to the API. hitokoto.cn. 
		/// </summary>
		private const string URL = "https://v1.hitokoto.cn/?encode=json";

		/// <summary>
		/// Get a random hitokoto text.
		/// </summary>
		/// <param name="webBrowser"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		internal static async Task<string?> GetHitokotoText(WebBrowser webBrowser)
		{
			if (webBrowser == null)
			{
				throw new ArgumentNullException(nameof(webBrowser));
			}

			Uri request = new($"{URL}");

			ObjectResponse<HitokotoResponse>? response = await webBrowser.UrlGetToJsonObject<HitokotoResponse>(request).ConfigureAwait(false);
			if (response == null)
			{
				return null;
			}

			if (string.IsNullOrEmpty(response.Content?.Hitokoto))
			{
				throw new InvalidOperationException(nameof(response.Content.Hitokoto));
			}

			return response.Content.Hitokoto;
		}


		[SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
		private sealed class HitokotoResponse
		{
			[JsonProperty(PropertyName = "id", Required = Required.Always)]
			internal readonly string Id;

			[JsonProperty(PropertyName = "uuid", Required = Required.Always)]
			internal readonly string Uuid;

			[JsonProperty(PropertyName = "hitokoto", Required = Required.Always)]
			internal readonly string Hitokoto;

			[JsonProperty(PropertyName = "type", Required = Required.Always)]
			internal readonly string Type;

			[JsonProperty(PropertyName = "from", Required = Required.Always)]
			internal readonly string From;

			[JsonProperty(PropertyName = "from_who", Required = Required.AllowNull)]
			internal readonly string FromWho;

			[JsonProperty(PropertyName = "creator", Required = Required.Always)]
			internal readonly string Creator;

			[JsonProperty(PropertyName = "creator_uid", Required = Required.Always)]
			internal readonly int CreatorUid;

			[JsonProperty(PropertyName = "reviewer", Required = Required.Always)]
			internal readonly int Reviewer;

			[JsonProperty(PropertyName = "commit_from", Required = Required.Always)]
			internal readonly string CommitFrom;

			[JsonProperty(PropertyName = "created_at", Required = Required.Always)]
			internal readonly string CommitAt;

			[JsonProperty(PropertyName = "length", Required = Required.Always)]
			internal readonly int Length;

			[JsonConstructor]
			private HitokotoResponse() { }
		}

	}
}
