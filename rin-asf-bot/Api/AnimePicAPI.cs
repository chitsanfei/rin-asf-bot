using System;
using System.Threading.Tasks;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
namespace ArchiSteamFarm.CustomPlugins.Rin.Api;

internal static class AnimePicAPI
{
    /// <summary>
    ///    URL to the API.
    /// </summary>
    private const string Url = "https://www.dmoe.cc/random.php?return=json";

    /// <summary>
    ///   Get a random anime picture.
    ///   
    ///   <para>Return null if failed.</para>
    ///   <para>Return a string if success.</para>
    ///   <para>Return a string with a URL of a random anime picture.</para>
    /// </summary>
    internal static async Task<string?> GetRandomAnimePic(WebBrowser webBrowser)
    {
        ArgumentNullException.ThrowIfNull(webBrowser);

        Uri request = new($"{Url}");

        ObjectResponse<LoliApiResponse>? response = await webBrowser.UrlGetToJsonObject<LoliApiResponse>(request).ConfigureAwait(false);

        return response?.Content?.imgurl;
    }

    /// <summary>
    ///     The response of the API.
    /// </summary>
    private class LoliApiResponse
    {
        public string code { get; set; }
        public string imgurl { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }
}

