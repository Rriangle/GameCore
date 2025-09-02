using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GameCore.Infrastructure.Repositories
{
    public class UserSalesInformationRepository : IUserSalesInformationRepository
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<UserSalesInformationRepository> _logger;

        public UserSalesInformationRepository(GameCoreDbContext context, ILogger<UserSalesInformationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserSalesInformation?> GetByIdAsync(int id)
        {
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .FirstOrDefaultAsync(usi => usi.Id == id);
        }

        public async Task<UserSalesInformation?> GetByUserIdAsync(int userId)
        {
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .FirstOrDefaultAsync(usi => usi.UserId == userId);
        }

        public async Task<IEnumerable<UserSalesInformation>> GetAllAsync(int skip = 0, int take = 20)
        {
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .OrderByDescending(usi => usi.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<UserSalesInformation> AddAsync(UserSalesInformation salesInfo)
        {
            _context.UserSalesInformations.Add(salesInfo);
            await _context.SaveChangesAsync();
            return salesInfo;
        }

        public async Task UpdateAsync(UserSalesInformation salesInfo)
        {
            _context.UserSalesInformations.Update(salesInfo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserSalesInformation salesInfo)
        {
            _context.UserSalesInformations.Remove(salesInfo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var salesInfo = await _context.UserSalesInformations.FindAsync(id);
            if (salesInfo == null) return false;

            _context.UserSalesInformations.Remove(salesInfo);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<UserSalesInformation>> GetByStatusAsync(string status, int skip = 0, int take = 20)
        {
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .Where(usi => usi.Status == status)
                .OrderByDescending(usi => usi.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetCountByStatusAsync(string status)
        {
            return await _context.UserSalesInformations
                .CountAsync(usi => usi.Status == status);
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var salesInfo = await _context.UserSalesInformations.FindAsync(id);
            if (salesInfo == null) return false;

            salesInfo.Status = status;
            salesInfo.UpdatedAt = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // IRepository<T> implementation
        public async Task<UserSalesInformation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .FirstOrDefaultAsync(usi => usi.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<UserSalesInformation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserSalesInformation>> GetAsync(Expression<Func<UserSalesInformation, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserSalesInformation?> GetFirstOrDefaultAsync(Expression<Func<UserSalesInformation, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<UserSalesInformation> AddAsync(UserSalesInformation entity, CancellationToken cancellationToken = default)
        {
            _context.UserSalesInformations.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(UserSalesInformation entity, CancellationToken cancellationToken = default)
        {
            _context.UserSalesInformations.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(UserSalesInformation entity, CancellationToken cancellationToken = default)
        {
            _context.UserSalesInformations.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.UserSalesInformations.FindAsync(id);
            if (entity != null)
            {
                _context.UserSalesInformations.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.UserSalesInformations.AnyAsync(usi => usi.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Expression<Func<UserSalesInformation, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.UserSalesInformations.AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UserSalesInformations.CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<UserSalesInformation, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.UserSalesInformations.CountAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<UserSalesInformation>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var skip = (page - 1) * pageSize;
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .OrderByDescending(usi => usi.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserSalesInformation>> GetPagedAsync(Expression<Func<UserSalesInformation, bool>> predicate, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var skip = (page - 1) * pageSize;
            return await _context.UserSalesInformations
                .Include(usi => usi.User)
                .Where(predicate)
                .OrderByDescending(usi => usi.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
} 