namespace GameCore.Core.DTOs
{
    #region Forum DTOs
    /// <summary>
    /// 論壇 DTO
    /// </summary>
    public class ForumDto
    {
        public int ForumId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PostCount { get; set; }
        public int ReplyCount { get; set; }
        public int ViewCount { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 貼文詳細結果
    /// </summary>
    public class PostDetailResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ForumPostDto? Post { get; set; }
    }

    /// <summary>
    /// 論壇貼文 DTO
    /// </summary>
    public class ForumPostDto
    {
        public int PostId { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public PostStatus Status { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastReplyAt { get; set; }
    }

    /// <summary>
    /// 貼文建立 DTO
    /// </summary>
    public class PostCreateDto
    {
        public int ForumId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 貼文更新 DTO
    /// </summary>
    public class PostUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 貼文更新結果
    /// </summary>
    public class PostUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ForumPostDto? Post { get; set; }
    }

    /// <summary>
    /// 貼文刪除結果
    /// </summary>
    public class PostDeleteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 回覆列表結果
    /// </summary>
    public class ReplyListResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PostReplyDto> Replies { get; set; } = new List<PostReplyDto>();
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 論壇回覆 DTO
    /// </summary>
    public class PostReplyDto
    {
        public int ReplyId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public PostReplyStatus Status { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 回覆建立 DTO
    /// </summary>
    public class ReplyCreateDto
    {
        public int PostId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 回覆更新 DTO
    /// </summary>
    public class ReplyUpdateDto
    {
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 回覆更新結果
    /// </summary>
    public class ReplyUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PostReplyDto? Reply { get; set; }
    }

    /// <summary>
    /// 回覆刪除結果
    /// </summary>
    public class ReplyDeleteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 搜尋結果
    /// </summary>
    public class SearchResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<ForumPostDto> Posts { get; set; } = new List<ForumPostDto>();
        public int TotalCount { get; set; }
    }
    #endregion

    #region Manager DTOs
    /// <summary>
    /// 管理者登入 DTO
    /// </summary>
    public class ManagerLoginDto
    {
        public string Account { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// 管理者登入結果
    /// </summary>
    public class ManagerLoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ManagerDto? Manager { get; set; }
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// 管理者資料 DTO
    /// </summary>
    public class ManagerDto
    {
        public int ManagerId { get; set; }
        public string Account { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ManagerRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 管理者資料結果
    /// </summary>
    public class ManagerProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ManagerDto? Manager { get; set; }
    }

    /// <summary>
    /// 管理者更新 DTO
    /// </summary>
    public class ManagerUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// 管理者更新結果
    /// </summary>
    public class ManagerUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ManagerDto? Manager { get; set; }
    }

    /// <summary>
    /// 密碼變更 DTO
    /// </summary>
    public class PasswordChangeDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 密碼變更結果
    /// </summary>
    public class PasswordChangeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 管理者建立 DTO
    /// </summary>
    public class ManagerCreateDto
    {
        public string Account { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ManagerRole Role { get; set; }
    }

    /// <summary>
    /// 管理者建立結果
    /// </summary>
    public class ManagerCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ManagerDto? Manager { get; set; }
    }

    /// <summary>
    /// 管理者角色權限 DTO
    /// </summary>
    public class ManagerRolePermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsGranted { get; set; }
    }

    /// <summary>
    /// 管理者角色
    /// </summary>
    public enum ManagerRole
    {
        Admin = 1,
        Moderator = 2,
        Support = 3
    }
    #endregion

    #region Store DTOs
    /// <summary>
    /// 商品 DTO
    /// </summary>
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 購物車結果
    /// </summary>
    public class CartResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }

    /// <summary>
    /// 購物車項目 DTO
    /// </summary>
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    /// <summary>
    /// 購物車項目建立 DTO
    /// </summary>
    public class CartItemCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 購物車項目結果
    /// </summary>
    public class CartItemResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public CartItemDto? Item { get; set; }
    }

    /// <summary>
    /// 訂單建立 DTO
    /// </summary>
    public class OrderCreateDto
    {
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// 訂單 DTO
    /// </summary>
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal OrderTotal { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    /// <summary>
    /// 訂單項目 DTO
    /// </summary>
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    /// <summary>
    /// 訂單結果
    /// </summary>
    public class OrderResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDto? Order { get; set; }
    }

    /// <summary>
    /// 訂單更新結果
    /// </summary>
    public class OrderUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDto? Order { get; set; }
    }

    /// <summary>
    /// 訂單狀態
    /// </summary>
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Refunded = 6
    }
    #endregion

    #region Player Market DTOs
    /// <summary>
    /// 市場商品 DTO
    /// </summary>
    public class MarketItemDto
    {
        public int ItemId { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public MarketItemStatus Status { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 市場商品建立 DTO
    /// </summary>
    public class MarketItemCreateDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// 市場商品建立結果
    /// </summary>
    public class MarketItemCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public MarketItemDto? Item { get; set; }
    }

    /// <summary>
    /// 市場商品更新 DTO
    /// </summary>
    public class MarketItemUpdateDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// 市場商品更新結果
    /// </summary>
    public class MarketItemUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public MarketItemDto? Item { get; set; }
    }

    /// <summary>
    /// 交易 DTO
    /// </summary>
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public int BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    /// <summary>
    /// 評論建立 DTO
    /// </summary>
    public class ReviewCreateDto
    {
        public int TransactionId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    /// <summary>
    /// 評論建立結果
    /// </summary>
    public class ReviewCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ReviewDto? Review { get; set; }
    }

    /// <summary>
    /// 評論 DTO
    /// </summary>
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int TransactionId { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 市場商品狀態
    /// </summary>
    public enum MarketItemStatus
    {
        Active = 1,
        Sold = 2,
        Cancelled = 3,
        Expired = 4
    }

    /// <summary>
    /// 交易狀態
    /// </summary>
    public enum TransactionStatus
    {
        Pending = 1,
        Confirmed = 2,
        Completed = 3,
        Cancelled = 4,
        Disputed = 5
    }
    #endregion

    #region User DTOs
    /// <summary>
    /// 使用者註冊 DTO
    /// </summary>
    public class UserRegistrationDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 使用者登入 DTO
    /// </summary>
    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// 使用者資料結果
    /// </summary>
    public class UserProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }

    /// <summary>
    /// 使用者 DTO
    /// </summary>
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 使用者更新 DTO
    /// </summary>
    public class UserUpdateDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
    }

    /// <summary>
    /// 使用者更新結果
    /// </summary>
    public class UserUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }
    #endregion

