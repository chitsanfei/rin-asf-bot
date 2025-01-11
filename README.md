> [!CAUTION]
> 暂无力维护本项目，本项目的最后一次成功建构使用 dotnet 8.0, C# 12, ASF 6.0.5.2；
> 目前已知至 24/10/7 ASF 的所有高于 6.0.5.2 的新版本的建构环境是 dotnet 8.0.8，这会导致本插件挂载时导致以下错误：
> System.TypeLoadException: Could not load type 'System.Diagnostics.DebuggerNonUserCodeAttribute' from assembly 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
> 鉴于这是一个运行时错误，且可能和 dotnet 的更新有关，建议若您使用，自行编译 ASF 本体或暂时关闭更新，使用 6.0.5.2 版本。
> 
> This project is currently unmaintained. The last successful build of this project used .NET 8.0, C# 12, and ASF 6.0.5.2. As of 24/10/7, it is known that all ASF versions beyond 6.0.5.2 are built with .NET 8.0.8, which causes the  following error when loading this plugin:
> System.TypeLoadException: Could not load type 'System.Diagnostics.DebuggerNonUserCodeAttribute' from assembly 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
> Since this is a runtime error likely related to updates in .NET, it is recommended that if you use this project, either compile the ASF core yourself or temporarily disable updates and use version 6.0.5.2.

<div align="center">
  <img src="https://raw.githubusercontent.com/chitsanfei/rin-asf-bot/master/assets/banner.png" height="200">
  <h1>chitsanfei/RinBot</h1>
  <p>A highly useable Steam Pic Posting Bot on Steam, dev based on ASF.</p>
</div>

<br>

<p align="center">
  <a href="https://github.com/chitsanfei/rin-asf-bot/issues"><img src="https://img.shields.io/github/issues/chitsanfei/rin-asf-bot" alt="Github Issue"></a>
  <a href="https://github.com/chitsanfei/rin-asf-bot/fork"><img src="https://img.shields.io/github/forks/chitsanfei/rin-asf-bot" alt="Github Forks"></a>
  <a href="https://github.com/chitsanfei/rin-asf-bot"><img src="https://img.shields.io/github/stars/chitsanfei/rin-asf-bot" alt="Github Stars"></a>
  <a href="https://github.com/chitsanfei/rin-asf-bot/blob/master/LICENSE"><img src="https://img.shields.io/github/license/chitsanfei/rin-asf-bot" alt="GitHub License"></a>
</p>

<p align="center">
  <img src="https://repobeats.axiom.co/api/embed/10309d9ebe0dad4128646852628802e7dfe79ea3.svg" alt="Repobeats analytics image">
</p>

## Muti-Language Option
[Simplified Chinese | 简体中文](./assets/docs/README_zh_CN.md)  
[Traditional Chinese | 繁體中文](./assets/docs/README_zh_TW.md)  
[Japanese | 日本語](./assets/docs/README_ja_JP.md)  
[German | Deutsch](./assets/docs/README_de_DE.md)  

## Description
Rin is designed for giving steam gamers better experience, based [ArchiSteamFarm](https://github.com/JustArchiNET/ArchiSteamFarm).

Rin is named from Chinese character 「凛」, no special meaning.

---

### Features
- Post anime pictures, including steam group chat and private chat.
- Post cute cats.

### Compatibility
- RinBot should fully support to ArchiSteamFarm on Windows or Linux or MacOS.
- RinBot is available only on specific version of ArchiSteamFarm, usually we notify which version we support when we make a new release. You shall use proper version of ASF at least no lower than we recommand one.

### Manual Compliation
```
dotnet build -c Release
```

## Help & How to Use
- Please visit our Github [Wiki Page](https://github.com/chitsanfei/rin-asf-bot/wiki).
