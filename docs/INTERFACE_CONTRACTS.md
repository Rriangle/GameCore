# GameCore 介面契約規範

## 概述

本文檔定義了 GameCore 專案中所有 Repository 和 Service 介面的統一命名規範和方法簽名，確保整個系統的一致性和可預測性。

## 統一回應模式

### 1. Result<T> 模式
```csharp
// 成功回應
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
    public IReadOnlyList<string> Errors { get; }
    
    public static Result<T> Success(T value);
    public static Result<T> Failure(string error);
    public static Result<T> Failure(IEnumerable<string> errors);
}

// 操作結果
public class OperationResult
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public IReadOnlyList<string> Errors { get; }
    
    public static OperationResult Success();
    public static OperationResult Failure(string error);
    public static OperationResult Failure(IEnumerable<string> errors);
}
```

### 2. 分頁結果模式
```csharp
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
}
```

## Repository 介面契約

### 1. 基礎 Repository 介面
```csharp
public interface IRepository<T> where T : class
{
    // 查詢操作
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    
    // 修改操作
    Task<int> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    
    // 批次操作
    Task<IEnumerable<int>> CreateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteManyAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
```

### 2. 用戶相關 Repository

#### IUserRepository
```csharp
public interface IUserRepository : IRepository<User>
{
    // 基本查詢
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetWithProfileAsync(int userId, CancellationToken cancellationToken = default);
    Task<User?> GetWithWalletAsync(int userId, CancellationToken cancellationToken = default);
    Task<User?> GetWithPetsAsync(int userId, CancellationToken cancellationToken = default);
    
    // 狀態查詢
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersByStatusAsync(UserStatus status, CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetUsersByRoleAsync(UserRole role, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 統計查詢
    Task<int> GetActiveUserCountAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetRecentlyRegisteredUsersAsync(int days, CancellationToken cancellationToken = default);
    
    // 驗證操作
    Task<bool> IsUserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<OperationResult> ActivateUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> DeactivateUserAsync(int userId, CancellationToken cancellationToken = default);
}
```

#### IUserWalletRepository
```csharp
public interface IUserWalletRepository : IRepository<UserWallet>
{
    // 錢包查詢
    Task<UserWallet?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserWallet?> GetWithTransactionsAsync(int userId, CancellationToken cancellationToken = default);
    
    // 餘額操作
    Task<OperationResult> UpdateBalanceAsync(int userId, decimal amount, CancellationToken cancellationToken = default);
    Task<OperationResult> AddPointsAsync(int userId, int points, CancellationToken cancellationToken = default);
    Task<OperationResult> DeductPointsAsync(int userId, int points, CancellationToken cancellationToken = default);
    
    // 交易記錄
    Task<IEnumerable<WalletTransaction>> GetTransactionHistoryAsync(int userId, int limit = 50, CancellationToken cancellationToken = default);
    Task<PagedResult<WalletTransaction>> GetPagedTransactionsAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 統計查詢
    Task<decimal> GetTotalBalanceAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalPointsAsync(CancellationToken cancellationToken = default);
}
```

### 3. 聊天相關 Repository

#### IChatRepository
```csharp
public interface IChatRepository : IRepository<ChatMessage>
{
    // 聊天室訊息
    Task<IEnumerable<ChatMessage>> GetRoomMessagesAsync(int roomId, int limit = 50, CancellationToken cancellationToken = default);
    Task<PagedResult<ChatMessage>> GetPagedRoomMessagesAsync(int roomId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatMessage>> GetRecentMessagesAsync(int roomId, DateTime since, CancellationToken cancellationToken = default);
    
    // 私人訊息
    Task<IEnumerable<ChatMessage>> GetPrivateMessagesAsync(int senderId, int receiverId, int limit = 50, CancellationToken cancellationToken = default);
    Task<PagedResult<ChatMessage>> GetPagedPrivateMessagesAsync(int senderId, int receiverId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 訊息管理
    Task<OperationResult> MarkAsReadAsync(int messageId, CancellationToken cancellationToken = default);
    Task<OperationResult> MarkAllAsReadAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    
    // 聊天室管理
    Task<IEnumerable<ChatRoom>> GetUserRoomsAsync(int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> JoinRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    Task<OperationResult> LeaveRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default);
}
```

