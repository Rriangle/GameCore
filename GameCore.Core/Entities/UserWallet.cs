using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 使用者錢包表
    /// </summary>
    [Table("User_wallet")]
    public class UserWallet
    {
        /// <summary>
        /// 使用者ID (主鍵，外鍵到Users)
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int User_Id { get; set; }

        /// <summary>
        /// 使用者點數餘額
        /// </summary>
        public int User_Point { get; set; } = 0;

        /// <summary>
        /// 優惠券編號 (可選)
        /// </summary>
        [StringLength(50)]
        public string? Coupon_Number { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後點數變動時間
        /// </summary>
        public DateTime LastPointsChange { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 點數變動原因
        /// </summary>
        [StringLength(200)]
        public string? LastChangeReason { get; set; }

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}