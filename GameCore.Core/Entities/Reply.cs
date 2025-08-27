using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 論壇回覆實體
    /// </summary>
    [Table("Replies")]
    public class Reply
    {
        /// <summary>
        /// 回覆 ID (主鍵)
        /// </summary>
        [Key]
        [Column("reply_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyId { get; set; }

        /// <summary>
        /// 貼文 ID (外鍵到 Post)
        /// </summary>
        [Column("post_id")]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// 作者 ID (外鍵到 User)
        /// </summary>
        [Column("author_id")]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        /// <summary>
        /// 父回覆 ID (用於巢狀回覆)
        /// </summary>
        [Column("parent_reply_id")]
        [ForeignKey("ParentReply")]
        public int? ParentReplyId { get; set; }

        /// <summary>
        /// 回覆內容
        /// </summary>
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 點讚次數
        /// </summary>
        [Column("like_count")]
        public int LikeCount { get; set; } = 0;

        /// <summary>
        /// 是否為最佳答案
        /// </summary>
        [Column("is_best_answer")]
        public bool IsBestAnswer { get; set; } = false;

        /// <summary>
        /// 創建時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // 導航屬性
        public virtual Post Post { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
        public virtual Reply? ParentReply { get; set; }
        public virtual ICollection<Reply> ChildReplies { get; set; } = new List<Reply>();
    }

    /// <summary>
    /// 論壇貼文 (ThreadPost 的別名)
    /// </summary>
    [Table("ThreadPosts")]
    public class ThreadPost
    {
        /// <summary>
        /// 貼文 ID (主鍵)
        /// </summary>
        [Key]
        [Column("thread_post_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ThreadPostId { get; set; }

        /// <summary>
        /// 論壇 ID (外鍵到 Forum)
        /// </summary>
        [Column("forum_id")]
        [ForeignKey("Forum")]
        public int ForumId { get; set; }

        /// <summary>
        /// 作者 ID (外鍵到 User)
        /// </summary>
        [Column("author_id")]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        /// <summary>
        /// 貼文標題
        /// </summary>
        [Column("title")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 貼文內容
        /// </summary>
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 貼文類型
        /// </summary>
        [Column("post_type")]
        [StringLength(50)]
        public string PostType { get; set; } = "Normal";

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        [Column("view_count")]
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// 回覆次數
        /// </summary>
        [Column("reply_count")]
        public int ReplyCount { get; set; } = 0;

        /// <summary>
        /// 點讚次數
        /// </summary>
        [Column("like_count")]
        public int LikeCount { get; set; } = 0;

        /// <summary>
        /// 創建時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // 導航屬性
        public virtual Forum Forum { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
    }

    /// <summary>
    /// 反應實體
    /// </summary>
    [Table("Reactions")]
    public class Reaction
    {
        /// <summary>
        /// 反應 ID (主鍵)
        /// </summary>
        [Key]
        [Column("reaction_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReactionId { get; set; }

        /// <summary>
        /// 用戶 ID (外鍵到 User)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 目標類型 (Post, Reply, Comment 等)
        /// </summary>
        [Column("target_type")]
        [StringLength(50)]
        public string TargetType { get; set; } = string.Empty;

        /// <summary>
        /// 目標 ID
        /// </summary>
        [Column("target_id")]
        public int TargetId { get; set; }

        /// <summary>
        /// 反應類型 (Like, Dislike, Love, etc.)
        /// </summary>
        [Column("reaction_type")]
        [StringLength(50)]
        public string ReactionType { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual User User { get; set; } = null!;
    }
} 