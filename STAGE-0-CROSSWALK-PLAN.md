# Stage 0 — Crosswalk Plan & Gap Analysis

**專案**: GameCore 整合平台  
**日期**: 2025年1月16日  
**階段**: 0 - 跨步計劃與差距分析  

---

## 📋 Gap Table

| 模組 | 規格需求 | 當前狀態 | 缺失項目 | 需要修改的檔案 |
|------|----------|----------|----------|----------------|
| **Auth/Users** | 註冊/登入/個資/權限/錢包 | ⚠️ 70% 完成 | 第三方登入、完整權限檢查、錢包流水 | `UserController.cs`, `AuthService.cs`, `UserService.cs` |
| **Wallet/Sales** | 點數系統、銷售權限、銷售錢包 | ❌ 0% 完成 | 點數流水、銷售申請流程、錢包管理 | `WalletController.cs`, `SalesController.cs`, `WalletService.cs` |
| **Official Store** | 商品管理、訂單流程、排行榜 | ⚠️ 60% 完成 | 完整訂單狀態機、支付回調、排行榜API | `StoreController.cs`, `OrderService.cs`, `RankingService.cs` |
| **Player Market** | C2C交易、交易頁、平台抽成 | ❌ 0% 完成 | 交易流程、雙方確認、抽成計算 | `PlayerMarketController.cs`, `TradeService.cs`, `MarketService.cs` |
| **Forums/Threads/Posts/Reactions/Bookmarks** | 論壇系統、互動功能 | ⚠️ 40% 完成 | 完整論壇CRUD、互動功能、狀態管理 | `ForumController.cs`, `ThreadController.cs`, `ReactionService.cs` |
| **Popularity/Leaderboards/Insights** | 熱度計算、榜單快照、洞察貼文 | ❌ 0% 完成 | 熱度指數計算、榜單生成、洞察系統 | `PopularityController.cs`, `LeaderboardService.cs`, `InsightService.cs` |
| **Social/Notifications/DM/Groups/Blocks** | 通知、私訊、群組、封鎖 | ⚠️ 30% 完成 | 通知投遞、群組管理、封鎖機制 | `NotificationController.cs`, `ChatController.cs`, `GroupService.cs` |
| **Daily Sign-In** | 每日簽到、連續獎勵、點數回饋 | ⚠️ 80% 完成 | 獎勵規則、連續計算、全勤獎勵 | `SignInController.cs`, `SignInService.cs` |
| **Virtual Pet (Slime)** | 史萊姆養成、互動、屬性系統 | ⚠️ 90% 完成 | 完整動畫、音效、狀態機 | `PetController.cs`, `PetService.cs`, 前端動畫 |
| **Mini-Game (Adventure)** | 冒險遊戲、關卡系統、獎勵 | ⚠️ 85% 完成 | 完整遊戲邏輯、獎勵計算 | `MiniGameController.cs`, `MiniGameService.cs` |
| **Admin Backoffice** | 管理員介面、權限管理、數據統計 | ❌ 0% 完成 | 管理介面、權限控制、統計報表 | `ManagerController.cs`, `AdminService.cs`, 管理頁面 |

---

## 🎯 Ordered Stage Plan

### Stage 1: Auth/Users + Wallet/Sales (基礎用戶系統)
**目標**: 完成用戶認證、權限管理、錢包系統  
**交付物**: 
- 完整的用戶註冊/登入系統
- 第三方登入整合 (Google, Facebook, Discord)
- 用戶權限管理 (User_Rights)
- 錢包系統 (User_wallet)
- 銷售權限申請流程
- 基礎測試和種子資料

**檔案**: 
- `Controllers/AuthController.cs`
- `Controllers/UserController.cs` 
- `Controllers/WalletController.cs`
- `Controllers/SalesController.cs`
- `Services/AuthService.cs`
- `Services/UserService.cs`
- `Services/WalletService.cs`
- `Services/SalesService.cs`
- `Views/Auth/`, `Views/User/`, `Views/Wallet/`
- 測試專案

