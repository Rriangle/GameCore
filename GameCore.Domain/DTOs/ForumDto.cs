namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 論壇版面 DTO
    /// </summary>
    public class ForumDto
    {
        public int ForumId { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string GameName { get; set; } = string.Empty;
        public int ThreadCount { get; set; }
        public int PostCount { get; set; }
    }

    /// <summary>
    /// 主題 DTO
    /// </summary>
    public class ThreadDto
    {
        public long ThreadId { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; } = string.Empty;
        public int AuthorUserId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ReplyCount { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsBookmarked { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    /// <summary>
    /// 主題建立 DTO
    /// </summary>
    public class ThreadCreateDto
    {
        public int ForumId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }

    /// <summary>
    /// 回覆 DTO
    /// </summary>
    public class ThreadPostDto
    {
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public int AuthorUserId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string ContentMd { get; set; } = string.Empty;
        public string ContentHtml { get; set; } = string.Empty;
        public long? ParentPostId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public List<ThreadPostDto> ChildPosts { get; set; } = new List<ThreadPostDto>();
    }

    /// <summary>
    /// 回覆建立 DTO
    /// </summary>
    public class ThreadPostCreateDto
    {
        public long ThreadId { get; set; }
        public string Content { get; set; } = string.Empty;
        public long? ParentPostId { get; set; }
    }

    /// <summary>
    /// 論壇統計 DTO
    /// </summary>
    public class ForumStatsDto
    {
        public int ForumId { get; set; }
        public string ForumName { get; set; } = string.Empty;
        public int TotalThreads { get; set; }
        public int TotalPosts { get; set; }
        public int TotalUsers { get; set; }
        public DateTime LastActivity { get; set; }
        public List<ThreadDto> RecentThreads { get; set; } = new List<ThreadDto>();
    }

    /// <summary>
    /// 搜尋結果 DTO
    /// </summary>
    public class SearchResultDto
    {
        public string Type { get; set; } = string.Empty; // thread, post, forum
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int RelevanceScore { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
} 
