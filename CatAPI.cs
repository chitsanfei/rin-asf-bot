using System;
using System.Threading.Tasks;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ArchiSteamFarm.CustomPlugins.Rin;

internal static class CatAPI
{
	private const string URL = "https://aws.random.cat";

	internal static async Task<Uri?> GetRandomCatURL(WebBrowser webBrowser)
	{
		ArgumentNullException.ThrowIfNull(webBrowser);

		Uri request = new($"{URL}/meow");

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
