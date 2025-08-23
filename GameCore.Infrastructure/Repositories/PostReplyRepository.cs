using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class PostReplyRepository : IPostReplyRepository
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<PostReplyRepository> _logger;

        public PostReplyRepository(GameCoreDbContext context, ILogger<PostReplyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PostReply> GetByIdAsync(int id)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<PostReply>> GetByPostIdAsync(int postId, int page = 1, int pageSize = 50)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active)
                .OrderBy(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetByAuthorIdAsync(int authorId, int page = 1, int pageSize = 50)
        {
            return await _context.PostReplies
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.AuthorId == authorId && r.Status == PostStatus.Active)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetByParentReplyIdAsync(int parentReplyId)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.ParentReplyId == parentReplyId && r.Status == PostStatus.Active)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetByStatusAsync(PostStatus status, int page = 1, int pageSize = 50)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<PostReply> AddAsync(PostReply reply)
        {
            await _context.PostReplies.AddAsync(reply);
            return reply;
        }

        public async Task UpdateAsync(PostReply reply)
        {
            _context.PostReplies.Update(reply);
        }

        public async Task DeleteAsync(int id)
        {
            var reply = await GetByIdAsync(id);
            if (reply != null)
            {
                reply.Status = PostStatus.Deleted;
                _context.PostReplies.Update(reply);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PostReplies.AnyAsync(r => r.Id == id);
        }

        public async Task<int> GetReplyCountAsync()
        {
            return await _context.PostReplies.CountAsync(r => r.Status == PostStatus.Active);
        }

        public async Task<int> GetReplyCountByPostAsync(int postId)
        {
            return await _context.PostReplies.CountAsync(r => 
                r.PostId == postId && r.Status == PostStatus.Active);
        }

        public async Task<int> GetReplyCountByAuthorAsync(int authorId)
        {
            return await _context.PostReplies.CountAsync(r => 
                r.AuthorId == authorId && r.Status == PostStatus.Active);
        }

        public async Task<int> GetReplyCountByStatusAsync(PostStatus status)
        {
            return await _context.PostReplies.CountAsync(r => r.Status == status);
        }

        public async Task<IEnumerable<PostReply>> SearchRepliesAsync(string searchTerm, int page = 1, int pageSize = 50)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active &&
                           (r.Content.Contains(searchTerm) ||
                            r.Author.Username.Contains(searchTerm) ||
                            r.Post.Title.Contains(searchTerm)))
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active &&
                           r.CreatedAt >= startDate && r.CreatedAt <= endDate)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByLastActivityAsync(DateTime lastActivity)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active &&
                           r.LastActivityAt >= lastActivity)
                .OrderByDescending(r => r.LastActivityAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByLikeCountAsync(int minLikeCount)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active &&
                           r.Likes.Count >= minLikeCount)
                .OrderByDescending(r => r.Likes.Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByReportStatusAsync(bool hasReport)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => hasReport ? r.ReportCount > 0 : r.ReportCount == 0)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByModerationStatusAsync(string moderationStatus)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.ModerationStatus == moderationStatus)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByUserAndStatusAsync(int userId, PostStatus status)
        {
            return await _context.PostReplies
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.AuthorId == userId && r.Status == status)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByLanguageAsync(string language)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active && r.Language == language)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByCountryAsync(string country)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active && r.Country == country)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByAgeRestrictionAsync(int minAge)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active && r.MinAge <= minAge)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesBySubscriptionAsync(bool requiresSubscription)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active && r.RequiresSubscription == requiresSubscription)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByFeaturedAsync(bool isFeatured)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active && r.IsFeatured == isFeatured)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByPopularityAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active &&
                           r.CreatedAt >= cutoffDate)
                .OrderByDescending(r => r.Likes.Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByTrendingAsync(int days = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active &&
                           r.CreatedAt >= cutoffDate)
                .OrderByDescending(r => r.LastActivityAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByNewestAsync()
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByAlphabeticalAsync()
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active)
                .OrderBy(r => r.Content)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByCustomOrderAsync()
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active)
                .OrderBy(r => r.Order)
                .ThenByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByUserPreferencesAsync(int userId)
        {
            // This would need to be implemented based on user preferences
            // For now, return all active replies
            return await GetAllAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByTimeOfDayAsync(int hour)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active && r.PeakActivityHour == hour)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesBySeasonAsync(string season)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active && r.SeasonalTheme == season)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByEventAsync(string eventName)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Post)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.Status == PostStatus.Active && r.EventName == eventName)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateLastActivityAsync(int replyId)
        {
            var reply = await GetByIdAsync(replyId);
            if (reply != null)
            {
                reply.LastActivityAt = DateTime.UtcNow;
                _context.PostReplies.Update(reply);
            }
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByThreadAsync(int postId, int? parentReplyId = null)
        {
            var query = _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active);

            if (parentReplyId.HasValue)
            {
                query = query.Where(r => r.ParentReplyId == parentReplyId.Value);
            }
            else
            {
                query = query.Where(r => r.ParentReplyId == null);
            }

            return await query
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByDepthAsync(int postId, int depth)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active && r.Depth == depth)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByPathAsync(int postId, string path)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active && r.Path.StartsWith(path))
                .OrderBy(r => r.Path)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByAncestorAsync(int postId, int ancestorId)
        {
            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active && r.Path.Contains(ancestorId.ToString()))
                .OrderBy(r => r.Path)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByDescendantAsync(int postId, int descendantId)
        {
            var descendant = await GetByIdAsync(descendantId);
            if (descendant == null) return new List<PostReply>();

            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active && 
                           (r.Path.StartsWith(descendant.Path) || r.Id == descendantId))
                .OrderBy(r => r.Path)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesBySiblingAsync(int postId, int replyId)
        {
            var reply = await GetByIdAsync(replyId);
            if (reply == null) return new List<PostReply>();

            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active && 
                           r.ParentReplyId == reply.ParentReplyId && r.Id != replyId)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByUncleAsync(int postId, int replyId)
        {
            var reply = await GetByIdAsync(replyId);
            if (reply == null || reply.ParentReplyId == null) return new List<PostReply>();

            var parentReply = await GetByIdAsync(reply.ParentReplyId.Value);
            if (parentReply == null || parentReply.ParentReplyId == null) return new List<PostReply>();

            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active && 
                           r.ParentReplyId == parentReply.ParentReplyId.Value && r.Id != parentReply.Id)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReply>> GetRepliesByCousinAsync(int postId, int replyId)
        {
            var reply = await GetByIdAsync(replyId);
            if (reply == null || reply.ParentReplyId == null) return new List<PostReply>();

            var parentReply = await GetByIdAsync(reply.ParentReplyId.Value);
            if (parentReply == null || parentReply.ParentReplyId == null) return new List<PostReply>();

            var grandparentReply = await GetByIdAsync(parentReply.ParentReplyId.Value);
            if (grandparentReply == null) return new List<PostReply>();

            return await _context.PostReplies
                .Include(r => r.Author)
                .Include(r => r.Likes)
                .Include(r => r.ParentReply)
                .Where(r => r.PostId == postId && r.Status == PostStatus.Active && 
                           r.ParentReplyId.HasValue && r.ParentReplyId != reply.ParentReplyId.Value &&
                           _context.PostReplies.Any(pr => pr.Id == r.ParentReplyId.Value && 
                                                        pr.ParentReplyId == grandparentReply.Id))
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}