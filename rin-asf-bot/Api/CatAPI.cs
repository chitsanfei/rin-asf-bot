using System;
using System.Threading.Tasks;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ArchiSteamFarm.CustomPlugins.Rin.Api;

internal static class CatAPI
{
	private const string URL = "https://api.thecatapi.com";

	internal static async Task<Uri?> GetRandomCatURL(WebBrowser webBrowser)
	{
		ArgumentNullException.ThrowIfNull(webBrowser);

		Uri request = new($"{URL}/v1/images/search");

		ObjectResponse<MeowResponse>? response = await webBrowser.UrlGetToJsonObject<MeowResponse>(request).ConfigureAwait(false);

		return response?.Content?.URL;
	}
}

[SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
internal sealed class MeowResponse
{
	[JsonProperty("file", Required = Required.Always)]
	internal readonly Uri URL = null!;

	[JsonConstructor]
	private MeowResponse() { }
}
