using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�員?�售資�?實�?
    /// ?�於管�??�員?�銷?��??�申�?
    /// </summary>
    [Table("member_sales_profiles")]
    public partial class MemberSalesProfile
    {
        /// <summary>
        /// ?�戶ID（主?��?外鍵?�Users�?
        /// </summary>
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// ?�行代??
        /// </summary>
        [Column("bank_code")]
        public int BankCode { get; set; }

        /// <summary>
        /// ?�行帳??
        /// </summary>
        [Column("bank_account_number")]
        [StringLength(50)]
        public string BankAccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶封面?��?
        /// </summary>
        [Column("account_cover_photo")]
        public byte[]? AccountCoverPhoto { get; set; }

        // 導航屬�?
        /// <summary>
        /// 對�??�用??
        /// </summary>
        public virtual User? User { get; set; }
    }
} 
