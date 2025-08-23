using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<StoreRepository> _logger;

        public StoreRepository(GameCoreDbContext context, ILogger<StoreRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Store> GetByIdAsync(int id)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Store>> GetAllAsync()
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Store> AddAsync(Store store)
        {
            await _context.Stores.AddAsync(store);
            return store;
        }

        public async Task UpdateAsync(Store store)
        {
            _context.Stores.Update(store);
        }

        public async Task DeleteAsync(int id)
        {
            var store = await GetByIdAsync(id);
            if (store != null)
            {
                store.IsActive = false;
                _context.Stores.Update(store);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Stores.AnyAsync(s => s.Id == id);
        }

        public async Task<int> GetStoreCountAsync()
        {
            return await _context.Stores.CountAsync(s => s.IsActive);
        }

        public async Task<IEnumerable<Store>> GetStoresByCategoryAsync(int categoryId)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.CategoryId == categoryId && s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> SearchStoresAsync(string searchTerm)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive &&
                           (s.Name.Contains(searchTerm) || 
                            s.Description.Contains(searchTerm) ||
                            s.Category.Name.Contains(searchTerm)))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByStatusAsync(bool isActive)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive == isActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByPermissionAsync(string permission)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.RequiredPermission == permission)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByCountryAsync(string country)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.Country == country)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByLanguageAsync(string language)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.Language == language)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByAgeRestrictionAsync(int minAge)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.MinAge <= minAge)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresBySubscriptionAsync(bool requiresSubscription)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.RequiresSubscription == requiresSubscription)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByFeaturedAsync(bool isFeatured)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.IsFeatured == isFeatured)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByPopularityAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive &&
                           s.LastActivityAt >= cutoffDate)
                .OrderByDescending(s => s.ViewCount)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByTrendingAsync(int days = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive &&
                           s.LastActivityAt >= cutoffDate)
                .OrderByDescending(s => s.LastActivityAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByNewestAsync()
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByAlphabeticalAsync()
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByCustomOrderAsync()
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive)
                .OrderBy(s => s.Order)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByUserPreferencesAsync(int userId)
        {
            // This would need to be implemented based on user preferences
            // For now, return all active stores
            return await GetAllAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByTimeOfDayAsync(int hour)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.PeakActivityHour == hour)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresBySeasonAsync(string season)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.SeasonalTheme == season)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByEventAsync(string eventName)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.IsActive && s.EventName == eventName)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task UpdateLastActivityAsync(int storeId)
        {
            var store = await GetByIdAsync(storeId);
            if (store != null)
            {
                store.LastActivityAt = DateTime.UtcNow;
                _context.Stores.Update(store);
            }
        }

        public async Task<IEnumerable<Store>> GetStoresByModerationStatusAsync(string moderationStatus)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => s.ModerationStatus == moderationStatus)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByReportStatusAsync(bool hasReport)
        {
            return await _context.Stores
                .Include(s => s.Category)
                .Include(s => s.Products)
                .Where(s => hasReport ? s.ReportCount > 0 : s.ReportCount == 0)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByUserAccessAsync(int userId)
        {
            // This would need to be implemented based on user permissions
            // For now, return all active stores
            return await GetAllAsync();
        }

        public async Task<Store> GetOrCreateStoreAsync(int categoryId, string name, string description = null)
        {
            var existingStore = await _context.Stores
                .FirstOrDefaultAsync(s => s.CategoryId == categoryId && s.Name == name);

            if (existingStore != null)
            {
                return existingStore;
            }

            var newStore = new Store
            {
                CategoryId = categoryId,
                Name = name,
                Description = description ?? $"Store for {name}",
                IsActive = true,
                Order = 1,
                CreatedAt = DateTime.UtcNow,
                LastActivityAt = DateTime.UtcNow
            };

            return await AddAsync(newStore);
        }
    }
}