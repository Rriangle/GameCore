using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 管理員 Repository 實作
    /// </summary>
    public class ManagerRepository : Repository<ManagerData>, IManagerRepository
    {
        public ManagerRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 獲取管理員權限
        /// </summary>
        public async Task<IEnumerable<ManagerRolePermission>> GetPermissionsAsync(int managerId)
        {
            // 簡化實作，返回空集合
            return new List<ManagerRolePermission>();
        }

        /// <summary>
        /// 獲取包含角色的管理員資料
        /// </summary>
        public async Task<ManagerData?> GetWithRoleAsync(int managerId)
        {
            return await _context.ManagerData
                .FirstOrDefaultAsync(m => m.ManagerId == managerId);
        }

        /// <summary>
        /// 根據使用者ID獲取管理員資料
        /// </summary>
        public async Task<ManagerData?> GetByUserIdAsync(int userId)
        {
            // 簡化實作，因為 ManagerData 沒有 UserId
            return await _context.ManagerData
                .FirstOrDefaultAsync(m => m.ManagerId == userId);
        }

        /// <summary>
        /// 檢查是否為管理員
        /// </summary>
        public async Task<bool> IsManagerAsync(int userId)
        {
            // 簡化實作，因為 ManagerData 沒有 UserId
            return await _context.ManagerData
                .AnyAsync(m => m.ManagerId == userId);
        }
    }
}