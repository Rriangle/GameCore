using System.ComponentModel;

namespace GameCore.Core.Enums
{
    /// <summary>
    /// 貼文狀態
    /// </summary>
    public enum PostStatus
    {
        [Description("草稿")]
        Draft = 0,
        [Description("已發布")]
        Published = 1,
        [Description("已隱藏")]
        Hidden = 2,
        [Description("已刪除")]
        Deleted = 3
    }

    /// <summary>
    /// 貼文回覆狀態
    /// </summary>
    public enum PostReplyStatus
    {
        [Description("正常")]
        Normal = 0,
        [Description("已隱藏")]
        Hidden = 1,
        [Description("已刪除")]
        Deleted = 2
    }

    /// <summary>
    /// 訂單狀態
    /// </summary>
    public enum OrderStatus
    {
        [Description("待付款")]
        Pending = 0,
        [Description("已付款")]
        Paid = 1,
        [Description("處理中")]
        Processing = 2,
        [Description("已發貨")]
        Shipped = 3,
        [Description("已完成")]
        Completed = 4,
        [Description("已取消")]
        Cancelled = 5,
        [Description("已退款")]
        Refunded = 6
    }

    /// <summary>
    /// 市場商品狀態
    /// </summary>
    public enum MarketItemStatus
    {
        [Description("上架中")]
        Active = 0,
        [Description("已售出")]
        Sold = 1,
        [Description("已下架")]
        Inactive = 2,
        [Description("已過期")]
        Expired = 3,
        [Description("已刪除")]
        Deleted = 4
    }

    /// <summary>
    /// 交易狀態
    /// </summary>
    public enum TransactionStatus
    {
        [Description("待付款")]
        Pending = 0,
        [Description("已付款")]
        Paid = 1,
        [Description("已完成")]
        Completed = 2,
        [Description("已取消")]
        Cancelled = 3,
        [Description("已退款")]
        Refunded = 4
    }

    /// <summary>
    /// 通知類型
    /// </summary>
    public enum NotificationType
    {
        [Description("系統通知")]
        System = 0,
        [Description("遊戲通知")]
        Game = 1,
        [Description("交易通知")]
        Transaction = 2,
        [Description("社交通知")]
        Social = 3,
        [Description("活動通知")]
        Event = 4
    }

    /// <summary>
    /// 寵物心情
    /// </summary>
    public enum PetMood
    {
        [Description("非常開心")]
        VeryHappy = 0,
        [Description("開心")]
        Happy = 1,
        [Description("普通")]
        Normal = 2,
        [Description("不開心")]
        Unhappy = 3,
        [Description("非常不開心")]
        VeryUnhappy = 4
    }

    /// <summary>
    /// 寵物互動類型
    /// </summary>
    public enum PetInteractionType
    {
        [Description("餵食")]
        Feed = 0,
        [Description("洗澡")]
        Bath = 1,
        [Description("玩耍")]
        Play = 2,
        [Description("休息")]
        Rest = 3
    }

    /// <summary>
    /// 用戶角色
    /// </summary>
    public enum UserRole
    {
        [Description("一般用戶")]
        User = 0,
        [Description("VIP用戶")]
        VIP = 1,
        [Description("管理員")]
        Admin = 2
    }

    /// <summary>
    /// 管理員角色
    /// </summary>
    public enum ManagerRole
    {
        [Description("一般管理員")]
        Manager = 0,
        [Description("高級管理員")]
        SeniorManager = 1,
        [Description("超級管理員")]
        SuperAdmin = 2
    }
}