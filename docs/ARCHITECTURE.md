# GameCore 架構藍圖

## 概述

GameCore 採用 Clean Architecture（清潔架構）設計模式，確保系統的可維護性、可測試性和可擴展性。本文件描述了整體架構佈局、模組邊界和依賴關係。

## 解決方案佈局

```
GameCore/
├── GameCore.Domain/           # 領域層 - 核心業務邏輯
├── GameCore.Application/       # 應用層 - 用例和服務
├── GameCore.Infrastructure/    # 基礎設施層 - 數據存取和外部服務
├── GameCore.Web/              # 表現層 - API 和 MVC
└── GameCore.Tests/            # 測試層 - 單元測試和整合測試
```

## 層級職責與依賴規則

### 1. GameCore.Domain（領域層）
**職責**：
- 定義核心業務實體（Entities）
- 定義值物件（Value Objects）
- 定義枚舉（Enums）
- 定義領域事件（Domain Events）
- 定義 Repository 接口

**依賴規則**：
- ✅ 無外部依賴
- ✅ 不依賴任何其他層
- ✅ 純粹的業務邏輯

**包含模組**：
```
Domain/
├── Entities/           # 業務實體
├── ValueObjects/       # 值物件
├── Enums/             # 枚舉定義
├── Events/             # 領域事件
├── Interfaces/         # Repository 接口
└── Exceptions/         # 領域異常
```

### 2. GameCore.Application（應用層）
**職責**：
- 定義 DTOs（數據傳輸物件）
- 實現 Result<T> 和 OperationResult 模式
- 定義 UseCase/Service 接口
- 實現驗證器（Validators）
- 處理業務邏輯協調

**依賴規則**：
- ✅ 依賴 Domain 層
- ❌ 不依賴 Infrastructure 層
- ❌ 不依賴 Web 層

**包含模組**：
```
Application/
├── DTOs/               # 數據傳輸物件
├── Common/             # 通用模式（Result<T>, OperationResult）
├── Services/           # 服務接口
├── UseCases/           # 用例實現
├── Validators/         # 驗證器
└── Adapters/           # 適配器（兼容性）
```

### 3. GameCore.Infrastructure（基礎設施層）
**職責**：
- 實現 EF Core DbContext
- 實現 Repository 類別
- 處理數據庫映射
- 實現外部服務整合
- 處理快取、日誌等橫切關注點

**依賴規則**：
- ✅ 依賴 Application 層
- ✅ 依賴 Domain 層
- ❌ 不依賴 Web 層

**包含模組**：
```
Infrastructure/
├── Data/               # 數據存取
│   ├── GameCoreDbContext.cs
│   ├── Configurations/ # 實體配置
│   └── SeedData/      # 種子數據
├── Repositories/       # Repository 實現
├── Services/           # 外部服務實現
├── Caching/            # 快取實現
├── Logging/            # 日誌實現
└── External/           # 外部服務整合
```

### 4. GameCore.Web（表現層）
**職責**：
- 實現 API 控制器
- 實現 MVC 控制器
- 處理 HTTP 請求和回應
- 實現身份驗證和授權
- 處理路由配置

**依賴規則**：
- ✅ 依賴 Application 層
- ❌ 不直接依賴 Domain 層
- ❌ 不直接依賴 Infrastructure 層

**包含模組**：
```
Web/
├── Controllers/        # API 控制器
├── Models/             # MVC 模型
├── Views/              # MVC 視圖
├── Middleware/         # 中間件
├── Filters/            # 過濾器
└── Configuration/      # 配置
```

### 5. GameCore.Tests（測試層）
**職責**：
- 單元測試
- 整合測試
- API 冒煙測試
- 測試工具和輔助類別

**依賴規則**：
- ✅ 可以依賴所有層
- ✅ 測試所有層的組件

## 模組地圖

### 核心業務模組

#### 1. Users/Auth（用戶/認證）
- **Domain**: User, UserRole, UserRights, UserIntroduce
- **Application**: AuthService, UserService
- **Infrastructure**: UserRepository, AuthRepository
- **Web**: AuthController, UserController

#### 2. Wallet（錢包）
- **Domain**: UserWallet, WalletTransaction
- **Application**: WalletService
- **Infrastructure**: WalletRepository
- **Web**: WalletController

#### 3. Chat（聊天）
- **Domain**: ChatMessage, ChatRoom, PrivateChat
- **Application**: ChatService
- **Infrastructure**: ChatRepository
- **Web**: ChatController

#### 4. Pet（寵物）
- **Domain**: Pet, PetStatus
- **Application**: PetService
- **Infrastructure**: PetRepository
- **Web**: PetController

