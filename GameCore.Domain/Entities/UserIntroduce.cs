using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 使用?��?紹實�?
    /// 對�?資�?�?User_Introduce �?
    /// </summary>
    [Table("User_Introduce")]
    public partial class UserIntroduce
    {
        /// <summary>
        /// 使用?�編??(主鍵，�??��???Users.User_ID)
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int User_ID { get; set; }

        /// <summary>
        /// 使用?�暱�?(必填，唯一)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string User_NickName { get; set; } = string.Empty;

        /// <summary>
        /// ?�別 (必填)
        /// </summary>
        [Required]
        [StringLength(1)]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// 身�?證�???(必填，唯一)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string IdNumber { get; set; } = string.Empty;

        /// <summary>
        /// ?�繫?�話 (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Cellphone { get; set; } = string.Empty;

        /// <summary>
        /// ?��??�件 (必填，唯一)
        /// </summary>
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// ?��? (必填)
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// ?��?年�???(必填)
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// ?�建帳�??��? (必填)
        /// </summary>
        [Required]
        public DateTime Create_Account { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?��??��? (二進�?資�?)
        /// </summary>
        public byte[]? User_Picture { get; set; }

        /// <summary>
        /// 使用?�自�?(?��?00�?
        /// </summary>
        [StringLength(200)]
        public string? User_Introduce { get; set; }

        // 導航屬�?
        /// <summary>
        /// ?�聯?�使?��?(一對�??��?)
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
} 