### 4. 寵物相關 Repository

#### IPetRepository
```csharp
public interface IPetRepository : IRepository<Pet>
{
    // 寵物查詢
    Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId, CancellationToken cancellationToken = default);
    Task<Pet?> GetWithOwnerAsync(int petId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pet>> GetActivePetsAsync(int ownerId, CancellationToken cancellationToken = default);
    
    // 寵物狀態
    Task<IEnumerable<Pet>> GetPetsByStatusAsync(PetStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pet>> GetHungryPetsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Pet>> GetSickPetsAsync(CancellationToken cancellationToken = default);
    
    // 寵物照護
    Task<OperationResult> FeedPetAsync(int petId, CancellationToken cancellationToken = default);
    Task<OperationResult> CleanPetAsync(int petId, CancellationToken cancellationToken = default);
    Task<OperationResult> PlayWithPetAsync(int petId, CancellationToken cancellationToken = default);
    Task<OperationResult> RestPetAsync(int petId, CancellationToken cancellationToken = default);
    
    // 統計查詢
    Task<int> GetPetCountByOwnerAsync(int ownerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pet>> GetMostActivePetsAsync(int limit = 10, CancellationToken cancellationToken = default);
}
```

### 5. 遊戲相關 Repository

#### IMiniGameRepository
```csharp
public interface IMiniGameRepository : IRepository<MiniGame>
{
    // 遊戲查詢
    Task<IEnumerable<MiniGame>> GetActiveGamesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MiniGame>> GetGamesByTypeAsync(GameType type, CancellationToken cancellationToken = default);
    Task<MiniGame?> GetWithSettingsAsync(int gameId, CancellationToken cancellationToken = default);
    
    // 遊戲記錄
    Task<IEnumerable<MiniGameRecord>> GetUserGameRecordsAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MiniGameRecord>> GetGameRecordsAsync(int gameId, int limit = 100, CancellationToken cancellationToken = default);
    Task<PagedResult<MiniGameRecord>> GetPagedGameRecordsAsync(int gameId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 排行榜
    Task<IEnumerable<MiniGameRecord>> GetLeaderboardAsync(int gameId, int limit = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<MiniGameRecord>> GetUserBestScoresAsync(int userId, CancellationToken cancellationToken = default);
    
    // 遊戲統計
    Task<int> GetGamePlayCountAsync(int gameId, CancellationToken cancellationToken = default);
    Task<double> GetAverageScoreAsync(int gameId, CancellationToken cancellationToken = default);
    Task<OperationResult> RecordGameResultAsync(int userId, int gameId, int score, CancellationToken cancellationToken = default);
}
```

### 6. 市場相關 Repository

#### IPlayerMarketRepository
```csharp
public interface IPlayerMarketRepository : IRepository<PlayerMarketItem>
{
    // 商品查詢
    Task<IEnumerable<PlayerMarketItem>> GetActiveItemsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PlayerMarketItem>> GetItemsBySellerAsync(int sellerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PlayerMarketItem>> GetItemsByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<PagedResult<PlayerMarketItem>> SearchItemsAsync(string keyword, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 價格查詢
    Task<IEnumerable<PlayerMarketItem>> GetItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    Task<IEnumerable<PlayerMarketItem>> GetRecentlyListedAsync(int days, CancellationToken cancellationToken = default);
    Task<IEnumerable<PlayerMarketItem>> GetPopularItemsAsync(int limit = 20, CancellationToken cancellationToken = default);
    
    // 交易操作
    Task<OperationResult> ListItemAsync(PlayerMarketItem item, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateItemAsync(int itemId, decimal newPrice, CancellationToken cancellationToken = default);
    Task<OperationResult> RemoveItemAsync(int itemId, CancellationToken cancellationToken = default);
    Task<OperationResult> PurchaseItemAsync(int itemId, int buyerId, CancellationToken cancellationToken = default);
    
    // 交易記錄
    Task<IEnumerable<MarketTransaction>> GetTransactionHistoryAsync(int userId, CancellationToken cancellationToken = default);
    Task<PagedResult<MarketTransaction>> GetPagedTransactionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
```

### 7. 商店相關 Repository