#### 5. PlayerMarket（玩家市場）
- **Domain**: MarketItem, MarketTransaction, PlayerMarketOrder
- **Application**: PlayerMarketService
- **Infrastructure**: MarketRepository
- **Web**: MarketController

#### 6. MiniGame（小遊戲）
- **Domain**: MiniGame, MiniGameRecord, GameSettings
- **Application**: MiniGameService
- **Infrastructure**: MiniGameRepository
- **Web**: MiniGameController

#### 7. Store（商店）
- **Domain**: StoreProduct, StoreOrder, StoreCart
- **Application**: StoreService
- **Infrastructure**: StoreRepository
- **Web**: StoreController

#### 8. Notification（通知）
- **Domain**: Notification, NotificationSource, NotificationAction
- **Application**: NotificationService
- **Infrastructure**: NotificationRepository
- **Web**: NotificationController

#### 9. Sales/Admin（銷售/管理）
- **Domain**: ManagerData, ManagerRole, SalesProfile
- **Application**: SalesService, ManagerService
- **Infrastructure**: ManagerRepository, SalesRepository
- **Web**: SalesController, ManagerController

### 橫切關注點

#### 1. Result Pattern（結果模式）
- **位置**: `GameCore.Application.Common`
- **用途**: 統一錯誤處理和回應格式
- **實現**: `Result<T>`, `OperationResult`

#### 2. Validation（驗證）
- **位置**: `GameCore.Application.Validators`
- **用途**: 輸入驗證和業務規則驗證
- **實現**: FluentValidation

#### 3. Logging（日誌）
- **位置**: `GameCore.Infrastructure.Logging`
- **用途**: 系統日誌記錄
- **實現**: Serilog

#### 4. Caching（快取）
- **位置**: `GameCore.Infrastructure.Caching`
- **用途**: 性能優化
- **實現**: Redis/Memory Cache

#### 5. Transactions（事務）
- **位置**: `GameCore.Infrastructure.Data`
- **用途**: 數據一致性
- **實現**: EF Core Unit of Work

#### 6. Pagination（分頁）
- **位置**: `GameCore.Application.Common`
- **用途**: 大量數據處理
- **實現**: `PagedResult<T>`

#### 7. Error Handling（錯誤處理）
- **位置**: `GameCore.Application.Common`
- **用途**: 統一錯誤處理
- **實現**: Global Exception Handler

## 依賴注入配置

### 服務註冊順序
1. **Domain Services** - 註冊領域服務
2. **Application Services** - 註冊應用服務
3. **Infrastructure Services** - 註冊基礎設施服務
4. **Web Services** - 註冊 Web 服務

### 生命週期管理
- **Singleton**: 配置服務、快取服務
- **Scoped**: DbContext、Repository
- **Transient**: 業務服務、控制器

## 數據庫設計原則

### 1. 實體設計
- 使用 EF Core Code First
- 保持現有數據庫 schema 不變
- 通過配置映射處理差異

### 2. 關係映射
- 使用 Fluent API 配置
- 避免循環依賴
- 優化查詢性能

### 3. 遷移策略
- 保持向後兼容性
- 漸進式遷移
- 數據完整性檢查

## 安全考量

### 1. 身份驗證
- JWT Token 認證
- 角色基礎授權
- API 金鑰管理

### 2. 數據保護
- 敏感數據加密
- SQL Injection 防護
- XSS 防護

### 3. 審計日誌
- 操作日誌記錄
- 安全事件追蹤
- 合規性報告

## 性能優化

### 1. 數據庫優化
- 索引優化
- 查詢優化
- 連接池管理

### 2. 快取策略
- 多層快取
- 快取失效策略
- 快取一致性

### 3. 非同步處理
- 非同步 I/O
- 背景任務
- 消息佇列

## 部署架構

### 1. 環境配置
- **Development**: 本地開發環境
- **Staging**: 測試環境
- **Production**: 生產環境

### 2. 容器化
- Docker 容器
- Docker Compose
- Kubernetes 部署

### 3. 監控
- 健康檢查端點
- 性能監控
- 錯誤追蹤

## 開發流程

### 1. 代碼組織
- 功能模組化
- 清晰的命名規範
- 一致的代碼風格

### 2. 測試策略
- 單元測試覆蓋率 > 80%
- 整合測試
- 端到端測試

### 3. 代碼審查
- Pull Request 流程
- 代碼品質檢查
- 安全掃描

## 未來擴展

### 1. 微服務架構
- 服務拆分策略
- API Gateway
- 服務發現

### 2. 事件驅動架構
- 領域事件
- 事件總線
- 事件溯源

### 3. 雲端原生
- 雲端服務整合
- 無伺服器架構
- 自動擴展

---

*本架構藍圖將隨著專案發展持續更新和完善。* 