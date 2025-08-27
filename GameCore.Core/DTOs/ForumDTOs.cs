using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    public class ForumDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int PostCount { get; set; }
        public DateTime? LastPostDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PostCreateDto
    {
        [Required]
        public int ForumId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(10000)]
        public string Content { get; set; } = string.Empty;
    }

    public class PostDto
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ReplyCreateDto
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [StringLength(5000)]
        public string Content { get; set; } = string.Empty;

        public int? ParentReplyId { get; set; }
    }

    public class ReplyDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? ParentReplyId { get; set; }
        public int LikeCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 論壇統計 DTO
    /// </summary>
    public class ForumStatsDto
    {
        public int ForumId { get; set; }
        public int TotalPosts { get; set; }
        public int TotalReplies { get; set; }
        public int TotalViews { get; set; }
        public DateTime? LastActivity { get; set; }
    }
} 