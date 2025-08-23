using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(GameCoreDbContext context, ILogger<PostRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetByForumIdAsync(int forumId, int page = 1, int pageSize = 50)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.ForumId == forumId && p.Status == PostStatus.Active)
                .OrderByDescending(p => p.IsPinned)
                .ThenByDescending(p => p.LastActivityAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetByAuthorIdAsync(int authorId, int page = 1, int pageSize = 50)
        {
            return await _context.Posts
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.AuthorId == authorId && p.Status == PostStatus.Active)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetByStatusAsync(PostStatus status, int page = 1, int pageSize = 50)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Post> AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            return post;
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
        }

        public async Task DeleteAsync(int id)
        {
            var post = await GetByIdAsync(id);
            if (post != null)
            {
                post.Status = PostStatus.Deleted;
                _context.Posts.Update(post);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Posts.AnyAsync(p => p.Id == id);
        }

        public async Task<int> GetPostCountAsync()
        {
            return await _context.Posts.CountAsync(p => p.Status == PostStatus.Active);
        }

        public async Task<int> GetPostCountByForumAsync(int forumId)
        {
            return await _context.Posts.CountAsync(p => 
                p.ForumId == forumId && p.Status == PostStatus.Active);
        }

        public async Task<int> GetPostCountByAuthorAsync(int authorId)
        {
            return await _context.Posts.CountAsync(p => 
                p.AuthorId == authorId && p.Status == PostStatus.Active);
        }

        public async Task<int> GetPostCountByStatusAsync(PostStatus status)
        {
            return await _context.Posts.CountAsync(p => p.Status == status);
        }

        public async Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm, int page = 1, int pageSize = 50)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           (p.Title.Contains(searchTerm) || 
                            p.Content.Contains(searchTerm) ||
                            p.Author.Username.Contains(searchTerm) ||
                            p.Forum.Name.Contains(searchTerm)))
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByLastActivityAsync(DateTime lastActivity)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.LastActivityAt >= lastActivity)
                .OrderByDescending(p => p.LastActivityAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByViewCountAsync(int minViewCount)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.ViewCount >= minViewCount)
                .OrderByDescending(p => p.ViewCount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByLikeCountAsync(int minLikeCount)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.Likes.Count >= minLikeCount)
                .OrderByDescending(p => p.Likes.Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByReplyCountAsync(int minReplyCount)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.Replies.Count >= minReplyCount)
                .OrderByDescending(p => p.Replies.Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPinnedPostsAsync(int forumId)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.ForumId == forumId && 
                           p.Status == PostStatus.Active && 
                           p.IsPinned)
                .OrderBy(p => p.PinOrder)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetStickyPostsAsync(int forumId)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.ForumId == forumId && 
                           p.Status == PostStatus.Active && 
                           p.IsSticky)
                .OrderBy(p => p.StickyOrder)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByTagAsync(string tag)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.Tags.Contains(tag))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByCategoryAsync(int categoryId)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.Forum.CategoryId == categoryId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post> GetLatestPostByForumAsync(int forumId)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.ForumId == forumId && p.Status == PostStatus.Active)
                .OrderByDescending(p => p.LastActivityAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Post>> GetRecentPostsAsync(int count = 10)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetTrendingPostsAsync(int days = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.CreatedAt >= cutoffDate)
                .OrderByDescending(p => p.ViewCount + p.Likes.Count + p.Replies.Count)
                .Take(20)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByModerationStatusAsync(string moderationStatus)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.ModerationStatus == moderationStatus)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByReportStatusAsync(bool hasReport)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => hasReport ? p.ReportCount > 0 : p.ReportCount == 0)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateViewCountAsync(int postId)
        {
            var post = await GetByIdAsync(postId);
            if (post != null)
            {
                post.ViewCount++;
                post.LastActivityAt = DateTime.UtcNow;
                _context.Posts.Update(post);
            }
        }

        public async Task UpdateLastActivityAsync(int postId)
        {
            var post = await GetByIdAsync(postId);
            if (post != null)
            {
                post.LastActivityAt = DateTime.UtcNow;
                _context.Posts.Update(post);
            }
        }

        public async Task<IEnumerable<Post>> GetPostsByUserAndStatusAsync(int userId, PostStatus status)
        {
            return await _context.Posts
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.AuthorId == userId && p.Status == status)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByLanguageAsync(string language)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active && p.Language == language)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByCountryAsync(string country)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active && p.Country == country)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByAgeRestrictionAsync(int minAge)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active && p.MinAge <= minAge)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsBySubscriptionAsync(bool requiresSubscription)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active && p.RequiresSubscription == requiresSubscription)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByFeaturedAsync(bool isFeatured)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active && p.IsFeatured == isFeatured)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByPopularityAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.CreatedAt >= cutoffDate)
                .OrderByDescending(p => p.ViewCount + p.Likes.Count + p.Replies.Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByTrendingAsync(int days = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active &&
                           p.CreatedAt >= cutoffDate)
                .OrderByDescending(p => p.LastActivityAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByNewestAsync()
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByAlphabeticalAsync()
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByCustomOrderAsync()
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active)
                .OrderBy(p => p.Order)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByUserPreferencesAsync(int userId)
        {
            // This would need to be implemented based on user preferences
            // For now, return all active posts
            return await GetAllAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByTimeOfDayAsync(int hour)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active && p.PeakActivityHour == hour)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsBySeasonAsync(string season)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active && p.SeasonalTheme == season)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByEventAsync(string eventName)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Forum)
                .Include(p => p.Replies)
                .Include(p => p.Likes)
                .Include(p => p.Bookmarks)
                .Where(p => p.Status == PostStatus.Active && p.EventName == eventName)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}