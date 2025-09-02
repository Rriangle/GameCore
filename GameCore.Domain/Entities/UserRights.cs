using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 使用?��??�實�?
    /// 對�?資�?�?User_Rights �?
    /// </summary>
    [Table("User_Rights")]
    public partial class UserRights
    {
        /// <summary>
        /// 使用?�編??(主鍵，�??��???Users.User_ID)
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int User_Id { get; set; }

        /// <summary>
        /// 使用?��???(true=?�用, false=?��?)
        /// </summary>
        public bool User_Status { get; set; } = true;

        /// <summary>
        /// 購物權�? (true=?�許購物, false=禁止購物)
        /// </summary>
        public bool ShoppingPermission { get; set; } = true;

        /// <summary>
        /// ?��?權�? (true=?�許?��?, false=禁止?��?)
        /// </summary>
        public bool MessagePermission { get; set; } = true;

        /// <summary>
        /// ?�售權�? (true=?�許?�售, false=禁止?�售)
        /// </summary>
        public bool SalesAuthority { get; set; } = false;

        // 導航屬�?
        /// <summary>
        /// ?�聯?�使?��?(一對�??��?)
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
} 
