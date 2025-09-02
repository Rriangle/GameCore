using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using GameCore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 管理員倉庫實作
    /// </summary>
    public class ManagerRepository : Repository<ManagerData>, IManagerRepository
    {
        private readonly ILogger<ManagerRepository> _logger;

        public ManagerRepository(GameCoreDbContext context, ILogger<ManagerRepository> logger) : base(context)
        {
            _logger = logger;
        }

        // 實現 IManagerRepository 接口的缺少方法
        public async Task<ManagerData?> GetByIdAsync(int id)
        {
            return await _context.ManagerData.FindAsync(id);
        }

        public async Task<ManagerData?> GetByUsernameAsync(string username)
        {
            return await _context.ManagerData
                .FirstOrDefaultAsync(m => m.Username == username);
        }

        public async Task<ManagerData?> GetByEmailAsync(string email)
        {
            return await _context.ManagerData
                .FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.ManagerData
                .AnyAsync(m => m.Username == username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.ManagerData
                .AnyAsync(m => m.Email == email);
        }

        public async Task<ManagerData> AddAsync(ManagerData manager)
        {
            _context.ManagerData.Add(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        public async Task<ManagerData> UpdateAsync(ManagerData manager)
        {
            _context.ManagerData.Update(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        public async Task DeleteAsync(int id)
        {
            var manager = await _context.ManagerData.FindAsync(id);
            if (manager != null)
            {
                _context.ManagerData.Remove(manager);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ManagerData> Update(ManagerData manager)
        {
            _context.ManagerData.Update(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        public async Task<IEnumerable<ManagerData>> GetAllAsync()
        {
            return await _context.ManagerData
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ManagerData
                .AnyAsync(m => m.Id == id);
        }

        public async Task<ManagerData> Add(ManagerData manager)
        {
            _context.ManagerData.Add(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        // 保留原有的 Manager 相關方法（用於向後兼容）
        public async Task<Manager?> GetByAccountAsync(string account)
        {
            return await _context.Managers
                .Include(m => m.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(m => m.Username == account);
        }

        public async Task<IEnumerable<Manager>> GetManagersByRoleAsync(int roleId)
        {
            return await _context.Managers
                .Where(m => m.Role == roleId.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Manager>> GetActiveManagersAsync()
        {
            return await _context.Managers
                .Where(m => m.IsActive)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(int managerId, string permissionCode)
        {
            return await _context.Managers
                .Where(m => m.ManagerId == managerId)
                .SelectMany(m => m.RolePermissions)
                .AnyAsync(rp => rp.Permission.Code == permissionCode);
        }

        public async Task<IEnumerable<Permission>> GetManagerPermissionsAsync(int managerId)
        {
            return await _context.Managers
                .Where(m => m.ManagerId == managerId)
                .SelectMany(m => m.RolePermissions)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameCore.Domain.Entities.ManagerRole>> GetAllRolesAsync()
        {
            return await _context.ManagerRoles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .OrderBy(r => r.CreateTime)
                .ToListAsync();
        }

        public async Task<GameCore.Domain.Entities.ManagerRole?> GetRoleByIdAsync(int roleId)
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