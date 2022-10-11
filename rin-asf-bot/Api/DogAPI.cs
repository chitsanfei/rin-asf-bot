using System;
using System.Threading.Tasks;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ArchiSteamFarm.CustomPlugins.Rin.Api;

internal static class DogAPI
{
    private const string URL = "https://dog.ceo/api/breeds/image/random";

    internal static async Task<Uri?> GetRandomDogURL(WebBrowser webBrowser)
    {
        ArgumentNullException.ThrowIfNull(webBrowser);

        Uri request = new($"{URL}");

        ObjectResponse<DoggeResponse>? response = await webBrowser.UrlGetToJsonObject<DoggeResponse>(request).ConfigureAwait(false);

        return response?.Content?.URL;
    }
}

[SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
internal sealed class DoggeResponse
{
    [JsonProperty("message", Required = Required.Always)]
    internal readonly Uri URL = null!;

    [JsonConstructor]
    private DoggeResponse() { }
}