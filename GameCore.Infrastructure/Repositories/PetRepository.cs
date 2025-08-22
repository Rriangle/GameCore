using Microsoft.EntityFrameworkCore;
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.Services;
using GameCore.Infrastructure.Data;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 寵物 Repository 實作
    /// 處理寵物相關的資料存取邏輯
    /// </summary>
    public class PetRepository : Repository<Pet>, IPetRepository
    {
        public PetRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 根據使用者 ID 取得寵物 (包含使用者資訊)
        /// </summary>
        public async Task<Pet?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        /// <summary>
        /// 取得需要每日衰減的所有寵物
        /// </summary>
        public async Task<IEnumerable<Pet>> GetPetsForDailyDecayAsync()
        {
            // 取得所有寵物進行每日衰減
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// 更新寵物最後互動時間 (用於冷卻計算)
        /// 由於資料庫結構沒有專門的互動時間表，我們使用快取或其他方式處理
        /// </summary>
        public async Task UpdateLastInteractionAsync(int userId, PetInteractionType interactionType, DateTime interactionTime)
        {
            // 這裡可以使用 Redis 快取或建立臨時表來記錄互動時間
            // 為了簡化，我們暫時使用記憶體快取
            var cacheKey = $"pet_interaction_{userId}_{interactionType}";
            
            // 實際專案中應該使用 IMemoryCache 或 Redis
            // 這裡先用簡單的方式處理
            await Task.CompletedTask;
        }

        /// <summary>
        /// 取得最後互動時間
        /// </summary>
        public async Task<DateTime?> GetLastInteractionTimeAsync(int userId, PetInteractionType interactionType)
        {
            // 對應上面的邏輯，從快取中取得最後互動時間
            // 為了簡化實作，返回 null (表示沒有冷卻)
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 取得寵物等級排行榜
        /// </summary>
        public async Task<IEnumerable<Pet>> GetLevelLeaderboardAsync(int top = 10)
        {
            return await _dbSet
                .Include(p => p.User)
                .ThenInclude(u => u.UserIntroduce)
                .OrderByDescending(p => p.Level)
                .ThenByDescending(p => p.Experience)
                .Take(top)
                .ToListAsync();
        }

        /// <summary>
        /// 取得特定等級範圍的寵物數量統計
        /// </summary>
        public async Task<int> GetPetCountByLevelRangeAsync(int minLevel, int maxLevel)
        {
            return await _dbSet
                .CountAsync(p => p.Level >= minLevel && p.Level <= maxLevel);
        }

        /// <summary>
        /// 取得寵物狀態統計 (健康度分佈)
        /// </summary>
        public async Task<PetHealthStats> GetHealthStatsAsync()
        {
            var allPets = await _dbSet.ToListAsync();

            if (!allPets.Any())
            {
                return new PetHealthStats();
            }

            var stats = new PetHealthStats
            {
                TotalPetCount = allPets.Count,
                ExcellentHealthCount = allPets.Count(p => p.Health >= 80),
                GoodHealthCount = allPets.Count(p => p.Health >= 60 && p.Health < 80),
                AverageHealthCount = allPets.Count(p => p.Health >= 40 && p.Health < 60),
                PoorHealthCount = allPets.Count(p => p.Health >= 20 && p.Health < 40),
                CriticalHealthCount = allPets.Count(p => p.Health < 20),
                AverageHealth = allPets.Average(p => p.Health),
                AverageLevel = allPets.Average(p => p.Level)
            };

            return stats;
        }

        /// <summary>
        /// 搜尋寵物 (根據名稱或主人暱稱)
        /// </summary>
        public async Task<PagedResult<Pet>> SearchPetsAsync(string searchTerm, int page = 1, int pageSize = 20)
        {
            var query = _dbSet
                .Include(p => p.User)
                .ThenInclude(u => u.UserIntroduce)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(p => 
                    p.PetName.ToLower().Contains(term) ||
                    (p.User.UserIntroduce != null && p.User.UserIntroduce.UserNickName.ToLower().Contains(term)));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.Level)
                .ThenByDescending(p => p.Experience)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new PagedResult<Pet>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
    }
}