using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class ForumRepository : Repository<Forum>, IForumRepository
    {
        private readonly ILogger<ForumRepository> _logger;

        public ForumRepository(GameCoreDbContext context, ILogger<ForumRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<Forum> GetByIdAsync(int id)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Forum>> GetAllAsync()
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.CategoryId == categoryId && f.IsActive)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<Forum> AddAsync(Forum forum)
        {
            await _context.Forums.AddAsync(forum);
            return forum;
        }

        public async Task UpdateAsync(Forum forum)
        {
            _context.Forums.Update(forum);
        }

        public async Task DeleteAsync(int id)
        {
            var forum = await GetByIdAsync(id);
            if (forum != null)
            {
                forum.IsActive = false;
                _context.Forums.Update(forum);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Forums.AnyAsync(f => f.Id == id);
        }

        // 實現 IForumRepository 接口的缺少方法
        public async Task<IEnumerable<Forum>> GetActiveForumsAsync(int limit)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive)
                .OrderByDescending(f => f.LastActivityAt ?? f.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetByCategoryAsync(string category)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.Category.Name == category && f.IsActive)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<int> GetForumCountAsync()
        {
            return await _context.Forums.CountAsync(f => f.IsActive);
        }

        public async Task<int> GetForumCountByCategoryAsync(int categoryId)
        {
            return await _context.Forums.CountAsync(f => 
                f.CategoryId == categoryId && f.IsActive);
        }

        public async Task<IEnumerable<Forum>> GetForumsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.CreatedAt >= startDate && f.CreatedAt <= endDate && f.IsActive)
                .OrderBy(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByLastActivityAsync(DateTime lastActivity)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.LastActivityAt >= lastActivity && f.IsActive)
                .OrderByDescending(f => f.LastActivityAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByPostCountAsync(int minPostCount)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.PostCount >= minPostCount)
                .OrderByDescending(f => f.PostCount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByViewCountAsync(int minViewCount)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.ViewCount >= minViewCount)
                .OrderByDescending(f => f.ViewCount)
                .ToListAsync();
        }

        public async Task<Forum> GetOrCreateForumAsync(int categoryId, string name, string description = null)
        {
            var existingForum = await _context.Forums
                .FirstOrDefaultAsync(f => f.CategoryId == categoryId && f.Name == name);

            if (existingForum != null)
            {
                return existingForum;
            }

            var newForum = new Forum
            {
                CategoryId = categoryId,
                Name = name,
                Description = description ?? $"Forum for {name}",
                IsActive = true,
                Order = 1,
                CreatedAt = DateTime.UtcNow,
                LastActivityAt = DateTime.UtcNow
            };

            return await AddAsync(newForum);
        }

        public async Task UpdateLastActivityAsync(int forumId)
        {
            var forum = await GetByIdAsync(forumId);
            if (forum != null)
            {
                forum.LastActivityAt = DateTime.UtcNow;
                _context.Forums.Update(forum);
            }
        }

        public async Task<IEnumerable<Forum>> GetForumsByStatusAsync(bool isActive)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive == isActive)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByPermissionAsync(string permission)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.RequiredPermission == permission)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> SearchForumsAsync(string searchTerm)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive &&
                           (f.Name.Contains(searchTerm) || 
                            f.Description.Contains(searchTerm) ||
                            f.Category.Name.Contains(searchTerm)))
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByUserAccessAsync(int userId)
        {
            // This would need to be implemented based on user permissions
            // For now, return all active forums
            return await GetAllAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByModerationStatusAsync(string moderationStatus)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.ModerationStatus == moderationStatus)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByLanguageAsync(string language)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.Language == language)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByCountryAsync(string country)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.Country == country)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByAgeRestrictionAsync(int minAge)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.MinAge <= minAge)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsBySubscriptionAsync(bool requiresSubscription)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.RequiresSubscription == requiresSubscription)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByFeaturedAsync(bool isFeatured)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.IsFeatured == isFeatured)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByPopularityAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive &&
                           f.LastActivityAt >= cutoffDate)
                .OrderByDescending(f => f.PostCount)
                .ThenByDescending(f => f.ViewCount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByTrendingAsync(int days = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive &&
                           f.LastActivityAt >= cutoffDate)
                .OrderByDescending(f => f.LastActivityAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByNewestAsync()
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByAlphabeticalAsync()
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByCustomOrderAsync()
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByUserPreferencesAsync(int userId)
        {
            // This would need to be implemented based on user preferences
            // For now, return all active forums
            return await GetAllAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByTimeOfDayAsync(int hour)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.PeakActivityHour == hour)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsBySeasonAsync(string season)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.SeasonalTheme == season)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Forum>> GetForumsByEventAsync(string eventName)
        {
            return await _context.Forums
                .Include(f => f.Category)
                .Include(f => f.Posts)
                .Where(f => f.IsActive && f.EventName == eventName)
                .OrderBy(f => f.Order)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }
    }
}