#### IStoreRepository
```csharp
public interface IStoreRepository : IRepository<StoreProduct>
{
    // 商品查詢
    Task<IEnumerable<StoreProduct>> GetFeaturedProductsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreProduct>> GetProductsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<PagedResult<StoreProduct>> SearchProductsAsync(string keyword, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreProduct>> GetPopularProductsAsync(int limit = 20, CancellationToken cancellationToken = default);
    
    // 購物車操作
    Task<IEnumerable<StoreCartItem>> GetCartItemsAsync(int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> AddToCartAsync(int userId, int productId, int quantity, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateCartItemAsync(int cartItemId, int quantity, CancellationToken cancellationToken = default);
    Task<OperationResult> RemoveFromCartAsync(int cartItemId, CancellationToken cancellationToken = default);
    Task<OperationResult> ClearCartAsync(int userId, CancellationToken cancellationToken = default);
    
    // 訂單操作
    Task<IEnumerable<StoreOrder>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default);
    Task<StoreOrder?> GetOrderWithItemsAsync(int orderId, CancellationToken cancellationToken = default);
    Task<OperationResult> CreateOrderAsync(StoreOrder order, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateOrderStatusAsync(int orderId, OrderStatus status, CancellationToken cancellationToken = default);
    
    // 商品評價
    Task<IEnumerable<StoreProductReview>> GetProductReviewsAsync(int productId, CancellationToken cancellationToken = default);
    Task<OperationResult> AddReviewAsync(StoreProductReview review, CancellationToken cancellationToken = default);
    Task<double> GetProductRatingAsync(int productId, CancellationToken cancellationToken = default);
}
```

### 8. 通知相關 Repository

#### INotificationRepository
```csharp
public interface INotificationRepository : IRepository<Notification>
{
    // 通知查詢
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId, CancellationToken cancellationToken = default);
    Task<PagedResult<Notification>> GetPagedNotificationsAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 通知操作
    Task<OperationResult> CreateNotificationAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<OperationResult> MarkAsReadAsync(int notificationId, CancellationToken cancellationToken = default);
    Task<OperationResult> MarkAllAsReadAsync(int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteNotificationAsync(int notificationId, CancellationToken cancellationToken = default);
    
    // 通知統計
    Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int userId, int days, CancellationToken cancellationToken = default);
    
    // 批次操作
    Task<OperationResult> CreateBulkNotificationsAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteOldNotificationsAsync(int days, CancellationToken cancellationToken = default);
}
```

## Service 介面契約

### 1. 認證服務

#### IAuthService
```csharp
public interface IAuthService
{
    // 登入/登出
    Task<Result<AuthTokenDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<OperationResult> LogoutAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<AuthTokenDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    
    // 註冊
    Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
    Task<OperationResult> VerifyEmailAsync(string token, CancellationToken cancellationToken = default);
    Task<OperationResult> ResendVerificationEmailAsync(string email, CancellationToken cancellationToken = default);
    
    // 密碼管理
    Task<OperationResult> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default);
    Task<OperationResult> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
    Task<OperationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default);
    
    // 權限驗證
    Task<bool> HasPermissionAsync(int userId, string permission, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserPermissionsAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> IsInRoleAsync(int userId, string role, CancellationToken cancellationToken = default);
}
```

### 2. 用戶服務

#### IUserService
```csharp
public interface IUserService
{
    // 用戶查詢
    Task<Result<UserDto>> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<PagedResult<UserDto>>> GetPagedUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 用戶管理
    Task<Result<UserDto>> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateUserAsync(int id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    Task<OperationResult> ActivateUserAsync(int id, CancellationToken cancellationToken = default);
    Task<OperationResult> DeactivateUserAsync(int id, CancellationToken cancellationToken = default);
    
    // 用戶資料
    Task<Result<UserProfileDto>> GetUserProfileAsync(int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateUserProfileAsync(int userId, UpdateUserProfileDto profileDto, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateUserAvatarAsync(int userId, string avatarUrl, CancellationToken cancellationToken = default);
    
    // 用戶統計
    Task<Result<UserStatsDto>> GetUserStatsAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserDto>>> GetRecentlyActiveUsersAsync(int limit = 10, CancellationToken cancellationToken = default);
}
```

### 3. 錢包服務

