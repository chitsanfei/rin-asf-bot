using System;
using System.IO;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.CustomPlugins.Bot.Rin.Localization;

namespace ArchiSteamFarm.CustomPlugins.Rin;

public class Utils
{
    /// <summary>
    ///  <summary>Check if the file exists, and return if R18 is all-allowed</summary>
    /// </summary>
    /// <returns></returns>
    public static bool CheckFileExists()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "r18");

        return File.Exists(filePath);
    }
}