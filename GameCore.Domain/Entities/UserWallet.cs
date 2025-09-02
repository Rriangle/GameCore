using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 使用?�錢?�實�?
    /// 對�?資�?�?User_wallet �?
    /// </summary>
    [Table("User_wallet")]
    public partial class UserWallet
    {
        /// <summary>
        /// 使用?�編??(主鍵，�??��???Users.User_ID)
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int User_Id { get; set; }

        /// <summary>
        /// 使用?��??��?�?
        /// </summary>
        public int User_Point { get; set; } = 0;

        /// <summary>
        /// ?��??�編??
        /// </summary>
        [StringLength(50)]
        public string? Coupon_Number { get; set; }

        /// <summary>
        /// ?��??�編??
        /// </summary>
        [StringLength(50)]
        public string? CouponNumber { get; set; }

        // 導航屬�?
        /// <summary>
        /// ?�聯?�使?��?(一對�??��?)
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
} 
