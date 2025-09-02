# GameCore 實作計劃

## 概述

本文檔定義了 GameCore 專案架構重設後的實作路線圖，包括各模組的實作順序、需要完成的具體內容，以及每個階段的交付物。

## 實作順序

### 階段 1: 基礎架構與認證 (優先級：高)
**模組**: Auth → User → Wallet

**目標**: 建立穩固的基礎架構，實現用戶認證和基本管理功能

#### 1.1 認證模組 (Auth)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `LoginRequest`, `LoginResponse`, `RegisterRequest`, `RegisterResponse`
  - `RefreshTokenRequest`, `ChangePasswordRequest`, `ForgotPasswordRequest`
- [ ] **實體**: `AuthToken`, `UserRole`, `Permission`
- [ ] **倉庫**: `IAuthRepository` 實現
- [ ] **服務**: `AuthService` 完整實現
- [ ] **驗證器**: 所有請求 DTO 的驗證規則
- [ ] **控制器**: `AuthController` 完整端點

**依賴關係**: 無外部依賴

#### 1.2 用戶模組 (User)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `CreateUserRequest`, `UpdateUserRequest`, `UserResponse`
  - `UserQueryParameters`, `UserStats`
- [ ] **實體**: `User`, `UserProfile`, `UserSettings`
- [ ] **倉庫**: `IUserRepository` 完整實現
- [ ] **服務**: `UserService` 完整實現
- [ ] **驗證器**: 用戶相關 DTO 的驗證規則
- [ ] **控制器**: `UserController` 完整端點

**依賴關係**: 依賴 Auth 模組

#### 1.3 錢包模組 (Wallet)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `WalletResponse`, `TransactionRequest`, `TransactionResponse`
  - `WalletStats`, `TransferRequest`
- [ ] **實體**: `UserWallet`, `Transaction`, `WalletType`
- [ ] **倉庫**: `IWalletRepository` 完整實現
- [ ] **服務**: `WalletService` 完整實現
- [ ] **驗證器**: 錢包相關 DTO 的驗證規則
- [ ] **控制器**: `WalletController` 完整端點

**依賴關係**: 依賴 User 模組

### 階段 2: 社交功能 (優先級：中)
**模組**: Chat → Pet → Forum

**目標**: 實現用戶互動和社交功能

#### 2.1 聊天模組 (Chat)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `CreateChatRoomRequest`, `ChatRoomResponse`, `SendMessageRequest`
  - `ChatMessageResponse`, `ChatRoomMemberResponse`
- [ ] **實體**: `ChatRoom`, `ChatMessage`, `ChatRoomMember`
- [ ] **倉庫**: `IChatRepository` 完整實現
- [ ] **服務**: `ChatService` 完整實現
- [ ] **驗證器**: 聊天相關 DTO 的驗證規則
- [ ] **控制器**: `ChatController` 完整端點

**依賴關係**: 依賴 User 模組

#### 2.2 寵物模組 (Pet)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `CreatePetRequest`, `UpdatePetRequest`, `PetResponse`
  - `PetStats`, `FeedPetRequest`
- [ ] **實體**: `Pet`, `PetType`, `PetSkill`, `PetStats`
- [ ] **倉庫**: `IPetRepository` 完整實現
- [ ] **服務**: `PetService` 完整實現
- [ ] **驗證器**: 寵物相關 DTO 的驗證規則
- [ ] **控制器**: `PetController` 完整端點

**依賴關係**: 依賴 User 模組

#### 2.3 論壇模組 (Forum)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `CreatePostRequest`, `UpdatePostRequest`, `PostResponse`
  - `CreateReplyRequest`, `ReplyResponse`, `ForumResponse`
- [ ] **實體**: `Forum`, `Post`, `PostReply`, `PostStatus`
- [ ] **倉庫**: `IForumRepository`, `IPostRepository`, `IPostReplyRepository` 完整實現
- [ ] **服務**: `ForumService`, `PostService` 完整實現
- [ ] **驗證器**: 論壇相關 DTO 的驗證規則
- [ ] **控制器**: `ForumController`, `PostController` 完整端點

**依賴關係**: 依賴 User 模組

### 階段 3: 遊戲與娛樂 (優先級：中)
**模組**: MiniGame → Game

**目標**: 實現小遊戲和遊戲系統

#### 3.1 小遊戲模組 (MiniGame)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `PlayGameRequest`, `GameResult`, `GameRecord`
  - `LeaderboardEntry`, `GameSettings`, `GameStatistics`
