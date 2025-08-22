using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 使用者基本資料表
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>
        /// 使用者編號 (主鍵)
        /// </summary>
        [Key]
        [Column("User_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        /// <summary>
        /// 使用者姓名 (唯一)
        /// </summary>
        [Required]
        [Column("User_name")]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 登入帳號 (唯一)
        /// </summary>
        [Required]
        [Column("User_Account")]
        [StringLength(100)]
        public string UserAccount { get; set; } = string.Empty;

        /// <summary>
        /// 使用者密碼 (雜湊儲存)
        /// </summary>
        [Required]
        [Column("User_Password")]
        [StringLength(255)]
        public string UserPassword { get; set; } = string.Empty;

        // 導航屬性
        public virtual UserIntroduce? UserIntroduce { get; set; }
        public virtual UserRights? UserRights { get; set; }
        public virtual UserWallet? UserWallet { get; set; }
        public virtual MemberSalesProfile? MemberSalesProfile { get; set; }
        public virtual UserSalesInformation? UserSalesInformation { get; set; }
        public virtual Pet? Pet { get; set; }
        public virtual ICollection<UserSignInStats> SignInStats { get; set; } = new List<UserSignInStats>();
        public virtual ICollection<MiniGame> MiniGames { get; set; } = new List<MiniGame>();
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();
        public virtual ICollection<ThreadPost> ThreadPosts { get; set; } = new List<ThreadPost>();
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<OrderInfo> Orders { get; set; } = new List<OrderInfo>();
        public virtual ICollection<PlayerMarketProductInfo> PlayerMarketProducts { get; set; } = new List<PlayerMarketProductInfo>();
        public virtual ICollection<PlayerMarketOrderInfo> PlayerMarketOrders { get; set; } = new List<PlayerMarketOrderInfo>();
        public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
        public virtual ICollection<NotificationRecipient> ReceivedNotifications { get; set; } = new List<NotificationRecipient>();
        public virtual ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<Group> CreatedGroups { get; set; } = new List<Group>();
        public virtual ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
        public virtual ICollection<GroupChat> GroupChats { get; set; } = new List<GroupChat>();
    }

    /// <summary>
    /// 使用者介紹資料表
    /// </summary>
    [Table("User_Introduce")]
    public class UserIntroduce
    {
        /// <summary>
        /// 使用者編號 (主鍵, 外鍵到 Users)
        /// </summary>
        [Key]
        [Column("User_ID")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 使用者暱稱 (唯一)
        /// </summary>
        [Required]
        [Column("User_NickName")]
        [StringLength(50)]
        public string UserNickName { get; set; } = string.Empty;

        /// <summary>
        /// 性別
        /// </summary>
        [Required]
        [Column("Gender")]
        [StringLength(1)]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// 身分證字號 (唯一)
        /// </summary>
        [Required]
        [Column("IdNumber")]
        [StringLength(20)]
        public string IdNumber { get; set; } = string.Empty;

        /// <summary>
        /// 聯繫電話 (唯一)
        /// </summary>
        [Required]
        [Column("Cellphone")]
        [StringLength(20)]
        public string Cellphone { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件 (唯一)
        /// </summary>
        [Required]
        [Column("Email")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        [Column("Address")]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 出生年月日
        /// </summary>
        [Required]
        [Column("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// 創建帳號日期
        /// </summary>
        [Required]
        [Column("Create_Account")]
        public DateTime CreateAccount { get; set; }

        /// <summary>
        /// 頭像圖片 (二進位資料)
        /// </summary>
        [Column("User_Picture")]
        public byte[]? UserPicture { get; set; }

        /// <summary>
        /// 使用者自介
        /// </summary>
        [Column("User_Introduce")]
        [StringLength(200)]
        public string? UserIntroduceText { get; set; }

        // 導航屬性
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 使用者權限表
    /// </summary>
    [Table("User_Rights")]
    public class UserRights
    {
        /// <summary>
        /// 使用者編號 (主鍵, 外鍵到 Users)
        /// </summary>
        [Key]
        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 使用者狀態 (啟用/停權)
        /// </summary>
        [Column("User_Status")]
        public bool UserStatus { get; set; } = true;

        /// <summary>
        /// 購物權限
        /// </summary>
        [Column("ShoppingPermission")]
        public bool ShoppingPermission { get; set; } = true;

        /// <summary>
        /// 留言權限
        /// </summary>
        [Column("MessagePermission")]
        public bool MessagePermission { get; set; } = true;

        /// <summary>
        /// 銷售權限
        /// </summary>
        [Column("SalesAuthority")]
        public bool SalesAuthority { get; set; } = false;

        // 導航屬性
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 使用者錢包表
    /// </summary>
    [Table("User_wallet")]
    public class UserWallet
    {
        /// <summary>
        /// 使用者編號 (主鍵, 外鍵到 Users)
        /// </summary>
        [Key]
        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 使用者點數
        /// </summary>
        [Column("User_Point")]
        public int UserPoint { get; set; } = 0;

        /// <summary>
        /// 優惠券編號
        /// </summary>
        [Column("Coupon_Number")]
        [StringLength(50)]
        public string? CouponNumber { get; set; }

        // 導航屬性
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 開通銷售功能表
    /// </summary>
    [Table("MemberSalesProfile")]
    public class MemberSalesProfile
    {
        /// <summary>
        /// 使用者編號 (主鍵, 外鍵到 Users)
        /// </summary>
        [Key]
        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 銀行代號
        /// </summary>
        [Column("BankCode")]
        public int? BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Column("BankAccountNumber")]
        [StringLength(50)]
        public string? BankAccountNumber { get; set; }

        /// <summary>
        /// 帳戶封面照片
        /// </summary>
        [Column("AccountCoverPhoto")]
        public byte[]? AccountCoverPhoto { get; set; }

        // 導航屬性
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 使用者銷售資訊表
    /// </summary>
    [Table("User_Sales_Information")]
    public class UserSalesInformation
    {
        /// <summary>
        /// 使用者編號 (主鍵, 外鍵到 Users)
        /// </summary>
        [Key]
        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 使用者銷售錢包
        /// </summary>
        [Column("UserSales_Wallet")]
        public int UserSalesWallet { get; set; } = 0;

        // 導航屬性
        public virtual User User { get; set; } = null!;
    }
}