#### IWalletService
```csharp
public interface IWalletService
{
    // 錢包查詢
    Task<Result<UserWalletDto>> GetWalletAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<WalletBalanceDto>> GetBalanceAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<WalletTransactionDto>>> GetTransactionHistoryAsync(int userId, int limit = 50, CancellationToken cancellationToken = default);
    
    // 餘額操作
    Task<OperationResult> DepositAsync(int userId, decimal amount, string description, CancellationToken cancellationToken = default);
    Task<OperationResult> WithdrawAsync(int userId, decimal amount, string description, CancellationToken cancellationToken = default);
    Task<OperationResult> TransferAsync(int fromUserId, int toUserId, decimal amount, string description, CancellationToken cancellationToken = default);
    
    // 點數操作
    Task<OperationResult> AddPointsAsync(int userId, int points, string description, CancellationToken cancellationToken = default);
    Task<OperationResult> DeductPointsAsync(int userId, int points, string description, CancellationToken cancellationToken = default);
    Task<OperationResult> ExchangePointsAsync(int userId, int points, decimal exchangeRate, CancellationToken cancellationToken = default);
    
    // 錢包統計
    Task<Result<WalletStatsDto>> GetWalletStatsAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<decimal>> GetTotalSystemBalanceAsync(CancellationToken cancellationToken = default);
}
```

### 4. 聊天服務

#### IChatService
```csharp
public interface IChatService
{
    // 訊息操作
    Task<Result<ChatMessageDto>> SendMessageAsync(SendMessageDto sendMessageDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ChatMessageDto>>> GetRoomMessagesAsync(int roomId, int limit = 50, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ChatMessageDto>>> GetPrivateMessagesAsync(int senderId, int receiverId, int limit = 50, CancellationToken cancellationToken = default);
    
    // 聊天室管理
    Task<Result<ChatRoomDto>> CreateRoomAsync(CreateChatRoomDto createRoomDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ChatRoomDto>>> GetUserRoomsAsync(int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> JoinRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    Task<OperationResult> LeaveRoomAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    
    // 訊息狀態
    Task<OperationResult> MarkAsReadAsync(int messageId, int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> MarkAllAsReadAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    Task<Result<int>> GetUnreadCountAsync(int userId, int roomId, CancellationToken cancellationToken = default);
    
    // 訊息管理
    Task<OperationResult> DeleteMessageAsync(int messageId, int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> EditMessageAsync(int messageId, string newContent, int userId, CancellationToken cancellationToken = default);
}
```

### 5. 寵物服務

#### IPetService
```csharp
public interface IPetService
{
    // 寵物管理
    Task<Result<PetDto>> CreatePetAsync(int ownerId, CreatePetDto createPetDto, CancellationToken cancellationToken = default);
    Task<Result<PetDto>> GetPetByIdAsync(int petId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PetDto>>> GetUserPetsAsync(int ownerId, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdatePetAsync(int petId, UpdatePetDto updatePetDto, CancellationToken cancellationToken = default);
    Task<OperationResult> DeletePetAsync(int petId, CancellationToken cancellationToken = default);
    
    // 寵物照護
    Task<OperationResult> FeedPetAsync(int petId, CancellationToken cancellationToken = default);
    Task<OperationResult> CleanPetAsync(int petId, CancellationToken cancellationToken = default);
    Task<OperationResult> PlayWithPetAsync(int petId, CancellationToken cancellationToken = default);
    Task<OperationResult> RestPetAsync(int petId, CancellationToken cancellationToken = default);
    
    // 寵物狀態
    Task<Result<PetStatusDto>> GetPetStatusAsync(int petId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PetDto>>> GetHungryPetsAsync(int ownerId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PetDto>>> GetSickPetsAsync(int ownerId, CancellationToken cancellationToken = default);
    
    // 寵物統計
    Task<Result<PetStatsDto>> GetPetStatsAsync(int petId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PetDto>>> GetMostActivePetsAsync(int limit = 10, CancellationToken cancellationToken = default);
}
```

### 6. 小遊戲服務

