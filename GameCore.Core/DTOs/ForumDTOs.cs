using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 論壇 DTO
    /// </summary>
    public class ForumDto
    {
        public int ForumId { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int PostCount { get; set; }
        public int ReplyCount { get; set; }
        public DateTime? LastPostAt { get; set; }
        public string? LastPostTitle { get; set; }
        public string? LastPostAuthor { get; set; }
    }

    /// <summary>
    /// 貼文詳細結果 DTO
    /// </summary>
    public class PostDetailResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PostDto? Post { get; set; }
        public List<ReplyDto> Replies { get; set; } = new();
    }

    /// <summary>
    /// 貼文 DTO
    /// </summary>
    public class PostDto
    {
        public int PostId { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsPinned { get; set; }
        public List<string> Tags { get; set; } = new();
    }

    /// <summary>
    /// 貼文創建請求 DTO
    /// </summary>
    public class PostCreateDto
    {
        [Required(ErrorMessage = "標題不能為空")]
        [StringLength(200, ErrorMessage = "標題長度不能超過200個字符")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "內容不能為空")]
        [StringLength(10000, ErrorMessage = "內容長度不能超過10000個字符")]
        public string Content { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new();
    }

    /// <summary>
    /// 貼文更新請求 DTO
    /// </summary>
    public class PostUpdateDto
    {
        [StringLength(200, ErrorMessage = "標題長度不能超過200個字符")]
        public string? Title { get; set; }

        [StringLength(10000, ErrorMessage = "內容長度不能超過10000個字符")]
        public string? Content { get; set; }

        public List<string>? Tags { get; set; }
    }

    /// <summary>
    /// 貼文更新結果 DTO
    /// </summary>
    public class PostUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PostDto? Post { get; set; }
    }

    /// <summary>
    /// 貼文刪除結果 DTO
    /// </summary>
    public class PostDeleteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 回覆 DTO
    /// </summary>
    public class ReplyDto
    {
        public int ReplyId { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? ParentReplyId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public List<ReplyDto> ChildReplies { get; set; } = new();
    }

    /// <summary>
    /// 回覆列表結果 DTO
    /// </summary>
    public class ReplyListResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<ReplyDto> Replies { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 回覆創建請求 DTO
    /// </summary>
    public class ReplyCreateDto
    {
        [Required(ErrorMessage = "內容不能為空")]
        [StringLength(5000, ErrorMessage = "內容長度不能超過5000個字符")]
        public string Content { get; set; } = string.Empty;

        public int? ParentReplyId { get; set; }
    }

    /// <summary>
    /// 回覆更新請求 DTO
    /// </summary>
    public class ReplyUpdateDto
    {
        [Required(ErrorMessage = "內容不能為空")]
        [StringLength(5000, ErrorMessage = "內容長度不能超過5000個字符")]
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 回覆更新結果 DTO
    /// </summary>
    public class ReplyUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ReplyDto? Reply { get; set; }
    }

    /// <summary>
    /// 回覆刪除結果 DTO
    /// </summary>
    public class ReplyDeleteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 搜尋結果 DTO
    /// </summary>
    public class SearchResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PostDto> Posts { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 貼文創建（簡化版）
    /// </summary>
    public class PostCreate
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }

    /// <summary>
    /// 回覆創建（簡化版）
    /// </summary>
    public class ReplyCreate
    {
        public string Content { get; set; } = string.Empty;
        public int? ParentReplyId { get; set; }
    }
}