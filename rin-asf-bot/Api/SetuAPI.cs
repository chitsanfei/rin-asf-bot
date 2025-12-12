using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web;
using ArchiSteamFarm.Web.Responses;

namespace ArchiSteamFarm.CustomPlugins.Rin.Api
{
internal static class SetuAPI
{
    /// <summary>
    /// Picture API. For lolicon.app website.
    /// </summary>
    private const string URL = "https://api.lolicon.app/setu/v2";

    /// <summary>
    /// Fetch an animation picture based on specified arguments.
    /// </summary>
    /// <param name="webBrowser">ASF Web</param>
    /// <param name="args">A list of arguments [r18, requestNum, quality]</param>
    /// <returns>URL of the requested picture or multiple URLs</returns>
    /// <exception cref="ArgumentNullException">No Argument.</exception>
    /// <exception cref="InvalidOperationException">An invalid operation occurred.</exception>
    internal static async Task<string?> GetRandomSetu(WebBrowser webBrowser, List<int> args)
    {
        if (webBrowser == null)
        {
            throw new ArgumentNullException(nameof(webBrowser));
        }

        if (args == null || args.Count != 3)
        {
            throw new ArgumentException("Invalid arguments. Expected list with 3 integers.");
        }

        int r18 = 0;
        int requestNum = args[1];
		string quality = "original";

		switch (args[0])
		{
			case 0:
				r18 = 0;
				quality = "original";
				break;

			case 1:
				r18 = 1;
				quality = "original";
				break;

            default:
				r18 = 0;
                quality = "original";
                break;
        }

        // todo: when c# is 11.0 this is better

        // switch (args)
        // {
        //     case [0, 1, 1]:
        //         r18 = 0;
        //         requestNum = 1;
        //         quality = "regular";
        //         break;
        //     case [1, 1, 1]:
        //         r18 = 1;
        //         requestNum = 1;
        //         quality = "regular";
        //         break;
        //     case [0, 1, _]:
        //         r18 = 0;
        //         requestNum = 1;
        //         quality = "original";
        //         break;
        //     case [1, 1, _]:
        //         r18 = 1;
        //         requestNum = 1;
        //         quality = "original";
        //         break;
        //     case [0, int n, 1] when n > 1:
        //         r18 = 0;
        //         requestNum = n;
        //         quality = "regular";
        //         break;
        //     case [1, int n, 1] when n > 1:
        //         r18 = 1;
        //         requestNum = n;
        //         quality = "regular";
        //         break;
        //     default:
        //         throw new ArgumentException("Unsupported args configuration.");
        // }

        Uri request = new($"{URL}/?size={quality}&r18={r18}&num={requestNum}");

        ObjectResponse<LoliconJson>? response = await webBrowser.UrlGetToJsonObject<LoliconJson>(request).ConfigureAwait(false);

        if (response == null || response.Content == null)
        {
            return string.Empty;
        }

        // Check if API returned an error
        if (!string.IsNullOrEmpty(response.Content.Error))
        {
            Console.WriteLine($"Setu API Error: {response.Content.Error}");
            return string.Empty;
        }

        // Check if Data is null or empty
        if (response.Content.Data == null || response.Content.Data.Count == 0)
        {
            return string.Empty;
        }

        if (requestNum == 1)
        {
            // Return Original URL (since quality is set to "original")
            var firstItem = response.Content.Data[0];
            if (firstItem?.Urls?.Original != null)
            {
                return firstItem.Urls.Original;
            }
            // Fallback to regular if original is not available
            if (firstItem?.Urls?.Regular != null)
            {
                return firstItem.Urls.Regular;
            }
            return string.Empty;
        }

        var resultBuilder = new System.Text.StringBuilder();

        foreach (var item in response.Content.Data)
        {
            // Prefer Original URL, fallback to Regular
            if (item?.Urls?.Original != null)
            {
                resultBuilder.AppendLine(item.Urls.Original);
            }
            else if (item?.Urls?.Regular != null)
            {
                resultBuilder.AppendLine(item.Urls.Regular);
            }
        }

        return resultBuilder.ToString();
    }

    /// <summary>
    /// The response of the API.
    /// </summary>
    private class LoliconJson
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("data")]
        public List<SetuImageJson>? Data { get; set; }
    }

    /// <summary>
    /// The response of the API. (Setu Image)
    /// </summary>
    private class SetuImageJson
    {
        [JsonPropertyName("pid")]
        public int Pid { get; set; }

        [JsonPropertyName("p")]
        public int P { get; set; }

        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }

        [JsonPropertyName("urls")]
        public Urls? Urls { get; set; }

        [JsonPropertyName("r18")]
        public bool R18 { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        [JsonPropertyName("ext")]
        public string? Ext { get; set; }

        [JsonPropertyName("uploadDate")]
        public long UploadDate { get; set; }
    }

    private class Urls
    {
        [JsonPropertyName("original")]
        public string? Original { get; set; }

        [JsonPropertyName("regular")]
        public string? Regular { get; set; }

        [JsonPropertyName("small")]
        public string? Small { get; set; }

        [JsonPropertyName("thumb")]
        public string? Thumb { get; set; }

        [JsonPropertyName("compressed")]
        public string? Compressed { get; set; }
    }


		/*
		 * It may be useless in feature.
		 */

		//[SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
		//private sealed class SetuResponse
		//{
		//	[JsonProperty(PropertyName = "pid", Required = Required.Always)]
		//	internal readonly int Pid;
			
		//	[JsonProperty(PropertyName = "p", Required = Required.Always)]
		//	internal readonly int P;
			
		//	[JsonProperty(PropertyName = "uid", Required = Required.Always)]
		//	internal readonly int Uid;
			
		//	[JsonProperty(PropertyName = "title", Required = Required.Always)]
		//	internal readonly string Title = "";
			
		//	[JsonProperty(PropertyName = "author", Required = Required.Always)]
		//	internal readonly string Author = "";
			
		//	[JsonProperty(PropertyName = "url", Required = Required.Always)]
		//	internal readonly string Link = "";
			
		//	[JsonProperty(PropertyName = "r18", Required = Required.Always)]
		//	internal readonly bool R18;
			
		//	[JsonProperty(PropertyName = "width", Required = Required.Always)]
		//	internal readonly int Width = 0;
			
		//	[JsonProperty(PropertyName = "height", Required = Required.Always)]
		//	internal readonly int Height = 0;
			
		//	[JsonProperty(PropertyName = "tags", Required = Required.Always)]
		//	internal readonly string[] Tags = null;

		//	[JsonConstructor]
		//	private SetuResponse() { }
		//}
		
	}
}
