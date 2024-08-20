using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		string quality = "regular";
	
		switch (args[0])
		{
			case 0:
				r18 = 0;
				quality = "regular";
				break;

			case 1:
				r18 = 1;
				quality = "regular";
				break;

            default:
				r18 = 0;
                quality = "regular";
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

        if (response == null || response.Content == null || response.Content.data == null)
        {
            return string.Empty;
        }

        if (requestNum == 1)
        {
            return response.Content.data[0].urls.regular;
        }

        var resultBuilder = new System.Text.StringBuilder();

        foreach (var item in response.Content.data)
        {
            if (!string.IsNullOrEmpty(item.urls.regular))
            {
                resultBuilder.AppendLine(item.urls.regular);
            }
        }

        return resultBuilder.ToString();
    }

    /// <summary>
    /// The response of the API.
    /// </summary>
    private class LoliconJson
    {
        public string error { get; set; }
        public List<SetuImageJson> data { get; set; }
    }

    /// <summary>
    /// The response of the API. (Setu Image)
    /// </summary>
    private class SetuImageJson
    {
        public int pid { get; set; }
        public int p { get; set; }
        public int uid { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public Urls urls { get; set; }
        public bool r18 { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<string> tags { get; set; }
        public string ext { get; set; }
        public long uploadDate { get; set; }
    }

    private class Urls
    {
        public string regular { get; set; }
        //public string original { get; set; }
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
