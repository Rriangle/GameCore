using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 使用?�基?��??�實�?
    /// 對�?資�?�?Users �?
    /// </summary>
    [Table("Users")]
    public partial class User
    {
        /// <summary>
        /// 使用?�編??(主鍵，自?��?�?
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_ID { get; set; }

        /// <summary>
        /// 使用?��???(必填，唯一)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string User_name { get; set; } = string.Empty;

        /// <summary>
        /// ?�入帳�? (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string User_Account { get; set; } = string.Empty;

        /// <summary>
        /// 使用?��?�?(必填，�?湊儲�?
        /// </summary>
        [Required]
        [StringLength(255)]
        public string User_Password { get; set; } = string.Empty;

        /// <summary>
        /// 建�??��?
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�後更?��???
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�否?�用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ?�戶點數
        /// </summary>
        public int Points { get; set; } = 0;

        /// <summary>
        /// ?�戶經�???
        /// </summary>
        public int Experience { get; set; } = 0;

        // 導航屬�?
        /// <summary>
        /// 使用?��?�?(一對�??��?)
        /// </summary>
        public virtual UserIntroduce? UserIntroduce { get; set; }

        /// <summary>
        /// 使用?��???(一對�??��?)
        /// </summary>
        public virtual UserRights? UserRights { get; set; }

        /// <summary>
        /// 使用?�錢??(一對�??��?)
        /// </summary>
        public virtual UserWallet? UserWallet { get; set; }

        /// <summary>
        /// ?�員?�售檔�? (一對�??��?)
        /// </summary>
        public virtual MemberSalesProfile? MemberSalesProfile { get; set; }

        /// <summary>
        /// 使用?�銷?��?�?(一對�??��?)
        /// </summary>
        public virtual UserSalesInformation? UserSalesInformation { get; set; }

        /// <summary>
        /// 寵物 (一對�??��?)
        /// </summary>
        public virtual Pet? Pet { get; set; }

        /// <summary>
        /// 簽到記�? (一對�??��?)
        /// </summary>
        public virtual ICollection<UserSignInStats> UserSignInStats { get; set; } = new List<UserSignInStats>();

        /// <summary>
        /// 小�??��???(一對�??��?)
        /// </summary>
        public virtual ICollection<MiniGame> MiniGames { get; set; } = new List<MiniGame>();

        /// <summary>
        /// ?�表?�貼??(一對�??��?)
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// ?�表?��?壇主�?(一對�??��?)
        /// </summary>
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();

        /// <summary>
        /// ?�表?��?壇�?�?(一對�??��?)
        /// </summary>
        public virtual ICollection<ThreadPost> ThreadPosts { get; set; } = new List<ThreadPost>();

        /// <summary>
        /// ?��?記�? (一對�??��?)
        /// </summary>
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        /// <summary>
        /// ?��?記�? (一對�??��?)
        /// </summary>
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

        /// <summary>
        /// ?�送�??�知 (一對�??��?)
        /// </summary>
        public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();

        /// <summary>
        /// ?�收?�通知 (一對�??��?)
        /// </summary>
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; } = new List<NotificationRecipient>();

        /// <summary>
        /// ?�送�??�天訊息 (一對�??��?)
        /// </summary>
        public virtual ICollection<ChatMessage> SentChatMessages { get; set; } = new List<ChatMessage>();

        /// <summary>
        /// ?�收?��?天�???(一對�??��?)
        /// </summary>
        public virtual ICollection<ChatMessage> ReceivedChatMessages { get; set; } = new List<ChatMessage>();

        /// <summary>
        /// 建�??�群�?(一對�??��?)
        /// </summary>
        public virtual ICollection<Group> CreatedGroups { get; set; } = new List<Group>();

        /// <summary>
        /// 群�??�員 (一對�??��?)
        /// </summary>
        public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

        /// <summary>
        /// 群�??�天訊息 (一對�??��?)
        /// </summary>
        public virtual ICollection<GroupChat> GroupChats { get; set; } = new List<GroupChat>();

        /// <summary>
        /// 群�?封�?記�? (一對�??��?)
        /// </summary>
        public virtual ICollection<GroupBlock> GroupBlocks { get; set; } = new List<GroupBlock>();

        /// <summary>
        /// 官方?��?訂單 (一對�??��?)
        /// </summary>
        public virtual ICollection<OrderInfo> Orders { get; set; } = new List<OrderInfo>();

        /// <summary>
        /// ?�家市場?��? (一對�??��?)
        /// </summary>
        public virtual ICollection<PlayerMarketProductInfo> PlayerMarketProducts { get; set; } = new List<PlayerMarketProductInfo>();

        /// <summary>
        /// ?�家市場訂單 (買家) (一對�??��?)
        /// </summary>
        public virtual ICollection<PlayerMarketOrderInfo> BuyerOrders { get; set; } = new List<PlayerMarketOrderInfo>();

        /// <summary>
        /// ?�家市場訂單 (�?��) (一對�??��?)
        /// </summary>
        public virtual ICollection<PlayerMarketOrderInfo> SellerOrders { get; set; } = new List<PlayerMarketOrderInfo>();
    }
}
