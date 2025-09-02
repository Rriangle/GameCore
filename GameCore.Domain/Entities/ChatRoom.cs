using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?天室??實?
    /// </summary>
    [Table("chat_rooms")]
    public partial class ChatRoom
    {
        /// <summary>
        /// ?天室編??(主鍵)
        /// </summary>
        [Key]
        [Column("room_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ?天室ID（別???於??層兼容性?
        /// </summary>
        [NotMapped]
        public int RoomId => Id;

        /// <summary>
        /// ?天室??
        /// </summary>
        [Required]
        [Column("room_name")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// ?天室?稱???，用???層?容??
        /// </summary>
        [NotMapped]
        public string RoomName => Name;

        /// <summary>
        /// ?天室???(group, private)
        /// </summary>
        [Required]
        [Column("room_type")]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// ?天室?????，用???層?容??
        /// </summary>
        [NotMapped]
        public string RoomType => Type;

        /// <summary>
        /// ?天室??
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 建??編??(外鍵)
        /// </summary>
        [Required]
        [Column("created_by")]
        [ForeignKey("Creator")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?新??
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?否?用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ?天室ID (?於??層兼容?
        /// </summary>
        [NotMapped]
        public int ChatRoomId => Id;

        /// <summary>
        /// ?天室??(?於??層兼容?
        /// </summary>
        [NotMapped]
        public string ChatRoomName => Name;

        // 導航屬?
        public virtual User Creator { get; set; } = null!;
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
    }

    /// <summary>
    /// ?天室??實?
    /// </summary>
    [Table("chat_room_members")]
    public partial class ChatRoomMember
    {
        /// <summary>
        /// ?員ID (主鍵)
        /// </summary>
        [Key]
        [Column("member_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        /// <summary>
        /// ?天室編??(外鍵)
        /// </summary>
        [Required]
        [Column("room_id")]
        [ForeignKey("ChatRoom")]
        public int RoomId { get; set; }

        /// <summary>
        /// ?天室ID（別???於??層兼容性?
        /// </summary>
        [NotMapped]
        public int ChatRoomId => RoomId;

        /// <summary>
        /// ?戶編? (外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// ?入??
        /// </summary>
        [Column("joined_at")]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?否?管?員
        /// </summary>
        [Column("is_admin")]
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// ?員角色
        /// </summary>
        [Column("role")]
        [StringLength(50)]
        public string Role { get; set; } = "member";

        /// <summary>
        /// ?新??
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬?
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
} 
