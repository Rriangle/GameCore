using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        public ManagerRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<Manager?> GetByAccountAsync(string account)
        {
            return await _context.Managers
                .Include(m => m.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(m => m.Account == account);
        }

        public async Task<IEnumerable<Manager>> GetManagersByRoleAsync(int roleId)
        {
            return await _context.Managers
                .Include(m => m.Role)
                .Where(m => m.RoleId == roleId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Manager>> GetActiveManagersAsync()
        {
            return await _context.Managers
                .Include(m => m.Role)
                .Where(m => m.IsActive)
                .OrderBy(m => m.CreateTime)
                .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(int managerId, string permissionCode)
        {
            return await _context.Managers
                .Where(m => m.Id == managerId)
                .SelectMany(m => m.Role.RolePermissions)
                .AnyAsync(rp => rp.Permission.Code == permissionCode);
        }

        public async Task<IEnumerable<Permission>> GetManagerPermissionsAsync(int managerId)
        {
            return await _context.Managers
                .Where(m => m.Id == managerId)
                .SelectMany(m => m.Role.RolePermissions)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<ManagerRole>> GetAllRolesAsync()
        {
            return await _context.ManagerRoles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .OrderBy(r => r.CreateTime)
                .ToListAsync();
        }

        public async Task<ManagerRole?> GetRoleByIdAsync(int roleId)
        {
            return await _context.ManagerRoles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<Permission?> GetPermissionByCodeAsync(string code)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }
    }
}