using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class ManagerDataRepository : Repository<ManagerData>, IManagerDataRepository
    {
        private readonly ILogger<ManagerDataRepository> _logger;

        public ManagerDataRepository(GameCoreDbContext context, ILogger<ManagerDataRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ManagerData>> GetByManagerIdAsync(int managerId)
        {
            return await _dbSet
                .Include(md => md.Manager)
                .Where(md => md.ManagerId == managerId)
                .OrderBy(md => md.DataType)
                .ToListAsync();
        }

        public async Task<ManagerData?> GetByTypeAndKeyAsync(string dataType, string key)
        {
            return await _dbSet
                .Include(md => md.Manager)
                .FirstOrDefaultAsync(md => md.DataType == dataType && md.Key == key);
        }

        public async Task<IEnumerable<ManagerData>> GetByDataTypeAsync(string dataType)
        {
            return await _dbSet
                .Include(md => md.Manager)
                .Where(md => md.DataType == dataType)
                .OrderBy(md => md.ManagerId)
                .ToListAsync();
        }

        public async Task<bool> SetManagerDataAsync(int managerId, string dataType, string key, string value)
        {
            var existingData = await _dbSet
                .FirstOrDefaultAsync(md => md.ManagerId == managerId && md.DataType == dataType && md.Key == key);

            if (existingData != null)
            {
                existingData.Value = value;
                existingData.UpdatedAt = DateTime.UtcNow;
                _dbSet.Update(existingData);
                return true;
            }
            else
            {
                var newData = new ManagerData
                {
                    ManagerId = managerId,
                    DataType = dataType,
                    Key = key,
                    Value = value,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _dbSet.AddAsync(newData);
                return true;
            }
        }

        public async Task<bool> DeleteManagerDataAsync(int managerId, string dataType, string key)
        {
            var data = await _dbSet
                .FirstOrDefaultAsync(md => md.ManagerId == managerId && md.DataType == dataType && md.Key == key);

            if (data != null)
            {
                _dbSet.Remove(data);
                return true;
            }
            return false;
        }
    }
} 