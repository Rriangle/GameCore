using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�戶?�售資�?實�?
    /// ?�於管�??�戶?�銷?�錢??
    /// </summary>
    [Table("user_sales_information")]
    public partial class UserSalesInformation
    {
        /// <summary>
        /// ?�戶ID（主?��?外鍵?�Users�?
        /// </summary>
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// ?�戶?�售?��?餘�?
        /// </summary>
        [Column("user_sales_wallet")]
        public int UserSalesWallet { get; set; } = 0;

        // 導航屬�?
        /// <summary>
        /// 對�??�用??
        /// </summary>
        public virtual User? User { get; set; }
    }
} 
