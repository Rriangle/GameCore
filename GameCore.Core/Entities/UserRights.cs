using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 使用者權限表
    /// </summary>
    [Table("User_Rights")]
    public class UserRights
    {
        /// <summary>
        /// 使用者ID (主鍵，外鍵到Users)
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int User_Id { get; set; }

        /// <summary>
        /// 使用者狀態 (true=啟用, false=停權)
        /// </summary>
        public bool User_Status { get; set; } = true;

        /// <summary>
        /// 購物權限 (true=允許, false=禁止)
        /// </summary>
        public bool ShoppingPermission { get; set; } = true;

        /// <summary>
        /// 留言權限 (true=允許, false=禁止)
        /// </summary>
        public bool MessagePermission { get; set; } = true;

        /// <summary>
        /// 銷售權限 (true=允許, false=禁止)
        /// </summary>
        public bool SalesAuthority { get; set; } = false;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 權限變更原因
        /// </summary>
        [StringLength(500)]
        public string? ChangeReason { get; set; }

        /// <summary>
        /// 權限變更者ID (管理員)
        /// </summary>
        public int? ChangedByManagerId { get; set; }

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// 權限變更者 (管理員)
        /// </summary>
        [ForeignKey("ChangedByManagerId")]
        public virtual ManagerData? ChangedByManager { get; set; }
    }
}