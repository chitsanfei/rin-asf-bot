
using SteamKit2.GC.Dota.Internal;

namespace ArchiSteamFarm.CustomPlugins.Rin;

/// <summary>
/// For program localization.
/// </summary>
public static class Localization
{
    public const string Version = "1.0.1";
    public const string Author = "MashiroSA";
    public const string LastEditedTime = "2022.7.29";
    public const string DebugASFVersion = "5.2.8.3";
}

/// <summary>
/// 简体中文的本地化内容。
/// </summary>
public static class Langs
{
    /// <summary>
    /// log中的提示语。
    /// </summary>
    public const string InitWarning = "RinBotPlugin: Rin现在正在进行加载过程～";
    public const string InitProgramUnstableWarning = $"RinBotPlugin: 这是一个不稳定的构建，测试使用版本为{Localization.DebugASFVersion}";
    public const string OnRinLoaded = "RinBotPlugin: 欢迎您！Rin已经被ASF主程序侦测到并装载了，接下来需要进入加载过程！";
    public const string BotDisconnectedWarning = "RinBotPlugin: Rin掉线了哦！ASF主程序将尝试自动重连！";
    
    /// <summary>
    /// 指令回复。
    /// </summary>
    public const string HelpMenu = "/pre 欢迎您使用RinBot呢，茫茫人海相遇可是一种缘分～\n" +
        "本拉胯Bot的分支设计者:MashiroSAMAowo\n" +
        "输入下列指令获得相应功能：\n" +
        "!setu:获取一张色图\n" +
        "!r18:获取限制的色图（已被限制仅高权限使用）\n" +
        "!hito:获取一句一言\n" +
        "!cat:获取随机的猫猫图（ASF官方例程）\n" +
        "!abt:寻找关于这个bot的一切，包括到底是哪个憨批作者写出来的！";
    public const string SetuNotFound = "没有找到相应的内容或者内容呢丢失了！QAQ";
    public const string HitokotoNotFound = "好像一言走丢了哦！QAQ";
    public const string CatNotFoundOrLost = "不好！好像猫猫走丢了哦！QAQ";
    public const string NoPermissionWarning = "对不起！你的权限不够哟！:3";
    public const string OutOfOrderList = "好像没有对应的指令?输入「!h」可以查看可以使用的指令。";
    public const string About = "/code Github开源项目链接：https://github.com/ShizukuWorld/rin-asf-bot \n" +
        "分支开发作者Steam：https://steamcommunity.com/id/mrmarketsama/";
}