- [ ] **實體**: `MiniGame`, `MiniGameRecord`, `MiniGameSettings`
- [ ] **倉庫**: `IMiniGameRepository` 完整實現
- [ ] **服務**: `MiniGameService` 完整實現
- [ ] **驗證器**: 遊戲相關 DTO 的驗證規則
- [ ] **控制器**: `MiniGameController` 完整端點

**依賴關係**: 依賴 User 模組

#### 3.2 遊戲模組 (Game)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `GameRequest`, `GameResponse`, `GameSettings`
  - `GameStats`, `GameHistory`
- [ ] **實體**: `Game`, `GameRecord`, `GameSettings`
- [ ] **倉庫**: `IGameRepository` 完整實現
- [ ] **服務**: `GameService` 完整實現
- [ ] **驗證器**: 遊戲相關 DTO 的驗證規則
- [ ] **控制器**: `GameController` 完整端點

**依賴關係**: 依賴 User 模組

### 階段 4: 商業功能 (優先級：中)
**模組**: Store → PlayerMarket

**目標**: 實現商店和玩家市場功能

#### 4.1 商店模組 (Store)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `StoreProductResponse`, `CreateOrderRequest`, `OrderResponse`
  - `CartResponse`, `AddToCartRequest`, `StoreStats`
- [ ] **實體**: `Store`, `StoreProduct`, `StoreOrder`, `Cart`, `StoreCategory`
- [ ] **倉庫**: `IStoreRepository` 完整實現
- [ ] **服務**: `StoreService` 完整實現
- [ ] **驗證器**: 商店相關 DTO 的驗證規則
- [ ] **控制器**: `StoreController` 完整端點

**依賴關係**: 依賴 User 和 Wallet 模組

#### 4.2 玩家市場模組 (PlayerMarket)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `CreateMarketItemRequest`, `MarketItemResponse`, `PurchaseItemRequest`
  - `MarketTransactionResponse`, `MarketReviewRequest`, `MarketStats`
- [ ] **實體**: `MarketItem`, `MarketOrder`, `MarketTransaction`, `MarketReview`
- [ ] **倉庫**: `IPlayerMarketRepository` 完整實現
- [ ] **服務**: `PlayerMarketService` 完整實現
- [ ] **驗證器**: 市場相關 DTO 的驗證規則
- [ ] **控制器**: `PlayerMarketController` 完整端點

**依賴關係**: 依賴 User 和 Wallet 模組

### 階段 5: 管理與通知 (優先級：低)
**模組**: Notification → Sales → Manager

**目標**: 實現通知系統和管理功能

#### 5.1 通知模組 (Notification)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `NotificationResponse`, `CreateNotificationRequest`
  - `NotificationSettings`, `NotificationStats`
- [ ] **實體**: `Notification`, `NotificationTemplate`, `NotificationSource`
- [ ] **倉庫**: `INotificationRepository` 完整實現
- [ ] **服務**: `NotificationService` 完整實現
- [ ] **驗證器**: 通知相關 DTO 的驗證規則
- [ ] **控制器**: `NotificationController` 完整端點

**依賴關係**: 依賴 User 模組

#### 5.2 銷售模組 (Sales)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `SalesProfileResponse`, `UpdateSalesProfileRequest`
  - `SalesTransactionResponse`, `SalesStats`, `CommissionStatistics`
  - `SalesRanking`, `UpdateSalesSettingsRequest`
- [ ] **實體**: `SalesProfile`, `SalesTransaction`, `CommissionStatistics`
- [ ] **倉庫**: `ISalesRepository` 完整實現
- [ ] **服務**: `SalesService` 完整實現
- [ ] **驗證器**: 銷售相關 DTO 的驗證規則
- [ ] **控制器**: `SalesController` 完整端點

**依賴關係**: 依賴 User 和 Wallet 模組

#### 5.3 管理員模組 (Manager)
**需要完成的內容**:
- [ ] **DTOs**: 
  - `UserManagementRequest`, `SystemStats`, `SystemSettings`
  - `UpdateSystemSettingsRequest`, `SystemLog`
- [ ] **實體**: `Manager`, `ManagerRole`, `Permission`, `SystemLog`
- [ ] **倉庫**: `IManagerRepository` 完整實現
- [ ] **服務**: `ManagerService` 完整實現
- [ ] **驗證器**: 管理員相關 DTO 的驗證規則
- [ ] **控制器**: `ManagerController` 完整端點

**依賴關係**: 依賴 User 模組

## 每個模組的具體實作清單

### 基礎實作項目 (每個模組都需要)

