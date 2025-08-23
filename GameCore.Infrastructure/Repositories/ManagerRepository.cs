using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class ManagerRepository : Repository<ManagerData>, IManagerRepository
    {
        public ManagerRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<ManagerData?> GetByAccountAsync(string account)
        {
            return await _context.Set<ManagerData>()
                .FirstOrDefaultAsync(m => m.Account == account);
        }

        public async Task<IEnumerable<ManagerRolePermission>> GetPermissionsAsync(int managerId)
        {
            return await _context.Set<ManagerRolePermission>()
                .Where(p => p.ManagerId == managerId)
                .ToListAsync();
        }
    }
}