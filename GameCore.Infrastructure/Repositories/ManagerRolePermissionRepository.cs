using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class ManagerRolePermissionRepository : Repository<ManagerRolePermission>, IManagerRolePermissionRepository
    {
        private readonly ILogger<ManagerRolePermissionRepository> _logger;

        public ManagerRolePermissionRepository(GameCoreDbContext context, ILogger<ManagerRolePermissionRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ManagerRolePermission>> GetByRoleAsync(string role)
        {
            return await _dbSet
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .Where(rp => rp.Role.Name == role)
                .ToListAsync();
        }

        public async Task<IEnumerable<ManagerRolePermission>> GetByPermissionAsync(string permission)
        {
            return await _dbSet
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .Where(rp => rp.Permission == permission)
                .ToListAsync();
        }

        public async Task<ManagerRolePermission> AddAsync(ManagerRolePermission rolePermission)
        {
            var result = await _dbSet.AddAsync(rolePermission);
            return result.Entity;
        }

        public Task UpdateAsync(ManagerRolePermission rolePermission)
        {
            _dbSet.Update(rolePermission);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ManagerRolePermission rolePermission)
        {
            _dbSet.Remove(rolePermission);
            return Task.CompletedTask;
        }
    }
} 