#### IMiniGameService
```csharp
public interface IMiniGameService
{
    // 遊戲查詢
    Task<Result<IEnumerable<MiniGameDto>>> GetActiveGamesAsync(CancellationToken cancellationToken = default);
    Task<Result<MiniGameDto>> GetGameByIdAsync(int gameId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<MiniGameDto>>> GetGamesByTypeAsync(GameType type, CancellationToken cancellationToken = default);
    
    // 遊戲記錄
    Task<OperationResult> RecordGameResultAsync(RecordGameResultDto recordDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<MiniGameRecordDto>>> GetUserGameRecordsAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<MiniGameRecordDto>>> GetGameRecordsAsync(int gameId, int limit = 100, CancellationToken cancellationToken = default);
    
    // 排行榜
    Task<Result<IEnumerable<LeaderboardEntryDto>>> GetLeaderboardAsync(int gameId, int limit = 10, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<MiniGameRecordDto>>> GetUserBestScoresAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<int>> GetUserRankingAsync(int userId, int gameId, CancellationToken cancellationToken = default);
    
    // 遊戲統計
    Task<Result<GameStatsDto>> GetGameStatsAsync(int gameId, CancellationToken cancellationToken = default);
    Task<Result<UserGameStatsDto>> GetUserGameStatsAsync(int userId, CancellationToken cancellationToken = default);
}
```

### 7. 玩家市場服務

#### IPlayerMarketService
```csharp
public interface IPlayerMarketService
{
    // 商品管理
    Task<Result<PlayerMarketItemDto>> ListItemAsync(ListItemDto listItemDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PlayerMarketItemDto>>> GetActiveItemsAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PlayerMarketItemDto>>> GetUserItemsAsync(int sellerId, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateItemAsync(int itemId, UpdateItemDto updateItemDto, CancellationToken cancellationToken = default);
    Task<OperationResult> RemoveItemAsync(int itemId, CancellationToken cancellationToken = default);
    
    // 商品搜尋
    Task<Result<PagedResult<PlayerMarketItemDto>>> SearchItemsAsync(SearchItemsDto searchDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PlayerMarketItemDto>>> GetItemsByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PlayerMarketItemDto>>> GetItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    
    // 交易操作
    Task<Result<MarketTransactionDto>> PurchaseItemAsync(PurchaseItemDto purchaseDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<MarketTransactionDto>>> GetTransactionHistoryAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<MarketTransactionDto>> GetTransactionByIdAsync(int transactionId, CancellationToken cancellationToken = default);
    
    // 市場統計
    Task<Result<MarketStatsDto>> GetMarketStatsAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PlayerMarketItemDto>>> GetPopularItemsAsync(int limit = 20, CancellationToken cancellationToken = default);
}
```

### 8. 商店服務

#### IStoreService
```csharp
public interface IStoreService
{
    // 商品查詢
    Task<Result<IEnumerable<StoreProductDto>>> GetFeaturedProductsAsync(CancellationToken cancellationToken = default);
    Task<Result<StoreProductDto>> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<StoreProductDto>>> GetProductsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StoreProductDto>>> SearchProductsAsync(string keyword, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 購物車操作
    Task<Result<IEnumerable<StoreCartItemDto>>> GetCartItemsAsync(int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> AddToCartAsync(AddToCartDto addToCartDto, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateCartItemAsync(int cartItemId, int quantity, CancellationToken cancellationToken = default);
    Task<OperationResult> RemoveFromCartAsync(int cartItemId, CancellationToken cancellationToken = default);
    Task<OperationResult> ClearCartAsync(int userId, CancellationToken cancellationToken = default);
    
    // 訂單操作
    Task<Result<StoreOrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<StoreOrderDto>>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<StoreOrderDto>> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateOrderStatusAsync(int orderId, OrderStatus status, CancellationToken cancellationToken = default);
    
    // 商品評價
    Task<Result<IEnumerable<StoreProductReviewDto>>> GetProductReviewsAsync(int productId, CancellationToken cancellationToken = default);
    Task<OperationResult> AddReviewAsync(AddReviewDto addReviewDto, CancellationToken cancellationToken = default);
    Task<Result<double>> GetProductRatingAsync(int productId, CancellationToken cancellationToken = default);
}
```

### 9. 通知服務

