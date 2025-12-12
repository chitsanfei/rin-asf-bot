using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ArchiSteamFarm.CustomPlugins.Bot.Rin.Localization
{
    internal static class Langs
    {
        private static readonly string ResourcePath = Path.Combine(AppContext.BaseDirectory, "Localization", "Langs.json");
        private static readonly Dictionary<string, string> _translations = new();

        static Langs()
        {
            try
            {
                string jsonContent = File.ReadAllText(ResourcePath);
                using JsonDocument document = JsonDocument.Parse(jsonContent);
                foreach (JsonProperty property in document.RootElement.EnumerateObject())
                {
                    _translations[property.Name] = property.Value.GetString() ?? property.Name;
                }
            }
            catch (Exception)
            {
                // Fallback values are provided by the properties themselves
            }
        }

        private static string GetString(string key, string fallback)
        {
            return _translations.TryGetValue(key, out string? value) && !string.IsNullOrEmpty(value)
                ? value
                : fallback;
        }

        public static string VersionASF => GetString("VersionASF", "6.3.0.2");
        public static string VersionPlugin => GetString("VersionPlugin", "1.1.0.0");
        public static string VersionDate => GetString("VersionDate", "2025/12/13");
        public static string InitNotice => GetString("InitNotice", "RinBotPlugin: Rin现在正在进行加载过程，测试版本为");
        public static string InitProgramUnstable => GetString("InitProgramUnstable", "RinBotPlugin: 这是一个不稳定的构建！");
        public static string InitRinLoaded => GetString("InitRinLoaded", "RinBotPlugin: Rin加载完成！当前版本：");
        public static string SuccessSetu => GetString("SuccessSetu", "🎨 色图已发送！");
        public static string SuccessAnime => GetString("SuccessAnime", "🌸 动漫图片已发送！");
        public static string SuccessCat => GetString("SuccessCat", "🐱 猫猫图片已发送！");
        public static string SuccessR18 => GetString("SuccessR18", "🔥 R18 内容已发送！（仅限 18+）");
        public static string ErrorSetuFailed => GetString("ErrorSetuFailed", "❌ 色图获取失败！");
        public static string ErrorAnimeFailed => GetString("ErrorAnimeFailed", "❌ 动漫图片获取失败！");
        public static string ErrorCatFailed => GetString("ErrorCatFailed", "❌ 猫猫图片获取失败！");
        public static string ErrorR18NotAllowed => GetString("ErrorR18NotAllowed", "❌ R18 内容需要特殊权限！");
        public static string WarningRateLimit => GetString("WarningRateLimit", "⏱️ 请求过于频繁，请稍后再试！");
        public static string WarningWebLink => GetString("WarningWebLink", "检测到网页链接，请使用 Steam 社区或 Steam 商店链接！");
        public static string WarningBotDisconnected => GetString("WarningBotDisconnected", "⚠️ Bot 已断开连接！");
        public static string HelpMessage => GetString("HelpMessage", "🤖 RinBot 帮助菜单：\n/SETU [数量] - 获取色图\n/R18 [数量] - 获取 R18 内容（需权限）\n/ANIME [数量] - 获取动漫图片\n/CAT [数量] - 获取猫猫图片\n/H - 显示此帮助菜单\n/ABT - 关于插件");
        public static string AboutMessage => GetString("AboutMessage", "ℹ️ RinBot 插件信息：\n版本：1.1.0.0\nASF 版本：6.3.0.2\n构建日期：2025/12/13\n作者：@chitsanfei\nGitHub：https://github.com/chitsanfei/rin-asf-bot");
        public static string WarningParamIllegal => GetString("WarningParamIllegal", "您设置的参数是非法参数！");
        public static string WarningParamOutrage => GetString("WarningParamOutrage", "参数过大！");
        public static string WarningNoPermission => GetString("WarningNoPermission", "❌ 您没有权限执行此操作！");
        public static string WarningSetuLost => GetString("WarningSetuLost", "❌ 色图获取失败！");
        public static string WarningAnimePicLost => GetString("WarningAnimePicLost", "❌ 动漫图片获取失败！");
        public static string WarningCatLost => GetString("WarningCatLost", "❌ 猫猫图片获取失败！");
        public static string HelpMenu => GetString("HelpMenu", "🤖 RinBot 帮助菜单：\n/SETU [数量] - 获取色图\n/R18 [数量] - 获取 R18 内容（需权限）\n/ANIME [数量] - 获取动漫图片\n/CAT [数量] - 获取猫猫图片\n/H - 显示此帮助菜单\n/ABT - 关于插件");
        public static string About => GetString("About", "ℹ️ RinBot 插件信息");
        public static string WarningNoCommand => GetString("WarningNoCommand", "❌ 未知的命令！");
        public static string WarningWorkflow => GetString("WarningWorkflow", "执行流出现了错误，触发区域并没有设置异常抛出，请联系开发者获取支持。\n发生在方法体：");
    }
}
