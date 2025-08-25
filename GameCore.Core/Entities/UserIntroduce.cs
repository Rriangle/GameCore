using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 使用者介紹表
    /// </summary>
    [Table("User_Introduce")]
    public class UserIntroduce
    {
        /// <summary>
        /// 使用者ID (主鍵，外鍵到Users)
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int User_ID { get; set; }

        /// <summary>
        /// 使用者暱稱 (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string User_NickName { get; set; } = string.Empty;

        /// <summary>
        /// 性別 (必填)
        /// </summary>
        [Required]
        [StringLength(1)]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// 身分證字號 (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string IdNumber { get; set; } = string.Empty;

        /// <summary>
        /// 聯繫電話 (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Cellphone { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件 (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 地址 (必填)
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 出生年月日 (必填)
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// 創建帳號日期 (必填)
        /// </summary>
        [Required]
        public DateTime Create_Account { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 頭像圖片 (可選)
        /// </summary>
        public byte[]? User_Picture { get; set; }

        /// <summary>
        /// 使用者自介 (可選，最大200字)
        /// </summary>
        [StringLength(200)]
        public string? User_Introduce { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}