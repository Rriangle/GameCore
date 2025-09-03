using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    // Ê≥®Ê?Ôºö‰ª•‰∏ãÁÇ∫?∏ÂÆπ?ßÊö´?ÇÈÅ©?çÔ??Ö‰?Á∑®Ë≠Ø?öÈ?Ôºå‰??πË?Ë≥áÊ?Â∫?Schema

    public partial class Forum
    {
        [NotMapped]
        public string Category { get; set; } = string.Empty;

        [NotMapped]
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        [NotMapped]
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public int CategoryId { get; set; }

        [NotMapped]
        public int Order { get; set; }

        [NotMapped]
        public DateTime? LastActivityAt { get; set; }

        [NotMapped]
        public int Id { get; set; }

        [NotMapped]
        public int PostCount { get; set; }

        [NotMapped]
        public int ViewCount { get; set; }

        [NotMapped]
        public string RequiredPermission { get; set; } = string.Empty;

        [NotMapped]
        public string ModerationStatus { get; set; } = string.Empty;

        [NotMapped]
        public string Language { get; set; } = string.Empty;

        [NotMapped]
        public string Country { get; set; } = string.Empty;

        [NotMapped]
        public int MinAge { get; set; }

        [NotMapped]
        public bool RequiresSubscription { get; set; }

        [NotMapped]
        public bool IsFeatured { get; set; }

        [NotMapped]
        public int PeakActivityHour { get; set; }

        [NotMapped]
        public string SeasonalTheme { get; set; } = string.Empty;

        [NotMapped]
        public string EventName { get; set; } = string.Empty;
    }

    public partial class Game
    {
        [NotMapped]
        public string Category { get; set; } = string.Empty;

        [NotMapped]
        public string Description { get; set; } = string.Empty;

        [NotMapped]
        public int PlayCount { get; set; }
    }

    public partial class Post
    {
        [NotMapped]
        public int Id { get => PostId; set => PostId = value; }

        [NotMapped]
        public int Likes { get => LikeCount; set => LikeCount = value; }

        [NotMapped]
        public GameCore.Domain.Enums.PostStatus Status { get; set; } = GameCore.Domain.Enums.PostStatus.Published;

        [NotMapped]
        public DateTime? LastActivityAt { get => UpdatedAt ?? CreatedAt; set => UpdatedAt = value; }

        [NotMapped]
        public PostMetricSnapshot? PostMetricSnapshot { get; set; }

        [NotMapped]
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

        [NotMapped]
        public int PinOrder { get; set; }

        [NotMapped]
        public bool IsSticky { get; set; }

        [NotMapped]
        public string Tags { get; set; } = string.Empty;

        [NotMapped]
        public string ModerationStatus { get; set; } = string.Empty;

        [NotMapped]
        public int ReportCount { get; set; }

        [NotMapped]
        public string Language { get; set; } = string.Empty;

        [NotMapped]
        public string Country { get; set; } = string.Empty;

        [NotMapped]
        public int MinAge { get; set; }

        [NotMapped]
        public bool RequiresSubscription { get; set; }

        [NotMapped]
        public int Order { get; set; }

        [NotMapped]
        public int PeakActivityHour { get; set; }

        [NotMapped]
        public string SeasonalTheme { get; set; } = string.Empty;

        [NotMapped]
        public string EventName { get; set; } = string.Empty;

        [NotMapped]
        public int StickyOrder { get; set; }

        [NotMapped]
        public int Count { get; set; }
    }

    public partial class PostReply
    {
        [NotMapped]
        public int Id { get => ReplyId; set => ReplyId = value; }

        [NotMapped]
        public GameCore.Domain.Enums.PostStatus Status { get; set; } = GameCore.Domain.Enums.PostStatus.Published;

        [NotMapped]
        public DateTime? LastActivityAt { get => UpdatedAt; set => UpdatedAt = value ?? DateTime.UtcNow; }

        [NotMapped]
        public string Path { get; set; } = string.Empty;

        [NotMapped]
        public User Author { get => Replier; set => Replier = value; }

        [NotMapped]
        public int AuthorId { get => ReplierId; set => ReplierId = value; }

        [NotMapped]
        public int Likes { get; set; }

        [NotMapped]
        public int ReportCount { get; set; }

        [NotMapped]
        public string ModerationStatus { get; set; } = string.Empty;

        [NotMapped]
        public string Language { get; set; } = string.Empty;

        [NotMapped]
        public string Country { get; set; } = string.Empty;

        [NotMapped]
        public int MinAge { get; set; }

        [NotMapped]
        public bool RequiresSubscription { get; set; }

        [NotMapped]
        public int Order { get; set; }

        [NotMapped]
        public int PeakActivityHour { get; set; }

        [NotMapped]
        public string SeasonalTheme { get; set; } = string.Empty;

        [NotMapped]
        public string EventName { get; set; } = string.Empty;

        [NotMapped]
        public int Depth { get; set; }

        [NotMapped]
        public bool IsFeatured { get; set; }
    }

    public partial class Pet
    {
        [NotMapped]
        public int OwnerId { get => UserId; set => UserId = value; }

        [NotMapped]
        public User Owner { get; set; } = null!;

        [NotMapped]
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public DateTime? LastFeedTime { get; set; }

        [NotMapped]
        public DateTime? LastCleanTime { get; set; }

        [NotMapped]
        public DateTime? LastPlayTime { get; set; }

        [NotMapped]
        public DateTime? LastRestTime { get; set; }

        [NotMapped]
        public DateTime? LastInteractionTime { get; set; }

        [NotMapped]
        public string EyeColor { get; set; } = string.Empty;

        [NotMapped]
        public string Species { get; set; } = string.Empty;

        [NotMapped]
        public int Age { get; set; }
    }

    public partial class PlayerMarketProductInfo
    {
        [NotMapped]
        public string PStatus { get => Status; set => Status = value; }
    }

    public partial class UserWallet
    {
        [NotMapped]
        public int UserId { get; set; }
    }

    public partial class UserIntroduce
    {
        [NotMapped]
        public string UserNickName { get; set; } = string.Empty;

        [NotMapped]
        public int UserId { get; set; }

        [NotMapped]
        public DateTime CreateAccount { get; set; } = DateTime.UtcNow;
    }

    public partial class ManagerData
    {
        [NotMapped]
        public Manager Manager { get; set; } = new Manager();

        [NotMapped]
        public int ManagerId { get; set; }

        [NotMapped]
        public string DataType { get; set; } = string.Empty;

        [NotMapped]
        public string Key { get; set; } = string.Empty;

        [NotMapped]
        public string Value { get; set; } = string.Empty;
    }

    public partial class ManagerRolePermission
    {
        [NotMapped]
        public ManagerRole Role { get; set; } = new ManagerRole();

        [NotMapped]
        public string Permission { get; set; } = string.Empty;
    }

    public partial class ManagerRole
    {
        [NotMapped]
        public ICollection<ManagerRolePermission> RolePermissions { get; set; } = new List<ManagerRolePermission>();

        [NotMapped]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public int Id { get; set; }
    }

    public partial class NotificationSource
    {
        [NotMapped]
        public string Name { get; set; } = string.Empty;

        [NotMapped]
        public string SourceType { get; set; } = string.Empty;
    }

    public partial class NotificationAction
    {
        [NotMapped]
        public string Name { get; set; } = string.Empty;

        [NotMapped]
        public string ActionType { get; set; } = string.Empty;
    }

    public partial class PlayerMarketOrderInfo
    {
        [NotMapped]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string Status { get; set; } = string.Empty;

        [NotMapped]
        public Product Product { get; set; } = null!;
    }

    public partial class MarketTransaction
    {
        [NotMapped]
        public MarketItem Item { get; set; } = new MarketItem();

        [NotMapped]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public ICollection<MarketReview> Reviews { get; set; } = new List<MarketReview>();
    }

    public partial class MiniGameRecord
    {
        [NotMapped]
        public Game Game { get; set; } = new Game();

        [NotMapped]
        public int GameId { get; set; }
    }

    public partial class MemberSalesProfile
    {
        [NotMapped]
        public string Status { get; set; } = string.Empty;

        [NotMapped]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public partial class UserSalesInformation
    {
        [NotMapped]
        public int Id { get => UserId; set => UserId = value; }

        [NotMapped]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string Status { get; set; } = string.Empty;

        [NotMapped]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public partial class UserRights
    {
        [NotMapped]
        public string Name { get; set; } = string.Empty;

        [NotMapped]
        public string RightType { get; set; } = string.Empty;

        [NotMapped]
        public string User_Role { get; set; } = string.Empty;

        [NotMapped]
        public int UserId { get; set; }
    }

    public partial class User
    {
        [NotMapped]
        public string User_Status { get; set; } = string.Empty;

        [NotMapped]
        public DateTime User_CreatedAt { get => CreatedAt; set => CreatedAt = value; }

        [NotMapped]
        public ICollection<Pet> Pets { get; set; } = new List<Pet>();

        [NotMapped]
        public string Username { get => User_Account; set => User_Account = value; }

        [NotMapped]
        public ICollection<PlayerMarketOrderInfo> PlayerMarketOrders { get; set; } = new List<PlayerMarketOrderInfo>();

        [NotMapped]
        public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();

        [NotMapped]
        public ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();
    }

    public partial class ChatMessage
    {
        [NotMapped]
        public User User { get; set; } = null!;

        [NotMapped]
        public DateTime CreateTime { get => CreatedAt; set => CreatedAt = value; }

        [NotMapped]
        public int UserId { get => SenderId; set => SenderId = value; }

        [NotMapped]
        public int ReceiverId { get; set; }

        [NotMapped]
        public User SenderUser { get; set; } = null!;

        [NotMapped]
        public User ReceiverUser { get; set; } = null!;
    }

    public partial class ChatRoom
    {
        [NotMapped]
        public DateTime UpdateTime { get => UpdatedAt; set => UpdatedAt = value; }
    }

    public partial class ChatRoomMember
    {
        [NotMapped]
        public DateTime LastReadTime { get; set; } = DateTime.UtcNow;
    }

    public partial class PrivateChat
    {
        [NotMapped]
        public User User1 { get; set; } = null!;

        [NotMapped]
        public User User2 { get; set; } = null!;

        [NotMapped]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
    }

    public partial class PrivateMessage
    {
        [NotMapped]
        public DateTime CreateTime { get => CreatedAt; set => CreatedAt = value; }

        [NotMapped]
        public DateTime? ReadTime { get => ReadAt; set => ReadAt = value; }

        [NotMapped]
        public User Receiver { get; set; } = null!;
    }

    public partial class GameSettings
    {
        [NotMapped]
        public string Key { get; set; } = string.Empty;

        [NotMapped]
        public string Value { get; set; } = string.Empty;

        [NotMapped]
        public int GameLevel { get; set; }

        [NotMapped]
        public int MaxMonsters { get; set; }

        [NotMapped]
        public double SpeedMultiplier { get; set; }
    }

    public partial class Notification
    {
        [NotMapped]
        public string Content { get; set; } = string.Empty;

        [NotMapped]
        public DateTime CreateTime { get => CreatedAt; set => CreatedAt = value; }

        [NotMapped]
        public DateTime? ReadTime { get; set; }

        [NotMapped]
        public NotificationSource NotificationSource { get; set; } = null!;

        [NotMapped]
        public NotificationAction NotificationAction { get; set; } = null!;
    }

    public partial class Product
    {
        [NotMapped]
        public int ProductId { get => Id; set => Id = value; }

        [NotMapped]
        public string ProductType { get; set; } = string.Empty;

        [NotMapped]
        public string ProductName { get => Name; set => Name = value; }

        [NotMapped]
        public int SalesCount { get; set; }
    }

    public partial class MarketItem
    {
        [NotMapped]
        public int Id { get; set; }

        [NotMapped]
        public string Category { get; set; } = string.Empty;

        [NotMapped]
        public string Status { get; set; } = string.Empty;

        [NotMapped]
        public string Name { get => PProductName; set => PProductName = value; }

        [NotMapped]
        public string Description { get => PProductDescription; set => PProductDescription = value; }

        [NotMapped]
        public ICollection<string> Images { get; set; } = new List<string>();

        [NotMapped]
        public int ViewCount { get; set; }
    }

    public partial class Reply
    {
        [NotMapped]
        public int AuthorId { get; set; }

        [NotMapped]
        public int Likes { get; set; }
    }

    public partial class ManagerRole
    {
        [NotMapped]
        public string Name { get; set; } = string.Empty;
    }

    public partial class Manager
    {
        [NotMapped]
        public string Code { get; set; } = string.Empty;
    }
} 