#### INotificationService
```csharp
public interface INotificationService
{
    // 通知查詢
    Task<Result<IEnumerable<NotificationDto>>> GetUserNotificationsAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<NotificationDto>>> GetUnreadNotificationsAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<NotificationDto>>> GetPagedNotificationsAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    // 通知操作
    Task<Result<NotificationDto>> CreateNotificationAsync(CreateNotificationDto createNotificationDto, CancellationToken cancellationToken = default);
    Task<OperationResult> MarkAsReadAsync(int notificationId, CancellationToken cancellationToken = default);
    Task<OperationResult> MarkAllAsReadAsync(int userId, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteNotificationAsync(int notificationId, CancellationToken cancellationToken = default);
    
    // 批次通知
    Task<OperationResult> SendBulkNotificationAsync(SendBulkNotificationDto bulkNotificationDto, CancellationToken cancellationToken = default);
    Task<OperationResult> SendSystemNotificationAsync(string message, CancellationToken cancellationToken = default);
    
    // 通知統計
    Task<Result<int>> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<NotificationStatsDto>> GetNotificationStatsAsync(int userId, CancellationToken cancellationToken = default);
}
```

### 10. 管理服務

#### IManagerService
```csharp
public interface IManagerService
{
    // 管理員查詢
    Task<Result<ManagerDto>> GetManagerByIdAsync(int managerId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ManagerDto>>> GetAllManagersAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ManagerDto>>> GetManagersByRoleAsync(string role, CancellationToken cancellationToken = default);
    
    // 管理員管理
    Task<Result<ManagerDto>> CreateManagerAsync(CreateManagerDto createManagerDto, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateManagerAsync(int managerId, UpdateManagerDto updateManagerDto, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteManagerAsync(int managerId, CancellationToken cancellationToken = default);
    Task<OperationResult> ActivateManagerAsync(int managerId, CancellationToken cancellationToken = default);
    Task<OperationResult> DeactivateManagerAsync(int managerId, CancellationToken cancellationToken = default);
    
    // 權限管理
    Task<Result<IEnumerable<string>>> GetManagerPermissionsAsync(int managerId, CancellationToken cancellationToken = default);
    Task<OperationResult> AssignPermissionAsync(int managerId, string permission, CancellationToken cancellationToken = default);
    Task<OperationResult> RevokePermissionAsync(int managerId, string permission, CancellationToken cancellationToken = default);
    
    // 管理統計
    Task<Result<ManagerStatsDto>> GetManagerStatsAsync(int managerId, CancellationToken cancellationToken = default);
    Task<Result<SystemStatsDto>> GetSystemStatsAsync(CancellationToken cancellationToken = default);
}
```

## 統一驗證規範

### 1. 輸入驗證
- 所有 DTO 必須實現相應的 Validator
- 使用 FluentValidation 進行驗證
- 驗證錯誤統一回傳 `Result.Failure(errors)`

### 2. 權限驗證
- 所有需要權限的操作必須先檢查權限
- 使用統一的 `IAuthService.HasPermissionAsync()` 方法
- 權限不足時回傳 `Result.Failure("Access denied")`

### 3. 資料驗證
- 檢查實體是否存在
- 檢查業務規則
- 檢查資料完整性

## 錯誤處理規範

### 1. 錯誤分類
- **ValidationError**: 輸入驗證錯誤
- **BusinessRuleError**: 業務規則錯誤  
- **NotFoundError**: 資源不存在錯誤
- **UnauthorizedError**: 權限錯誤
- **SystemError**: 系統錯誤

### 2. 錯誤訊息
- 使用繁體中文錯誤訊息
- 提供具體的錯誤描述
- 不暴露敏感系統資訊

### 3. 日誌記錄
- 記錄所有錯誤到日誌系統
- 包含必要的上下文資訊
- 區分錯誤等級（Warning, Error, Critical）

## 性能考量

### 1. 分頁查詢
- 大量資料查詢必須使用分頁
- 預設分頁大小：20
- 最大分頁大小：100

### 2. 快取策略
- 常用資料使用快取
- 設定合適的快取過期時間
- 支援快取失效機制

### 3. 非同步操作
- 所有 I/O 操作使用非同步方法
- 正確處理 CancellationToken
- 避免死鎖和阻塞

---

*本介面契約規範將隨著專案發展持續更新和完善。所有開發人員必須嚴格遵循這些規範。* 