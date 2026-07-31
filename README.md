# TaoTie（饕餮）

基于 Unity 的游戏开发框架，包含组件式 UI 框架、资源管理系统、Excel 配置导出工具链及编辑器扩展。。

## 环境要求

- Unity `2022.3.x`
- .NET 后端

## 核心功能

### 框架模块

| 模块 | 说明 |
| --- | --- |
| **UI 框架** | 组件式 UI 框架，支持多层窗口管理、生命周期回调（OnCreate/OnEnable/OnDisable/OnDestroy）、宽度适配 |
| **资源管理** | 基于 [YooAsset](https://github.com/tuyoogame/YooAsset) 的资源管理系统，支持动态图集、Unity 内置 SpriteAtlas 图集 |
| **配置系统** | 基于 protobuf-net 的 Excel 配置导出/读取工具，支持全量与单张表导出、懒加载反序列化 |
| **热更新** | 可方便接入 HybridCLR 或 ILRuntime 实现代码热更新 |
| **渲染管线** | 轻量级 Render Pipeline，支持 Forward+ / Deferred 渲染路径 |
| **异步任务** | 基于 ETTask 的 async/await 异步框架 |

## 项目结构

```
TaoTie/
├── Assets/Scripts/
│   ├── Code/                  # 游戏逻辑（可热更新）
│   │   ├── Game/              # 游戏玩法（Scene / UI）
│   │   └── Module/            # 框架模块（Camera / Config / Input / Resource / UI ...）
│   ├── Editor/                # 编辑器工具（BuildEditor / ArtEditor / UIManager ...）
│   ├── Mono/                  # 非热更层（Core / Helper / Module）
│   │   ├── Core/              # Manager / ObjectPool
│   │   └── Module/           # Assembly / Timer / UI / YooAssets ...
│   └── ThirdParty/            # 第三方库（ETTask / LitJson / protobuf-net / SuperScrollView）
├── Modules/                   # UPM 本地包
│   └── com.tuyoogame.yooasset/# 资源管理
├── Excel/                     # Excel 配置及导表工具
├── Packages/                  # UPM 依赖
└── ProjectSettings/           # Unity 项目设置
```

## 快速开始

1. Clone 仓库：
   ```bash
   git clone https://github.com/526077247/TaoTie.git
   ```
2. 使用 Unity 打开项目。
3. 等待 Unity 完成包导入和编译。

### Excel 配置导出

```bash
# 导出全部配置
Excel/win_startExportAll.bat

# 导出 I18N 配置
Excel/win_startI18NExport.bat
```

## 相关仓库

| 仓库 | 说明 |
| --- | --- |
| [TaoWu（梼杌）](https://github.com/526077247/TaoWu) | Cocos 引擎版本框架 |
| [QiongQi（穷奇）](https://github.com/526077247/QiongQi) | UE 引擎版本框架 |
| [HunDun（混沌）](https://github.com/526077247/HunDun) | Godot 引擎版本框架 |
| [GenshinGamePlay](https://github.com/526077247/GenshinGamePlay) | GamePlay 上游仓库（战斗、AI、解谜） |
| [TaoTieRP](https://github.com/526077247/TaoTieRP) | 轻量级 Render Pipeline，支持 Forward+ / Deferred 渲染路径 |

## License

[MIT](LICENSE)
