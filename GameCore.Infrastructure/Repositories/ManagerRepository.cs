using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using GameCore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// ç®¡ç??¡å€‰åº«å¯¦ä?
    /// </summary>
    public class ManagerRepository : Repository<ManagerData>, IManagerRepository
    {
        private readonly ILogger<ManagerRepository> _logger;

        public ManagerRepository(GameCoreDbContext context, ILogger<ManagerRepository> logger) : base(context)
        {
            _logger = logger;
        }

        // å¯¦ç¾ IManagerRepository ?¥å£?„ç¼ºå°‘æ–¹æ³?
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
                .Select(p => p.PermissionName).ToListAsync();
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

        // ä¿ç??Ÿæ???Manager ?¸é??¹æ?ï¼ˆç”¨?¼å?å¾Œå…¼å®¹ï?
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
                .Select(p => p.PermissionName).ToListAsync();
        }

        public async Task<IEnumerable<Manager>> GetActiveManagersAsync()
        {
            return await _context.Managers
                .Where(m => m.IsActive)
                .OrderBy(m => m.CreatedAt)
                .Select(p => p.PermissionName).ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(int managerId, string permissionCode)
        {
            return await _context.Managers
                .Where(m => m.ManagerId == managerId)
                .SelectMany(m => m.RolePermissions)
                .AnyAsync(rp => rp.Permission == permissionCode);
        }

        public async Task<IEnumerable<string>> GetManagerPermissionsAsync(int managerId)
        {
            return await _context.Managers
                .Where(m => m.ManagerId == managerId)
                .SelectMany(m => m.RolePermissions)
                .Select(rp => rp.Permission)
                .Select(p => p.PermissionName).ToListAsync();
        }

        public async Task<IEnumerable<GameCore.Domain.Entities.ManagerRole>> GetAllRolesAsync()
        {
            return await _context.ManagerRoles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .OrderBy(r => r.CreateTime)
                .Select(p => p.PermissionName).ToListAsync();
        }

        public async Task<GameCore.Domain.Entities.ManagerRole?> GetRoleByIdAsync(int roleId)
        {
            return await _context.ManagerRoles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<ManagerRolePermission?> GetPermissionByCodeAsync(string code)
        {
            return await _context.ManagerRolePermissions
                .FirstOrDefaultAsync(p => p.PermissionName == code);
        }

        public async Task<IEnumerable<string>> GetAllPermissionsAsync()
        {
            return await _context.ManagerRolePermissions
                .OrderBy(p => p.Description)
                .ThenBy(p => p.PermissionName)
                .Select(p => p.PermissionName).ToListAsync();
        }
    }
}