    #region Sign In DTOs
    /// <summary>
    /// 簽到結果
    /// </summary>
    public class SignInResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int ConsecutiveDays { get; set; }
        public int PointsEarned { get; set; }
        public DateTime SignInDate { get; set; }
    }

    /// <summary>
    /// 簽到狀態結果
    /// </summary>
    public class SignInStatusResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool HasSignedInToday { get; set; }
        public int ConsecutiveDays { get; set; }
        public DateTime? LastSignInDate { get; set; }
    }

    /// <summary>
    /// 簽到記錄 DTO
    /// </summary>
    public class SignInRecordDto
    {
        public int RecordId { get; set; }
        public int UserId { get; set; }
        public DateTime SignInDate { get; set; }
        public int PointsEarned { get; set; }
        public int ConsecutiveDays { get; set; }
    }

    /// <summary>
    /// 簽到統計 DTO
    /// </summary>
    public class SignInStatisticsDto
    {
        public int TotalSignIns { get; set; }
        public int ConsecutiveDays { get; set; }
        public int MaxConsecutiveDays { get; set; }
        public int TotalPointsEarned { get; set; }
        public DateTime FirstSignInDate { get; set; }
        public DateTime? LastSignInDate { get; set; }
    }

    /// <summary>
    /// 簽到日曆 DTO
    /// </summary>
    public class SignInCalendarDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<DateTime> SignInDates { get; set; } = new List<DateTime>();
    }
    #endregion

    #region Notification DTOs
    /// <summary>
    /// 通知 DTO
    /// </summary>
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }

    /// <summary>
    /// 通知類型
    /// </summary>
    public enum NotificationType
    {
        System = 1,
        Pet = 2,
        Order = 3,
        Chat = 4,
        Forum = 5
    }
    #endregion
}