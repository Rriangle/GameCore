using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 用戶表 (對應 Users)
    /// </summary>
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Nickname { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Avatar { get; set; }

        [Required]
        public int Level { get; set; } = 1;

        [Required]
        public int Experience { get; set; } = 0;

        [Required]
        public int Points { get; set; } = 0;

        [Required]
        public int Coins { get; set; } = 0;

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime? LastLoginTime { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
        public virtual ICollection<SignInRecord> SignInRecords { get; set; } = new List<SignInRecord>();
        public virtual ICollection<UserSignInStats> SignInStats { get; set; } = new List<UserSignInStats>();
        public virtual ICollection<MiniGameRecord> MiniGameRecords { get; set; } = new List<MiniGameRecord>();
        public virtual ICollection<StoreOrder> StoreOrders { get; set; } = new List<StoreOrder>();
        public virtual ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<PostReply> PostReplies { get; set; } = new List<PostReply>();
        public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public virtual ICollection<PostBookmark> PostBookmarks { get; set; } = new List<PostBookmark>();
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<PrivateMessage> PrivateMessages { get; set; } = new List<PrivateMessage>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<MarketTransaction> MarketTransactions { get; set; } = new List<MarketTransaction>();
        public virtual ICollection<MarketTransaction> MarketTransactionsAsBuyer { get; set; } = new List<MarketTransaction>();
        public virtual ICollection<MarketReview> MarketReviews { get; set; } = new List<MarketReview>();
        public virtual ICollection<MarketReview> MarketReviewsAsReviewee { get; set; } = new List<MarketReview>();
    }
}