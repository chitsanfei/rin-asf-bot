using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
using Newtonsoft.Json;

namespace ArchiSteamFarm.CustomPlugins.Rin.Api;

internal static class CatAPI {
	private const string Url = "https://api.thecatapi.com";

	internal static async Task<Uri?> GetRandomCatUrl(WebBrowser webBrowser, CancellationToken cancellationToken = default) {
		ArgumentNullException.ThrowIfNull(webBrowser);

		Uri request = new($"{Url}/v1/images/search");

		ObjectResponse<ImmutableList<MeowResponse>>? response = await webBrowser.UrlGetToJsonObject<ImmutableList<MeowResponse>>(request).ConfigureAwait(false);

		return response?.Content?.FirstOrDefault()?.Url;
	}
}

[SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
internal sealed class MeowResponse {
	[JsonProperty("url", Required = Required.Always)]
	internal readonly Uri Url = null!;

	[JsonConstructor]
	private MeowResponse() { }
}