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
	internal static class HitokotoAPI
	{
		private const string URL = "https://v1.hitokoto.cn/?encode=json";

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

			if (string.IsNullOrEmpty(response.Content.Hitokoto))
			{
				throw new InvalidOperationException(nameof(response.Content.Hitokoto));
			}

			return response.Content.Hitokoto;
		}

		private class HitokotoJson
		{
			public string id { get; set; }
			public string uuid { get; set; }
			public string hitokoto { get; set; }
			public string type { get; set; }
			public string from { get; set; }
			public string from_who { get; set; }
			public string creator { get; set; }
			public int creator_uid { get; set; }
			public int reviewer { get; set; }
			public string commit_from { get; set; }
			public string created_at { get; set; }
			public int length { get; set; }
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
			internal readonly string From_who;

			[JsonProperty(PropertyName = "creator", Required = Required.Always)]
			internal readonly string Creator;

			[JsonProperty(PropertyName = "creator_uid", Required = Required.Always)]
			internal readonly int Creator_uid;

			[JsonProperty(PropertyName = "reviewer", Required = Required.Always)]
			internal readonly int Reviewer;

			[JsonProperty(PropertyName = "commit_from", Required = Required.Always)]
			internal readonly string Commit_from;

			[JsonProperty(PropertyName = "created_at", Required = Required.Always)]
			internal readonly string Commit_at;

			[JsonProperty(PropertyName = "length", Required = Required.Always)]
			internal readonly int Length;

			[JsonConstructor]
			private HitokotoResponse() { }
		}

	}
}
