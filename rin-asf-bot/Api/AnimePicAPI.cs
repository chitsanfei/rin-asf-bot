using System;
using System.Threading.Tasks;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ArchiSteamFarm.CustomPlugins.Rin.Api;

internal static class AnimePicAPI
{
    /// <summary>
    ///    URL to the API.
    /// </summary>
    private const string URL = "https://www.dmoe.cc/random.php?return=json";

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

        Uri request = new($"{URL}");

        ObjectResponse<LoliApiResponse>? response = await webBrowser.UrlGetToJsonObject<LoliApiResponse>(request).ConfigureAwait(false);

        return response.Content.imgurl;
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

