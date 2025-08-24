namespace GameCore.Core.Enums
{
    /// <summary>
    /// 管理員角色枚舉
    /// </summary>
    public enum ManagerRole
    {
        Admin = 1,
        Moderator = 2,
        StoreManager = 3,
        ForumManager = 4,
        CustomerService = 5,
        PetManager = 6
    }

    /// <summary>
    /// 訂單狀態枚舉
    /// </summary>
    public enum OrderStatus
    {
        Created = 1,
        Pending = 2,
        Paid = 3,
        Processing = 4,
        Shipped = 5,
        Delivered = 6,
        Completed = 7,
        Cancelled = 8,
        Refunded = 9
    }

    /// <summary>
    /// 市場商品狀態枚舉
    /// </summary>
    public enum MarketItemStatus
    {
        Draft = 1,
        Active = 2,
        Paused = 3,
        Sold = 4,
        Cancelled = 5,
        Expired = 6
    }

    /// <summary>
    /// 交易狀態枚舉
    /// </summary>
    public enum TransactionStatus
    {
        Created = 1,
        Pending = 2,
        Confirmed = 3,
        InProgress = 4,
        SellerTransferred = 5,
        BuyerReceived = 6,
        Completed = 7,
        Cancelled = 8,
        Disputed = 9,
        Refunded = 10
    }

    /// <summary>
    /// 聊天訊息類型枚舉
    /// </summary>
    public enum ChatMessageType
    {
        Text = 1,
        Image = 2,
        File = 3,
        System = 4,
        Emoji = 5,
        Link = 6
    }

    /// <summary>
    /// 聊天成員角色枚舉
    /// </summary>
    public enum ChatMemberRole
    {
        Member = 1,
        Admin = 2,
        Owner = 3,
        Moderator = 4
    }

    /// <summary>
    /// 通知類型枚舉
    /// </summary>
    public enum NotificationType
    {
        System = 1,
        Order = 2,
        Forum = 3,
        Chat = 4,
        Pet = 5,
        SignIn = 6,
        Market = 7,
        Achievement = 8,
        Warning = 9,
        Promotion = 10
    }

    /// <summary>
    /// 貼文狀態枚舉
    /// </summary>
    public enum PostStatus
    {
        Draft = 1,
        Published = 2,
        Hidden = 3,
        Archived = 4,
        Deleted = 5
    }

    /// <summary>
    /// 用戶狀態枚舉
    /// </summary>
    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Suspended = 3,
        Banned = 4,
        PendingVerification = 5
    }

    /// <summary>
    /// 寵物狀態枚舉
    /// </summary>
    public enum PetStatus
    {
        Healthy = 1,
        Hungry = 2,
        Sad = 3,
        Tired = 4,
        Dirty = 5,
        Sick = 6,
        Happy = 7,
        Sleeping = 8
    }

    /// <summary>
    /// 小遊戲結果枚舉
    /// </summary>
    public enum MiniGameResult
    {
        Win = 1,
        Lose = 2,
        Draw = 3,
        Abort = 4
    }

    /// <summary>
    /// 權限類型枚舉
    /// </summary>
    public enum PermissionType
    {
        Read = 1,
        Write = 2,
        Delete = 3,
        Moderate = 4,
        Admin = 5
    }

    /// <summary>
    /// 支付狀態枚舉
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 1,
        Processing = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5,
        Refunded = 6
    }

    /// <summary>
    /// 商品類型枚舉
    /// </summary>
    public enum ProductType
    {
        Game = 1,
        DLC = 2,
        InGameItem = 3,
        Hardware = 4,
        Software = 5,
        Merchandise = 6,
        Digital = 7,
        Physical = 8
    }

    /// <summary>
    /// 論壇類型枚舉
    /// </summary>
    public enum ForumType
    {
        General = 1,
        GameSpecific = 2,
        Technical = 3,
        Trading = 4,
        Social = 5,
        Announcement = 6
    }

    /// <summary>
    /// 反應類型枚舉
    /// </summary>
    public enum ReactionType
    {
        Like = 1,
        Dislike = 2,
        Love = 3,
        Laugh = 4,
        Angry = 5,
        Sad = 6,
        Wow = 7
    }
}