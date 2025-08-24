# 建置錯誤診斷報告

## 主要問題類型

### 1. 重複定義錯誤 (CS0101)
- `ChatMessage` 在 `Notification.cs` 和 `IUnitOfWork.cs` 中重複定義
- `Forum` 在 `Post.cs` 中重複定義  
- `Post` 在 `Post.cs` 中重複定義
- 多個 Repository 介面在 `IUnitOfWork.cs` 中重複定義

### 2. 缺少 DTO 類別 (CS0246)
- `ChatRoomDto`, `ChatRoomCreateDto`, `ChatRoomUpdateDto`
- `ChatMessageDto`, `ChatMessageCreateDto`
- `PrivateChatDto`, `PrivateMessageDto`, `PrivateMessageCreateDto`
- `ForumDto`, `PostDetailResult`, `PostCreateDto`, `PostUpdateDto`
- `ManagerLoginDto`, `ManagerProfileResult`, `ManagerUpdateDto`
- `ProductDto`, `CartResult`, `CartItemCreateDto`, `OrderCreateDto`
- `MarketItemDto`, `MarketItemCreateDto`, `MarketItemUpdateDto`
- `TransactionDto`, `ReviewCreateDto`, `ReviewDto`
- `UserRegistrationDto`, `UserLoginDto`, `UserProfileResult`

### 3. 重複屬性錯誤 (CS0579)
- `Table` 屬性重複：`Notification.cs:189`, `Post.cs:206,9`
- `Column` 屬性重複：`Game.cs:227,292,349`, `Store.cs:64,329,393,406,465`, `Pet.cs:251`, `PlayerMarket.cs:73`, `Post.cs:138`

### 4. 介面實作不完整 (CS0535)
- `ChatService` 缺少多個介面方法實作
- `ForumService` 缺少多個介面方法實作
- `NotificationService` 缺少多個介面方法實作
- `StoreService` 缺少多個介面方法實作
- `UserService` 缺少多個介面方法實作
- `PlayerMarketService` 缺少多個介面方法實作
- `ManagerService` 缺少多個介面方法實作

### 5. 重複方法定義 (CS0111)
- `IManagerRepository` 中 `GetByAccountAsync` 和 `GetPermissionsAsync` 重複定義

## 修正優先順序
1. 移除重複定義
2. 建立缺少的 DTO 類別
3. 修正重複屬性
4. 實作缺少的介面方法
5. 移除重複方法定義