### Stage 2: Official Store (官方商城)
**目標**: 完整的B2C商城系統  
**交付物**:
- 商品管理 (ProductInfo, GameProductDetails, OtherProductDetails)
- 訂單流程 (OrderInfo, OrderItems)
- 狀態機實作 (Created → ToShip → Shipped → Completed)
- 排行榜系統 (Official_Store_Ranking)
- 供應商管理
- 完整測試和種子資料

**檔案**:
- `Controllers/StoreController.cs`
- `Controllers/ProductController.cs`
- `Controllers/OrderController.cs`
- `Services/StoreService.cs`
- `Services/ProductService.cs`
- `Services/OrderService.cs`
- `Views/Store/`, `Views/Product/`, `Views/Order/`

### Stage 3: Player Market (自由市場)
**目標**: C2C交易平台  
**交付物**:
- 商品上架/下架
- 訂單流程 (PlayerMarketOrderInfo)
- 交易頁面 (PlayerMarketOrderTradepage)
- 雙方確認機制
- 平台抽成計算
- 聊天系統 (PlayerMarketTradeMsg)
- 排行榜 (PlayerMarketRanking)

**檔案**:
- `Controllers/PlayerMarketController.cs`
- `Controllers/TradeController.cs`
- `Services/PlayerMarketService.cs`
- `Services/TradeService.cs`
- `Views/PlayerMarket/`, `Views/Trade/`

### Stage 4: Forums + Social + Notifications
**目標**: 完整的社群系統  
**交付物**:
- 論壇系統 (forums, threads, thread_posts)
- 互動功能 (reactions, bookmarks)
- 通知系統 (Notifications, Notification_Recipients)
- 私訊系統 (Chat_Message)
- 群組系統 (Groups, Group_Member, Group_Chat, Group_Block)

**檔案**:
- `Controllers/ForumController.cs`
- `Controllers/ThreadController.cs`
- `Controllers/NotificationController.cs`
- `Controllers/ChatController.cs`
- `Controllers/GroupController.cs`
- `Services/ForumService.cs`
- `Services/NotificationService.cs`
- `Services/ChatService.cs`
- `Services/GroupService.cs`
- `Views/Forum/`, `Views/Chat/`, `Views/Group/`

### Stage 5: Popularity + Leaderboards + Insights
**目標**: 遊戲熱度與數據洞察  
**交付物**:
- 熱度數據管理 (games, metric_sources, metrics)
- 每日指標計算 (game_metric_daily)
- 熱度指數計算 (popularity_index_daily)
- 排行榜快照 (leaderboard_snapshots)
- 洞察貼文系統 (posts, post_metric_snapshot)
- 數據分析API

**檔案**:
- `Controllers/PopularityController.cs`
- `Controllers/LeaderboardController.cs`
- `Controllers/InsightController.cs`
- `Services/PopularityService.cs`
- `Services/LeaderboardService.cs`
- `Services/InsightService.cs`
- `Views/Popularity/`, `Views/Leaderboard/`

### Stage 6: Daily Sign-In + Virtual Pet + Mini-Game
**目標**: 用戶留存與互動系統  
**交付物**:
- 每日簽到系統 (UserSignInStats)
- 完整的史萊姆養成系統 (Pet)
- 冒險遊戲系統 (MiniGame)
- 點數獎勵機制
- 屬性計算與衰減
- 動畫與音效系統

**檔案**:
- 完善現有的 `SignInController.cs`, `PetController.cs`, `MiniGameController.cs`
- 前端動畫與互動
- 音效系統整合

### Stage 7: Admin Backoffice
**目標**: 完整的管理後台  
**交付物**:
- 管理員登入與權限控制
- 用戶管理介面
- 商品審核系統
- 訂單管理
- 內容審核
- 數據統計報表
- 系統設定

