using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameCore.Domain.Enums;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 管�??��??�實�?
    /// ?�於管�?系統管�???
    /// </summary>
    [Table("manager_data")]
    public partial class ManagerData
    {
        /// <summary>
        /// 管�??�ID（主?��?
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// ?�戶??
        /// </summary>
        [Required]
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// ?��??�件
        /// </summary>
        [Required]
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼?��?
        /// </summary>
        [Required]
        [Column("password_hash")]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// 管�??��???
        /// </summary>
        [Column("role")]
        [StringLength(50)]
        public string Role { get; set; } = "admin";

        /// <summary>
        /// ?�否?�管?�員（別?��??�於?��?層兼容性�?
        /// </summary>
        [NotMapped]
        public bool IsAdmin => Role?.ToLower() == "admin";

        /// <summary>
        /// ?�否?�用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ?�後登?��???
        /// </summary>
        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 
