using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 聊天室實體
    /// </summary>
    [Table("chat_rooms")]
    public class ChatRoom
    {
        /// <summary>
        /// 聊天室編號 (主鍵)
        /// </summary>
        [Key]
        [Column("room_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }

        /// <summary>
        /// 聊天室名稱
        /// </summary>
        [Required]
        [Column("room_name")]
        [StringLength(100)]
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// 聊天室類型 (group, private)
        /// </summary>
        [Required]
        [Column("room_type")]
        [StringLength(20)]
        public string RoomType { get; set; } = string.Empty;

        /// <summary>
        /// 聊天室描述
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 建立者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("created_by")]
        [ForeignKey("Creator")]
        public int CreatedBy { get; set; }

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

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        // 導航屬性
        public virtual User Creator { get; set; } = null!;
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
    }

    /// <summary>
    /// 聊天室成員實體
    /// </summary>
    [Table("chat_room_members")]
    public class ChatRoomMember
    {
        /// <summary>
        /// 成員編號 (主鍵)
        /// </summary>
        [Key]
        [Column("member_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        /// <summary>
        /// 聊天室編號 (外鍵)
        /// </summary>
        [Required]
        [Column("room_id")]
        [ForeignKey("ChatRoom")]
        public int RoomId { get; set; }

        /// <summary>
        /// 使用者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 成員角色
        /// </summary>
        [Required]
        [Column("role")]
        public ChatMemberRole Role { get; set; } = ChatMemberRole.Member;

        /// <summary>
        /// 加入時間
        /// </summary>
        [Column("joined_at")]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後上線時間
        /// </summary>
        [Column("last_seen_at")]
        public DateTime? LastSeenAt { get; set; }

        /// <summary>
        /// 是否靜音
        /// </summary>
        [Column("is_muted")]
        public bool IsMuted { get; set; } = false;

        // 導航屬性
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 聊天成員角色列舉
    /// </summary>
    public enum ChatMemberRole
    {
        Owner,
        Admin,
        Moderator,
        Member
    }
} 