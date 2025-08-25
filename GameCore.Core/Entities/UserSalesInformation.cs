using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 使用者銷售資訊表
    /// </summary>
    [Table("User_Sales_Information")]
    public class UserSalesInformation
    {
        /// <summary>
        /// 使用者ID (主鍵，外鍵到Users)
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int User_Id { get; set; }

        /// <summary>
        /// 使用者銷售錢包
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal UserSales_Wallet { get; set; } = 0;

        /// <summary>
        /// 總銷售額
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSales { get; set; } = 0;

        /// <summary>
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; } = 0;

        /// <summary>
        /// 成功訂單數
        /// </summary>
        public int SuccessfulOrders { get; set; } = 0;

        /// <summary>
        /// 取消訂單數
        /// </summary>
        public int CancelledOrders { get; set; } = 0;

        /// <summary>
        /// 退款訂單數
        /// </summary>
        public int RefundedOrders { get; set; } = 0;

        /// <summary>
        /// 平均評分
        /// </summary>
        [Column(TypeName = "decimal(3,2)")]
        public decimal AverageRating { get; set; } = 0;

        /// <summary>
        /// 評分數量
        /// </summary>
        public int RatingCount { get; set; } = 0;

        /// <summary>
        /// 最後銷售時間
        /// </summary>
        public DateTime? LastSaleAt { get; set; }

        /// <summary>
        /// 銷售權限狀態
        /// </summary>
        [StringLength(50)]
        public string SalesStatus { get; set; } = "inactive";

        /// <summary>
        /// 銷售等級
        /// </summary>
        [StringLength(50)]
        public string SalesLevel { get; set; } = "bronze";

        /// <summary>
        /// 銷售徽章
        /// </summary>
        [StringLength(500)]
        public string? SalesBadges { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}