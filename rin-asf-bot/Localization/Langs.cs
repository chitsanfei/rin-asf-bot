using System;

namespace ArchiSteamFarm.CustomPlugins.Bot.Rin.Localization
{
    internal static class Langs
    {
        public static string VersionASF => "6.3.0.2";
        public static string VersionPlugin => "1.1.0.0";
        public static string VersionDate => "2025/12/13";
        public static string InitNotice => "RinBotPlugin: Rin现在正在进行加载过程，测试版本为";
        public static string InitProgramUnstable => "RinBotPlugin: 这是一个不稳定的构建！";
        public static string InitRinLoaded => "RinBotPlugin: Rin加载完成！当前版本：";
        public static string SuccessSetu => "🎨 色图已发送！";
        public static string SuccessAnime => "🌸 动漫图片已发送！";
        public static string SuccessCat => "🐱 猫猫图片已发送！";
        public static string SuccessR18 => "🔥 R18 内容已发送！（仅限 18+）";
        public static string ErrorSetuFailed => "❌ 色图获取失败！";
        public static string ErrorAnimeFailed => "❌ 动漫图片获取失败！";
        public static string ErrorCatFailed => "❌ 猫猫图片获取失败！";
        public static string ErrorR18NotAllowed => "❌ R18 内容需要特殊权限！";
        public static string WarningRateLimit => "⏱️ 请求过于频繁，请稍后再试！";
        public static string WarningWebLink => "Rin侦测到可能的一条链接哦！诈骗多发，请务必警惕陌生链接，网页千万条安全第一条。不要输入自己的任何账号密码，泄漏财产信息哦！！！";
        public static string WarningBotDisconnected => "⚠️ Bot 已断开连接！";
        public static string HelpMessage => "🤖 RinBot 帮助菜单：\n/SETU [数量] - 获取色图\n/R18 [数量] - 获取 R18 内容（需权限）\n/ANIME [数量] - 获取动漫图片\n/CAT [数量] - 获取猫猫图片\n/H - 显示此帮助菜单\n/ABT - 关于插件";
        public static string AboutMessage => "ℹ️ RinBot 插件信息：\n版本：1.1.0.0\nASF 版本：6.3.0.2\n构建日期：2025/12/13\n作者：@chitsanfei\nGitHub：https://github.com/chitsanfei/rin-asf-bot";
        public static string WarningParamIllegal => "您设置的参数是非法参数！";
        public static string WarningParamOutrage => "参数过大！";
        public static string WarningNoPermission => "❌ 您没有权限执行此操作！";
        public static string WarningSetuLost => "❌ 色图获取失败！";
        public static string WarningAnimePicLost => "❌ 动漫图片获取失败！";
        public static string WarningCatLost => "❌ 猫猫图片获取失败！";
        public static string HelpMenu => "🤖 RinBot 帮助菜单：\n/SETU [数量] - 获取色图\n/R18 [数量] - 获取 R18 内容（需权限）\n/ANIME [数量] - 获取动漫图片\n/CAT [数量] - 获取猫猫图片\n/H - 显示此帮助菜单\n/ABT - 关于插件";
        public static string About => "ℹ️ RinBot 插件信息";
        public static string WarningNoCommand => "❌ 未知的命令！";
        public static string WarningWorkflow => "执行流出现了错误，触发区域并没有设置异常抛出，请联系开发者获取支持。\n发生在方法体：";
    }
}