**檔案**:
- `Controllers/ManagerController.cs`
- `Controllers/AdminController.cs`
- `Services/ManagerService.cs`
- `Services/AdminService.cs`
- `Views/Manager/`, `Views/Admin/`
- 管理後台佈局

### Stage 8: Integration + Testing + Documentation
**目標**: 系統整合與品質保證  
**交付物**:
- 端到端測試
- 效能測試
- 安全測試
- 完整API文檔
- 部署指南
- 用戶手冊
- 專案報告書與簡報

---

## 🔧 技術實作重點

### 架構模式
- **三層式架構**: Web → Business Logic → Data Access
- **Repository Pattern**: 資料存取抽象化
- **Unit of Work**: 交易管理
- **Service Layer**: 業務邏輯封裝

### 資料庫設計
- **Entity Framework Core**: ORM框架
- **Code First**: 資料庫設計優先
- **Migration**: 版本控制
- **Seed Data**: 大量假資料 (1000+ 筆)

### 前端技術
- **ASP.NET MVC**: 後端框架
- **Razor Pages**: 視圖引擎
- **Bootstrap 5**: UI框架
- **jQuery**: JavaScript庫
- **Vue.js**: 前端框架 (可選)
- **Tailwind CSS**: 實用優先CSS (可選)

### 認證與授權
- **JWT Token**: 無狀態認證
- **OAuth 2.0**: 第三方登入
- **Role-Based Access Control**: 角色權限控制
- **Claims**: 細粒度權限

### 測試策略
- **單元測試**: xUnit + Moq
- **整合測試**: TestServer + InMemory Database
- **端到端測試**: Playwright
- **效能測試**: BenchmarkDotNet

---

## 📊 假資料策略

### 資料量目標
- **用戶資料**: 1000+ 筆
- **商品資料**: 500+ 筆
- **訂單資料**: 2000+ 筆
- **論壇內容**: 3000+ 筆
- **遊戲數據**: 100+ 遊戲，每日指標

### 資料真實性
- 參考真實遊戲網站
- 使用真實遊戲名稱
- 模擬真實用戶行為
- 合理的價格與數量

### 資料生成工具
- Bogus 假資料生成器
- 自定義假資料腳本
- 資料庫種子腳本

---

## 🚀 部署與CI/CD

### GitHub部署
- GitHub Actions 自動化
- Docker 容器化
- 環境變數管理
- 自動測試與部署

### 監控與維護
- 健康檢查端點
- 日誌記錄
- 效能監控
- 錯誤追蹤

---

## 📅 時程規劃

| 階段 | 預估時間 | 關鍵里程碑 | 交付物 |
|------|----------|------------|--------|
| Stage 1 | 2-3 天 | 用戶系統完成 | 認證、權限、錢包 |
| Stage 2 | 2-3 天 | 商城系統完成 | 商品、訂單、排行榜 |
| Stage 3 | 2-3 天 | 自由市場完成 | C2C交易、聊天 |
| Stage 4 | 2-3 天 | 社群系統完成 | 論壇、通知、群組 |
| Stage 5 | 2-3 天 | 熱度系統完成 | 數據、榜單、洞察 |
| Stage 6 | 1-2 天 | 互動系統完成 | 簽到、寵物、遊戲 |
| Stage 7 | 2-3 天 | 管理後台完成 | 管理介面、權限 |
| Stage 8 | 2-3 天 | 整合測試完成 | 測試、文檔、部署 |

**總預估時間**: 15-25 天  
**目標**: 100% 功能完成，可立即部署使用

---

## ✅ 下一步行動

1. **立即開始 Stage 1**: Auth/Users + Wallet/Sales
2. **建立基礎架構**: 完善現有的服務層
3. **創建假資料**: 大量種子資料
4. **前端頁面**: 完整的用戶介面
5. **測試覆蓋**: 單元測試與整合測試

---

**計劃制定者**: GameCore 專案實施助手  
**審核狀態**: 待審核  
**執行優先級**: 高