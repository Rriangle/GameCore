# GameCore 遊戲社群平台

![GameCore Logo](https://via.placeholder.com/800x200/7557ff/ffffff?text=GameCore+%E9%81%8A%E6%88%B2%E7%A4%BE%E7%BE%A4%E5%B9%B3%E5%8F%B0)

## 🎮 專案概述

GameCore 是一個功能完整的遊戲社群平台，整合了遊戲熱度觀測、論壇社群、官方商城、玩家自由市場（C2C）、寵物養成與小遊戲、即時訊息/群組與通知等功能。

### 🌟 核心特色

- **🐾 虛擬寵物系統**: 可愛的史萊姆養成，支援互動、升級、換色
- **📅 每日簽到系統**: 完整的獎勵機制，包含連續簽到和全勤獎勵
- **🎯 小冒險遊戲**: 每日限定的寵物冒險遊戲，獲得經驗和點數
- **🛒 雙重商城系統**: 官方商城 (B2C) + 玩家自由市場 (C2C)
- **💬 社群互動**: 論壇討論、即時聊天、群組功能
- **📊 遊戲熱度分析**: 多平台數據整合，即時排行榜
- **🎨 玻璃擬態設計**: 現代化的 UI 設計，支援深色模式
- **🔐 完整認證系統**: 支援傳統登入和 OAuth (Google, Facebook)

## 🛠 技術架構

### 後端技術
- **框架**: ASP.NET Core 8.0 MVC
- **資料庫**: SQL Server
- **ORM**: Entity Framework Core 8.0
- **認證**: Cookie Authentication + OAuth
- **即時通訊**: SignalR
- **密碼加密**: BCrypt
- **API 文件**: Swagger/OpenAPI

### 前端技術
- **模板引擎**: Razor Pages
- **JavaScript 框架**: Vue.js 3
- **CSS 框架**: Bootstrap 5 + 自訂玻璃風格
- **動畫系統**: Canvas 2D + CSS Animations
- **響應式設計**: 支援桌面、平板、手機

### 測試技術
- **單元測試**: xUnit
- **模擬框架**: Moq
- **斷言庫**: FluentAssertions
- **測試資料庫**: EF Core InMemory

## 🏗 專案結構

```
GameCore/
├── GameCore.sln                     # 解決方案檔案
├── GameCore.Web/                    # 主要 Web 應用程式
│   ├── Controllers/                 # MVC 控制器
│   ├── Views/                       # Razor 視圖
│   ├── wwwroot/                     # 靜態資源
│   ├── Models/                      # ViewModel
│   ├── Services/                    # 應用服務
│   └── Program.cs                   # 應用程式入口
├── GameCore.Core/                   # 核心業務邏輯
│   ├── Entities/                    # 實體類別
│   ├── Interfaces/                  # 介面定義
│   ├── Services/                    # 業務服務
│   └── DTOs/                        # 資料傳輸物件
├── GameCore.Infrastructure/         # 基礎設施層
│   ├── Data/                        # 資料存取
│   ├── Repositories/                # 資料庫存取
│   └── Services/                    # 外部服務
├── GameCore.Tests/                  # 測試專案
│   ├── UnitTests/                   # 單元測試
│   ├── IntegrationTests/            # 整合測試
│   └── EndToEndTests/               # 端對端測試
├── Database/                        # 資料庫腳本
│   ├── 01-CreateTables.sql          # 建立資料表
│   └── 02-InsertMockData.sql        # 插入假資料
└── Documentation/                   # 專案文件
    ├── API文件.md
    ├── 使用者手冊.pdf
    └── 系統架構圖.png
```

## 🚀 快速開始

### 前置需求

1. **.NET 8.0 SDK** - [下載連結](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **SQL Server** 或 **SQL Server Express** - [下載連結](https://www.microsoft.com/sql-server/sql-server-downloads)
3. **Visual Studio 2022** 或 **VS Code** (可選)

### 安裝步驟

1. **複製專案**
   ```bash
   git clone https://github.com/your-username/GameCore.git
   cd GameCore
   ```

2. **還原套件**
   ```bash
   dotnet restore
   ```

3. **設定資料庫連接字串**
   
   編輯 `GameCore.Web/appsettings.json`：
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GameCore;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

4. **建立資料庫**
   ```bash
   # 方法 1: 使用 EF Core 遷移
   dotnet ef database update --project GameCore.Infrastructure --startup-project GameCore.Web
   
   # 方法 2: 直接執行 SQL 腳本
   # 在 SQL Server Management Studio 中執行:
   # - Database/01-CreateTables.sql
   # - Database/02-InsertMockData.sql
   ```

5. **啟動應用程式**
   ```bash
   dotnet run --project GameCore.Web
   ```

6. **開啟瀏覽器**
   
   訪問 `https://localhost:5001` 或 `http://localhost:5000`

### 🎯 測試帳號

建立完成後，您可以使用以下測試帳號：

**一般使用者:**
- 帳號: `gamer1` / 密碼: `password123`
- 帳號: `player2` / 密碼: `password123`

**管理員:**
- 帳號: `admin` / 密碼: `admin123`
- 帳號: `wenjieyang` / 密碼: `password123`

## 🎮 主要功能

### 1. 寵物養成系統

#### 🐾 虛擬史萊姆
- **五維屬性**: 飢餓、心情、體力、清潔、健康 (0-100)
- **互動系統**: 餵食 (+10 飢餓)、洗澡 (+10 清潔)、玩耍 (+10 心情)、休息 (+10 體力)
- **外觀自訂**: 膚色、背景色 (消耗 2000 點數)
- **升級系統**: 複雜的經驗值計算公式，最高 250 級

#### 📅 每日簽到
- **基礎獎勵**:
  - 平日 (一~五): +20 點數
  - 假日 (六、日): +30 點數 + 200 經驗
- **特殊獎勵**:
  - 連續 7 天: 額外 +40 點數 + 300 經驗
  - 當月全勤: +200 點數 + 2000 經驗

#### 🎯 小冒險遊戲
- **每日限制**: 最多 3 次
- **關卡系統**: 難度遞增，怪物數量與速度提升
- **屬性影響**: 遊戲結果會影響寵物的心情和其他屬性

### 2. 商城系統

#### 🏪 官方商城 (B2C)
- **商品類型**: 遊戲軟體、硬體周邊
- **完整購物流程**: 瀏覽 → 加入購物車 → 結帳 → 付款 → 出貨
- **排行榜系統**: 日/月/季/年度銷售排行

#### 🤝 玩家自由市場 (C2C)
- **安全交易**: 買賣雙方確認制度
- **即時聊天**: 交易過程中的溝通功能
- **平台抽成**: 自動計算和分配

### 3. 社群系統

#### 💬 論壇討論
- **版面系統**: 每個遊戲有專屬討論版
- **討論串**: 支援二層回覆結構
- **互動功能**: 按讚、收藏、分享

#### 📱 即時通訊
- **私人訊息**: 一對一聊天
- **群組聊天**: 多人群組、管理員功能
- **客服系統**: 管理員客服支援

### 4. 熱度分析系統

#### 📈 數據收集
- **多平台整合**: Steam、Twitch、YouTube、Reddit 等
- **指標管理**: 同時在線人數、論壇貼文、觀看數等
- **數據品質**: 真實/估算/種子數據標記

#### 🏆 排行榜系統
- **即時榜單**: 日榜、週榜自動更新
- **歷史快照**: 保存排行榜歷史數據
- **多維度排名**: 支援不同指標的排名

## 🔧 開發指南

### 資料庫遷移

```bash
# 新增遷移
dotnet ef migrations add InitialCreate --project GameCore.Infrastructure --startup-project GameCore.Web

# 更新資料庫
dotnet ef database update --project GameCore.Infrastructure --startup-project GameCore.Web

# 刪除資料庫
dotnet ef database drop --project GameCore.Infrastructure --startup-project GameCore.Web
```

### 執行測試

```bash
# 執行所有測試
dotnet test

# 執行特定測試專案
dotnet test GameCore.Tests

# 產生測試覆蓋率報告
dotnet test --collect:"XPlat Code Coverage"
```

### 建置發佈

```bash
# 建置專案
dotnet build --configuration Release

# 發佈專案
dotnet publish GameCore.Web --configuration Release --output ./publish
```

## 🎨 UI 設計特色

### 玻璃擬態風格 (Glassmorphism)
- **半透明效果**: 使用 `backdrop-filter` 和 `blur` 效果
- **漸層配色**: 支援多種主題色彩
- **深色模式**: 完整的深色主題支援
- **響應式設計**: 適配各種螢幕尺寸

### 動畫系統
- **寵物動畫**: Canvas 2D 像素風格史萊姆
- **互動回饋**: 按鈕點擊、狀態變化動畫
- **頁面轉場**: 流暢的路由切換效果

## 📊 資料統計

執行假資料腳本後，系統將包含：

- **👥 使用者**: 1,200+ 人
- **🐾 寵物**: 1,200+ 隻
- **📅 簽到記錄**: 30,000+ 筆
- **🎮 小遊戲記錄**: 5,000+ 筆
- **🎯 遊戲**: 50+ 款
- **💬 討論主題**: 2,000+ 個
- **📝 討論回覆**: 15,000+ 則
- **🛒 商品**: 100+ 項
- **💎 玩家市場商品**: 800+ 項
- **💌 聊天訊息**: 5,000+ 則
- **👍 互動 (讚)**: 10,000+ 次
- **⭐ 收藏**: 3,000+ 次

**總計超過 70,000+ 筆資料記錄！**

## 🔒 安全特性

1. **密碼安全**: BCrypt 雜湊加鹽處理
2. **輸入驗證**: 所有輸入經過嚴格驗證
3. **權限控制**: 基於角色的存取控制 (RBAC)
4. **SQL 注入防護**: EF Core 參數化查詢
5. **XSS 防護**: 輸出編碼和 CSP 標頭
6. **CSRF 防護**: Anti-forgery token
7. **檔案上傳安全**: 類型驗證和大小限制

## 🚀 部署選項

### 本地開發
```bash
dotnet run --project GameCore.Web --environment Development
```

### Docker 部署
```bash
docker build -t gamecore .
docker run -p 8080:80 gamecore
```

### Azure 雲端部署
1. 建立 Azure App Service
2. 設定 Azure SQL Database
3. 配置連接字串和環境變數
4. 部署應用程式

### IIS 部署
1. 發佈應用程式
2. 在 IIS 中建立網站
3. 設定應用程式集區
4. 配置 SSL 憑證

## 📋 API 文件

### 寵物系統 API

```http
GET /api/pet                    # 取得寵物狀態
POST /api/pet/interact          # 寵物互動
POST /api/pet/recolor           # 寵物換色
PUT /api/pet/name               # 更新寵物名稱
GET /api/pet/can-adventure      # 檢查是否可冒險
```

### 簽到系統 API

```http
POST /api/signin                # 執行每日簽到
GET /api/signin/status          # 取得簽到狀態
GET /api/signin/history         # 取得簽到歷史
GET /api/signin/monthly/{year}/{month}  # 月度統計
```

### 使用者系統 API

```http
GET /api/users/me               # 取得個人資料
PUT /api/users/me               # 更新個人資料
GET /api/wallet/balance         # 取得點數餘額
GET /api/wallet/ledger          # 取得點數明細
```

## 🧪 測試指南

### 單元測試
```bash
# 執行寵物系統測試
dotnet test --filter "Category=PetSystem"

# 執行簽到系統測試
dotnet test --filter "Category=SignInSystem"

# 執行所有單元測試
dotnet test --filter "Category=Unit"
```

### 整合測試
```bash
# 執行 API 測試
dotnet test --filter "Category=Integration"

# 執行資料庫測試
dotnet test --filter "Category=Database"
```

### 端對端測試
```bash
# 執行完整流程測試
dotnet test --filter "Category=E2E"
```

## 📈 效能優化

### 資料庫優化
- **索引策略**: 針對常用查詢建立複合索引
- **查詢優化**: 使用 EF Core 的最佳實踐
- **連接池**: 優化資料庫連接管理

### 快取策略
- **記憶體快取**: 熱門資料快取
- **分散式快取**: Redis 支援 (可選)
- **輸出快取**: 靜態內容快取

### 前端優化
- **圖片優化**: WebP 格式和壓縮
- **CSS/JS 壓縮**: 生產環境自動壓縮
- **CDN 整合**: 靜態資源分發

## 🔧 設定說明

### 環境變數
```bash
ASPNETCORE_ENVIRONMENT=Development
GAMECORE_DB_CONNECTION="your-connection-string"
GAMECORE_GOOGLE_CLIENT_ID="your-google-client-id"
GAMECORE_GOOGLE_CLIENT_SECRET="your-google-client-secret"
GAMECORE_FACEBOOK_APP_ID="your-facebook-app-id"
GAMECORE_FACEBOOK_APP_SECRET="your-facebook-app-secret"
```

### OAuth 設定

#### Google OAuth
1. 前往 [Google Cloud Console](https://console.cloud.google.com/)
2. 建立新專案或選擇現有專案
3. 啟用 Google+ API
4. 建立 OAuth 2.0 憑證
5. 設定授權重新導向 URI: `https://yourdomain.com/signin-google`

#### Facebook OAuth
1. 前往 [Facebook Developers](https://developers.facebook.com/)
2. 建立新應用程式
3. 新增 Facebook 登入產品
4. 設定有效的 OAuth 重新導向 URI: `https://yourdomain.com/signin-facebook`

## 🐛 疑難排解

### 常見問題

#### 1. 資料庫連接失敗
```
確認 SQL Server 服務是否啟動
檢查連接字串是否正確
確認資料庫是否已建立
```

#### 2. OAuth 登入失敗
```
檢查 OAuth 應用程式設定
確認 Client ID 和 Secret 是否正確
驗證回調 URL 設定
```

#### 3. 寵物動畫不顯示
```
確認瀏覽器支援 Canvas 2D
檢查 JavaScript 是否有錯誤
確認 API 回應是否正常
```

### 偵錯模式
```bash
# 啟用詳細日誌
dotnet run --project GameCore.Web --environment Development --verbosity diagnostic

# 檢視 EF Core 查詢
# 在 appsettings.Development.json 中設定:
"Microsoft.EntityFrameworkCore": "Debug"
```

## 📚 相關資源

- [ASP.NET Core 文件](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core 文件](https://docs.microsoft.com/ef/core/)
- [Vue.js 文件](https://vuejs.org/)
- [Bootstrap 文件](https://getbootstrap.com/)
- [SignalR 文件](https://docs.microsoft.com/aspnet/core/signalr/)

## 🤝 貢獻指南

1. Fork 專案
2. 建立功能分支 (`git checkout -b feature/amazing-feature`)
3. 提交變更 (`git commit -m 'Add amazing feature'`)
4. 推送到分支 (`git push origin feature/amazing-feature`)
5. 開啟 Pull Request

## 📄 授權

本專案採用 MIT 授權條款 - 詳見 [LICENSE](LICENSE) 檔案

## 👥 開發團隊

- **專案負責人**: GameCore Team
- **熱度系統**: 溫傑揚
- **寵物系統**: 鐘群能  
- **商城系統**: 房立堯、成博儒
- **前端設計**: UI/UX Team

## 📞 聯絡資訊

- **Email**: contact@gamecore.com
- **GitHub**: https://github.com/gamecore/gamecore
- **官方網站**: https://gamecore.com

---

**🎉 感謝使用 GameCore 遊戲社群平台！**

如果您覺得這個專案有幫助，請給我們一個 ⭐ Star！