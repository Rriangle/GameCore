using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 管理者 ↔ 角色 的指派表 (關聯表；多對多)
    /// 一個管理者可有多個角色；一個角色可被多位管理者指派
    /// </summary>
    [Table("ManagerRole")]
    public class ManagerRole
    {
        /// <summary>
        /// 管理者ID (外鍵到ManagerData)
        /// </summary>
        [Required]
        [ForeignKey("ManagerData")]
        public int Manager_Id { get; set; }

        /// <summary>
        /// 角色ID (外鍵到ManagerRolePermission)
        /// </summary>
        [Required]
        [ForeignKey("ManagerRolePermission")]
        public int ManagerRole_Id { get; set; }

        /// <summary>
        /// 角色名稱
        /// </summary>
        [StringLength(100)]
        public string? ManagerRole { get; set; }

        /// <summary>
        /// 指派時間
        /// </summary>
        public DateTime assigned_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 指派者ID
        /// </summary>
        public int? assigned_by { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool is_active { get; set; } = true;

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(500)]
        public string? notes { get; set; }

        // 導航屬性
        /// <summary>
        /// 管理者
        /// </summary>
        public virtual ManagerData ManagerData { get; set; } = null!;

        /// <summary>
        /// 角色權限
        /// </summary>
        public virtual ManagerRolePermission ManagerRolePermission { get; set; } = null!;

        /// <summary>
        /// 指派者
        /// </summary>
        [ForeignKey("assigned_by")]
        public virtual ManagerData? AssignedBy { get; set; }
    }
}