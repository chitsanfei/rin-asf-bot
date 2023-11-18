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
	private const string URL = "https://api.thecatapi.com";

	internal static async Task<Uri?> GetRandomCatURL(WebBrowser webBrowser, CancellationToken cancellationToken = default) {
		ArgumentNullException.ThrowIfNull(webBrowser);

		Uri request = new($"{URL}/v1/images/search");

		ObjectResponse<ImmutableList<MeowResponse>>? response = await webBrowser.UrlGetToJsonObject<ImmutableList<MeowResponse>>(request).ConfigureAwait(false);

		return response?.Content?.FirstOrDefault()?.URL;
	}
}

[SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
internal sealed class MeowResponse {
	[JsonProperty("url", Required = Required.Always)]
	internal readonly Uri URL = null!;

	[JsonConstructor]
	private MeowResponse() { }
}