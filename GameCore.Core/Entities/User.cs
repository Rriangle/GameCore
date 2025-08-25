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
        /// 使用者編號 (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_ID { get; set; }

        /// <summary>
        /// 使用者姓名 (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string User_name { get; set; } = string.Empty;

        /// <summary>
        /// 登入帳號 (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string User_Account { get; set; } = string.Empty;

        /// <summary>
        /// 使用者密碼 (必填，雜湊儲存)
        /// </summary>
        [Required]
        [StringLength(255)]
        public string User_Password { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; } = true;

        // 導航屬性
        /// <summary>
        /// 使用者介紹
        /// </summary>
        public virtual UserIntroduce? UserIntroduce { get; set; }

        /// <summary>
        /// 使用者權限
        /// </summary>
        public virtual UserRights? UserRights { get; set; }

        /// <summary>
        /// 使用者錢包
        /// </summary>
        public virtual UserWallet? UserWallet { get; set; }

        /// <summary>
        /// 銷售資料
        /// </summary>
        public virtual MemberSalesProfile? MemberSalesProfile { get; set; }

        /// <summary>
        /// 銷售錢包
        /// </summary>
        public virtual UserSalesInformation? UserSalesInformation { get; set; }

        /// <summary>
        /// 寵物
        /// </summary>
        public virtual Pet? Pet { get; set; }

        /// <summary>
        /// 簽到記錄
        /// </summary>
        public virtual ICollection<UserSignInStats> UserSignInStats { get; set; } = new List<UserSignInStats>();

        /// <summary>
        /// 小遊戲記錄
        /// </summary>
        public virtual ICollection<MiniGame> MiniGames { get; set; } = new List<MiniGame>();

        /// <summary>
        /// 發表的文章
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// 發表的論壇主題
        /// </summary>
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();

        /// <summary>
        /// 論壇回覆
        /// </summary>
        public virtual ICollection<ThreadPost> ThreadPosts { get; set; } = new List<ThreadPost>();

        /// <summary>
        /// 反應記錄
        /// </summary>
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        /// <summary>
        /// 收藏記錄
        /// </summary>
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

        /// <summary>
        /// 通知
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        /// <summary>
        /// 通知接收者
        /// </summary>
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; } = new List<NotificationRecipient>();

        /// <summary>
        /// 聊天訊息
        /// </summary>
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        /// <summary>
        /// 群組成員
        /// </summary>
        public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

        /// <summary>
        /// 群組聊天
        /// </summary>
        public virtual ICollection<GroupChat> GroupChats { get; set; } = new List<GroupChat>();

        /// <summary>
        /// 群組封鎖
        /// </summary>
        public virtual ICollection<GroupBlock> GroupBlocks { get; set; } = new List<GroupBlock>();

        /// <summary>
        /// 官方商城訂單
        /// </summary>
        public virtual ICollection<OrderInfo> OrderInfos { get; set; } = new List<OrderInfo>();

        /// <summary>
        /// 自由市場商品
        /// </summary>
        public virtual ICollection<PlayerMarketProductInfo> PlayerMarketProducts { get; set; } = new List<PlayerMarketProductInfo>();

        /// <summary>
        /// 自由市場訂單 (買家)
        /// </summary>
        public virtual ICollection<PlayerMarketOrderInfo> BuyerOrders { get; set; } = new List<PlayerMarketOrderInfo>();

        /// <summary>
        /// 自由市場訂單 (賣家)
        /// </summary>
        public virtual ICollection<PlayerMarketOrderInfo> SellerOrders { get; set; } = new List<PlayerMarketOrderInfo>();
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