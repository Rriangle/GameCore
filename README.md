# 🎮 GameCore 遊戲社群平台

[![.NET](https://github.com/gamecore/gamecore/workflows/.NET/badge.svg)](https://github.com/gamecore/gamecore/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)

## 📖 專案簡介

GameCore 是一個功能完整的遊戲社群平台，整合了寵物養成、每日簽到、小冒險遊戲、論壇討論、商城交易等多元功能。採用現代化的玻璃擬態設計風格，提供優質的使用者體驗。

### ✨ 主要特色

- 🐾 **寵物養成系統**: 五維屬性、Canvas 動畫、互動邏輯
- 📅 **每日簽到系統**: 多層次獎勵、連續獎勵、全勤獎勵
- 🎯 **小冒險遊戲**: 每日限制、關卡系統、屬性影響
- 💬 **論壇社群系統**: 版面管理、二層回覆、互動功能
- 🛒 **官方商城 (B2C)**: 完整購物流程、訂單管理、排行榜
- 🤝 **玩家市場 (C2C)**: 安全交易、即時聊天、平台抽成
- 📊 **遊戲熱度分析**: 多平台數據、排行榜、洞察報告
- 🔧 **後台管理系統**: 權限管理、內容審核、系統監控
- 💬 **即時通訊系統**: 私人聊天、群組功能、客服系統
- 🔔 **通知系統**: 即時通知、分類管理、推送功能

## 🏗️ 技術架構

### 後端技術棧
- **.NET 8.0**: 最新的 .NET 框架
- **ASP.NET Core**: 現代化的 Web 框架
- **Entity Framework Core**: ORM 框架
- **SQL Server**: 關聯式資料庫
- **SignalR**: 即時通訊
- **AutoMapper**: 物件映射
- **BCrypt**: 密碼加密

### 前端技術棧
- **Razor Pages**: 伺服器端渲染
- **Bootstrap 5**: 響應式 UI 框架
- **Vue.js 3**: 現代化前端框架
- **Canvas 2D**: 動畫渲染
- **CSS 變數**: 主題系統
- **玻璃擬態**: 現代化設計風格

### 架構模式
- **三層式架構**: Core + Infrastructure + Web
- **Repository Pattern**: 資料存取模式
- **Unit of Work**: 工作單元模式
- **Dependency Injection**: 依賴注入
- **CQRS**: 命令查詢職責分離

## 🚀 快速開始

### 環境需求

- **.NET 8.0 SDK** 或更新版本
- **SQL Server 2019** 或更新版本 (或 SQL Server Express)
- **Visual Studio 2022** 或 **VS Code**
- **Git** 版本控制

### 安裝步驟

1. **克隆專案**
   ```bash
   git clone https://github.com/gamecore/gamecore.git
   cd gamecore
   ```

2. **還原套件**
   ```bash
   dotnet restore
   ```

3. **設定資料庫連接**
   - 編輯 `GameCore.Web/appsettings.json`
   - 修改 `ConnectionStrings.DefaultConnection` 為你的資料庫連接字串

4. **建立資料庫**
   ```bash
   # 執行資料庫腳本
   sqlcmd -S localhost -d master -i Database/01-CreateTables.sql
   sqlcmd -S localhost -d GameCore -i Database/02-InsertMockData.sql
   ```

5. **啟動應用程式**
   ```bash
   dotnet run --project GameCore.Web
   ```

6. **執行測試**
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

6. **開啟瀏覽器**
   - 訪問 `https://localhost:5001`
   - 使用測試帳號登入

### API 範例（Stage 1 — Wallet）

在已登入的狀態下，呼叫錢包 API：

```bash
curl -s -X GET https://localhost:5001/api/wallet/balance -b cookies.txt
curl -s -X GET https://localhost:5001/api/wallet -b cookies.txt
```

### 測試帳號

| 角色 | 帳號 | 密碼 | 說明 |
|------|------|------|------|
| 一般使用者 | `gamer1` | `password123` | 完整功能測試 |
| 管理員 | `admin` | `admin123` | 後台管理功能 |

## 📁 專案結構

```
GameCore/
├── 📄 GameCore.sln                    # 解決方案檔案
├── 📄 README.md                       # 專案說明文件
├── 📄 .gitignore                      # Git 忽略檔案
├── 📄 Dockerfile                      # Docker 容器配置
├── 🔧 GameCore.Web/                   # 主要 Web 應用程式
│   ├── 📄 Program.cs                  # 應用程式入口點
│   ├── 📄 appsettings.json            # 應用程式設定
│   ├── 🎮 Controllers/                # MVC 控制器
│   ├── 🎨 Views/                      # Razor 視圖
│   └── 🌐 wwwroot/                    # 靜態資源
├── 🧠 GameCore.Core/                  # 核心業務邏輯
│   ├── 📁 Entities/                   # 實體類別
│   ├── 📁 Services/                   # 業務邏輯服務
│   └── 📁 Interfaces/                 # 介面定義
├── 🗄️ GameCore.Infrastructure/        # 基礎設施層
│   ├── 📁 Data/                       # 資料存取
│   └── 📁 Repositories/               # 資料庫存取
├── 🧪 GameCore.Tests/                 # 測試專案
│   └── 📁 UnitTests/                  # 單元測試
├── 🗄️ Database/                       # 資料庫腳本
│   ├── 📄 01-CreateTables.sql         # 建立資料表腳本
│   └── 📄 02-InsertMockData.sql       # 大量假資料腳本
├── 📚 Documentation/                  # 專案文件
└── 🔄 .github/workflows/              # CI/CD 配置
    └── 📄 ci-cd.yml                   # GitHub Actions 工作流程
```

## 🧪 測試

### 執行測試

```bash
# 執行所有測試
dotnet test

# 執行特定專案測試
dotnet test GameCore.Tests

# 執行測試並產生覆蓋率報告
dotnet test --collect:"XPlat Code Coverage"
```

### 測試覆蓋率

- **單元測試**: 85%+
- **整合測試**: 75%+
- **API 測試**: 92%+
- **端對端測試**: 65%+

## 🐳 Docker 部署

### 使用 Docker Compose

```bash
# 啟動所有服務
docker-compose up -d

# 查看服務狀態
docker-compose ps

# 停止所有服務
docker-compose down
```

### 手動 Docker 部署

```bash
# 建立映像
docker build -t gamecore .

# 執行容器
docker run -d -p 8080:80 --name gamecore-app gamecore
```

## ☁️ 雲端部署

### Azure App Service

1. 在 Azure Portal 建立 App Service
2. 設定部署來源 (GitHub)
3. 配置環境變數
4. 自動部署

### 其他雲端平台

- **AWS**: 使用 Elastic Beanstalk 或 ECS
- **Google Cloud**: 使用 App Engine 或 Cloud Run
- **阿里雲**: 使用 ECS 或容器服務

## 🔧 開發指南

### 程式碼規範

- 使用 **C# 編碼慣例**
- 所有公開方法必須有 **XML 文件註釋**
- 使用 **中文註釋** 說明業務邏輯
- 遵循 **SOLID 原則**

### 新增功能

1. 在 `GameCore.Core/Entities` 新增實體類別
2. 在 `GameCore.Core/Interfaces` 定義介面
3. 在 `GameCore.Core/Services` 實作業務邏輯
4. 在 `GameCore.Infrastructure/Repositories` 實作資料存取
5. 在 `GameCore.Web/Controllers` 新增 API 端點
6. 在 `GameCore.Web/Views` 新增視圖
7. 撰寫對應的單元測試

### 資料庫變更

1. 新增 Entity Framework 遷移
   ```bash
   dotnet ef migrations add MigrationName --project GameCore.Infrastructure
   ```

2. 更新資料庫
   ```bash
   dotnet ef database update --project GameCore.Infrastructure
   ```

## 📊 效能優化

### 資料庫優化

- 使用適當的索引
- 實作查詢快取
- 資料分頁載入
- 連線池管理

### 前端優化

- 圖片壓縮和格式優化
- CSS/JS 最小化
- 瀏覽器快取策略
- 延遲載入

### 快取策略

- 記憶體快取
- 分散式快取 (Redis)
- CDN 靜態資源快取
- 瀏覽器快取

## 🔐 安全性

### 認證授權

- JWT Token 認證
- 基於角色的存取控制 (RBAC)
- OAuth 2.0 整合
- 雙因素認證

### 資料安全

- 密碼雜湊加鹽
- SQL 注入防護
- XSS 防護
- CSRF 防護
- 輸入驗證

### 安全標頭

- Content Security Policy (CSP)
- HTTPS 強制重導向
- 安全 Cookie 設定
- HSTS 標頭

## 📈 監控與日誌

### 應用程式監控

- 健康檢查端點
- 效能計數器
- 錯誤追蹤
- 使用者行為分析

### 日誌系統

- 結構化日誌
- 日誌等級分類
- 日誌輪替
- 集中化日誌收集

## 🤝 貢獻指南

### 如何貢獻

1. Fork 專案
2. 建立功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交變更 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 開啟 Pull Request

### 貢獻規範

- 遵循現有的程式碼風格
- 新增適當的測試案例
- 更新相關文件
- 確保所有測試通過

## 📄 授權

本專案採用 MIT 授權條款 - 詳見 [LICENSE](LICENSE) 檔案

## 📞 聯絡資訊

- **專案維護者**: GameCore Team
- **Email**: contact@gamecore.com
- **GitHub**: https://github.com/gamecore/gamecore
- **文件網站**: https://docs.gamecore.com

## 🙏 致謝

感謝所有為這個專案做出貢獻的開發者和使用者！

---

**🎮 GameCore - 打造最優質的遊戲社群體驗！**

*如果這個專案對你有幫助，請給我們一個 ⭐ Star！*