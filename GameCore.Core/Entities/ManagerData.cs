using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 管理員資料實體
    /// </summary>
    [Table("manager_data")]
    public class ManagerData
    {
        /// <summary>
        /// 資料編號 (主鍵)
        /// </summary>
        [Key]
        [Column("data_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DataId { get; set; }

        /// <summary>
        /// 管理員編號 (外鍵)
        /// </summary>
        [Required]
        [Column("manager_id")]
        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        /// <summary>
        /// 資料類型
        /// </summary>
        [Required]
        [Column("data_type")]
        [StringLength(50)]
        public string DataType { get; set; } = string.Empty;

        /// <summary>
        /// 資料鍵值
        /// </summary>
        [Required]
        [Column("data_key")]
        [StringLength(100)]
        public string DataKey { get; set; } = string.Empty;

        /// <summary>
        /// 資料值
        /// </summary>
        [Column("data_value")]
        public string DataValue { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual Manager Manager { get; set; } = null!;
    }
} 