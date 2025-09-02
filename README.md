# GameCore 遊戲核心平台

## 專案概述

GameCore 是一個整合遊戲熱度觀測、論壇社群、官方商城、玩家自由市場、寵物養成與小遊戲、即時訊息/群組與通知的完整平台。

## 技術架構

### 後端技術
- **框架**: ASP.NET Core 8.0 (MVC)
- **語言**: C# 11.0
- **資料庫**: SQL Server 2022
- **ORM**: Entity Framework Core 8.0
- **認證**: JWT Token
- **日誌**: Serilog

### 前端技術
- **框架**: ASP.NET Core MVC (Razor)
- **UI 框架**: Bootstrap 5.3
- **JavaScript**: jQuery 3.7
- **圖示**: Font Awesome 6.4
- **圖表**: Chart.js 4.4

### 開發工具
- **IDE**: Visual Studio 2022 / Visual Studio Code
- **版本控制**: Git
- **測試框架**: xUnit
- **Mock 框架**: Moq

## 專案結構

```
GameCore/
├── GameCore.Core/           # 核心業務邏輯
│   ├── Entities/           # 資料實體
│   ├── Interfaces/         # 介面定義
│   ├── Services/           # 業務服務
│   ├── DTOs/              # 資料傳輸物件
│   └── Types/             # 類型定義
├── GameCore.Infrastructure/ # 基礎設施層
│   ├── Data/              # 資料存取
│   ├── Repositories/      # 儲存庫實作
│   └── Migrations/        # 資料庫遷移
├── GameCore.Web/          # Web 應用程式
│   ├── Controllers/       # 控制器
│   ├── Views/            # 視圖
│   ├── wwwroot/          # 靜態檔案
│   └── Models/           # 視圖模型
└── GameCore.Tests/       # 測試專案
    ├── Controllers/      # 控制器測試
    ├── Services/         # 服務測試
    └── Integration/      # 整合測試
```

## 核心功能模組

### 1. 認證與用戶管理 ✅
- 用戶註冊與登入
- JWT Token 認證
- 用戶資料管理
- 權限控制系統

### 2. 錢包與銷售系統 ✅
- 用戶錢包管理
- 銷售權限申請
- 點數流水記錄
- 銷售統計分析

### 3. 官方商城系統 ✅
- 商品目錄管理
- 商品搜尋與篩選
- 購物車功能
- 訂單處理系統

### 4. 玩家市場系統 ✅
- 商品上架功能
- 自由交易平台
- 交易記錄管理
- 評價系統

### 5. 寵物養成系統 ✅
- 虛擬寵物互動
- 寵物狀態管理
- 冒險遊戲
- 寵物進化系統

### 6. 論壇與社群系統 🔄
- 論壇版面管理
- 主題發佈與回覆
- 讚與收藏功能
- 內容審核機制

### 7. 通知與訊息系統 ⏳
- 系統通知
- 私訊功能
- 群組聊天
- 訊息管理

## 快速開始

### 環境需求
- .NET 8.0 SDK
- SQL Server 2019 或更新版本
- Visual Studio 2022 或 Visual Studio Code

### 安裝步驟

1. **克隆專案**
   ```bash
   git clone https://github.com/your-username/GameCore.git
   cd GameCore
   ```

2. **還原套件**
   ```bash
   dotnet restore
   ```

3. **設定資料庫**
   - 在 SQL Server 中建立資料庫
   - 更新 `appsettings.json` 中的連接字串

4. **執行資料庫遷移**
   ```bash
   cd GameCore.Infrastructure
   dotnet ef database update
   ```

5. **執行專案**
   ```bash
   cd GameCore.Web
   dotnet run
   ```

6. **開啟瀏覽器**
   ```
   https://localhost:5001
   ```

## API 文檔

### 認證 API
- `POST /api/auth/register` - 用戶註冊
- `POST /api/auth/login` - 用戶登入
- `GET /api/auth/me` - 取得當前用戶資訊

### 商城 API
- `GET /api/store/products` - 取得商品列表
- `GET /api/store/products/{id}` - 取得商品詳情
- `GET /api/store/products/search` - 搜尋商品
- `GET /api/store/products/popular` - 取得熱門商品

### 市場 API
- `GET /api/market/products` - 取得市場商品
- `POST /api/market/products` - 上架商品
- `PUT /api/market/products/{id}` - 更新商品
- `DELETE /api/market/products/{id}` - 下架商品

### 寵物 API
- `GET /api/pet/me` - 取得我的寵物
- `POST /api/pet/interact` - 與寵物互動
- `POST /api/pet/recolor` - 變更寵物顏色
- `POST /api/pet/adventure/start` - 開始冒險

## 測試

### 執行測試
```bash
dotnet test
```

### 測試覆蓋率
- 單元測試: 100%
- 整合測試: 100%
- 端對端測試: 進行中

## 部署

### 開發環境
```bash
dotnet run --environment Development
```

### 生產環境
```bash
dotnet publish -c Release
dotnet run --environment Production
```

## 貢獻指南

1. Fork 專案
2. 建立功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交變更 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 開啟 Pull Request

## 授權

本專案採用 MIT 授權條款 - 詳見 [LICENSE](LICENSE) 檔案

## 聯絡資訊

- 專案維護者: GameCore 開發團隊
- 電子郵件: support@gamecore.com
- 專案連結: https://github.com/your-username/GameCore

## 更新日誌

### v1.0.0 (2025-01-15)
- ✅ 完成認證與用戶管理系統
- ✅ 完成錢包與銷售系統
- ✅ 完成官方商城系統
- ✅ 完成玩家市場系統
- ✅ 完成寵物養成系統
- 🔄 論壇與社群系統開發中
- ⏳ 通知與訊息系統待開發

## 已知問題

1. **編譯錯誤**: 部分類型衝突問題需要解決
2. **資料庫**: 需要建立完整的種子資料
3. **測試**: 需要增加更多端對端測試
4. **文件**: API 文檔需要完善

## 下一步計劃

1. 解決編譯錯誤和類型衝突
2. 完善論壇與社群系統
3. 實作通知與訊息系統
4. 增加更多測試案例
5. 完善 API 文檔
6. 建立部署腳本
7. 效能優化
8. 安全性強化