#### 1. DTOs 層
- [ ] 創建所有必要的 Request DTOs
- [ ] 創建所有必要的 Response DTOs
- [ ] 添加適當的驗證特性
- [ ] 確保命名一致性

#### 2. 實體層
- [ ] 定義實體類別和屬性
- [ ] 添加必要的導航屬性
- [ ] 實現適當的建構函數
- [ ] 添加必要的驗證邏輯

#### 3. 倉庫層
- [ ] 實現 IRepository<T> 介面
- [ ] 實現特定模組的查詢方法
- [ ] 添加適當的索引和優化
- [ ] 實現分頁查詢邏輯

#### 4. 服務層
- [ ] 實現業務邏輯
- [ ] 添加適當的驗證
- [ ] 實現錯誤處理
- [ ] 添加日誌記錄

#### 5. 驗證層
- [ ] 創建 FluentValidation 驗證器
- [ ] 定義業務規則驗證
- [ ] 添加自定義驗證邏輯
- [ ] 確保錯誤消息一致性

#### 6. 控制器層
- [ ] 實現所有必要的端點
- [ ] 添加適當的授權檢查
- [ ] 實現統一的錯誤處理
- [ ] 添加 API 文檔註解

#### 7. 測試層
- [ ] 單元測試覆蓋
- [ ] 集成測試
- [ ] API 測試
- [ ] 性能測試

## 技術債務與重構項目

### 高優先級重構
1. **命名空間清理**: 移除所有 `GameCore.Core` 引用
2. **接口實現**: 確保所有倉庫正確實現對應接口
3. **依賴注入**: 配置所有服務的 DI 容器
4. **錯誤處理**: 統一所有層的錯誤處理方式

### 中優先級重構
1. **數據庫查詢優化**: 添加適當的索引
2. **緩存策略**: 實現適當的緩存機制
3. **日誌記錄**: 統一日誌記錄格式
4. **驗證邏輯**: 統一驗證規則和錯誤消息

### 低優先級重構
1. **代碼重構**: 提取重複代碼到共用方法
2. **性能優化**: 優化數據庫查詢和業務邏輯
3. **文檔完善**: 完善 API 文檔和代碼註解
4. **測試覆蓋**: 提高測試覆蓋率

## 交付標準

### 每個階段的交付物
1. **功能完整性**: 所有定義的功能都必須實現
2. **測試覆蓋**: 單元測試覆蓋率 > 80%
3. **編譯成功**: 整個解決方案必須編譯成功
4. **API 響應**: 所有 API 端點必須正確響應
5. **錯誤處理**: 統一的錯誤處理和響應格式

### 質量門檻
1. **代碼審查**: 所有代碼必須通過代碼審查
2. **測試通過**: 所有測試必須通過
3. **性能基準**: 符合性能要求
4. **安全檢查**: 通過安全掃描
5. **文檔完整**: 所有文檔必須完整

## 風險管理

### 技術風險
1. **依賴複雜性**: 模組間依賴可能過於複雜
2. **性能問題**: 大量數據處理可能導致性能問題
3. **數據一致性**: 分布式操作可能導致數據不一致

### 緩解策略
1. **模組化設計**: 保持模組間低耦合
2. **性能測試**: 早期進行性能測試
3. **事務管理**: 使用適當的事務管理策略

## 時間規劃

### 階段 1: 基礎架構與認證 (2-3 週)
- 週 1: Auth 模組
- 週 2: User 模組
- 週 3: Wallet 模組

### 階段 2: 社交功能 (2-3 週)
- 週 1: Chat 模組
- 週 2: Pet 模組
- 週 3: Forum 模組

### 階段 3: 遊戲與娛樂 (2-3 週)
- 週 1: MiniGame 模組
- 週 2: Game 模組

### 階段 4: 商業功能 (2-3 週)
- 週 1: Store 模組
- 週 2: PlayerMarket 模組

### 階段 5: 管理與通知 (2-3 週)
- 週 1: Notification 模組
- 週 2: Sales 模組
- 週 3: Manager 模組

### 總計預估時間: 10-15 週

## 總結

本實作計劃提供了 GameCore 專案架構重設後的完整路線圖。通過分階段實作，我們可以：

1. **降低風險**: 逐步實現功能，及時發現和解決問題
2. **提高質量**: 每個階段都有明確的交付標準
3. **便於管理**: 清晰的里程碑和時間規劃
4. **團隊協作**: 明確的依賴關係和責任分工

記住：質量比速度更重要，每個階段都必須達到交付標準才能進入下一階段。 