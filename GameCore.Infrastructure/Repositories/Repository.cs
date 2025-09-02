using Microsoft.EntityFrameworkCore;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using GameCore.Application.Common;
using System.Linq.Expressions;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 基礎 Repository 實作
    /// 提供所有 Repository 的通用方法實作
    /// </summary>
    /// <typeparam name="T">實體類型</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly GameCoreDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(GameCoreDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// 根據 ID 取得實體
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        /// <summary>
        /// 取得所有實體
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根據條件查詢實體
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根據條件取得單一實體
        /// </summary>
        public virtual async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 新增實體
        /// </summary>
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _dbSet.AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        /// <summary>
        /// 更新實體
        /// </summary>
        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 刪除實體
        /// </summary>
        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 根據 ID 刪除實體
        /// </summary>
        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity != null)
            {
                await DeleteAsync(entity, cancellationToken);
            }
        }

        /// <summary>
        /// 檢查實體是否存在
        /// </summary>
        public virtual async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken) != null;
        }

        /// <summary>
        /// 根據條件檢查實體是否存在
        /// </summary>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 計算實體數量
        /// </summary>
        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        /// <summary>
        /// 根據條件計算實體數量
        /// </summary>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 分頁查詢實體
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根據條件分頁查詢實體
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(predicate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        // 保留原有的方法以向後兼容
        /// <summary>
        /// 根據條件查詢實體 (向後兼容)
        /// </summary>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAsync(predicate);
        }

        /// <summary>
        /// 根據條件取得單一實體 (向後兼容)
        /// </summary>
        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetFirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 批次新增實體 (向後兼容)
        /// </summary>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// 根據 ID 刪除實體 (向後兼容)
        /// </summary>
        public virtual async Task DeleteByIdAsync(int id)
        {
            await DeleteAsync(id);
        }

        /// <summary>
        /// 分頁查詢實體 (向後兼容)
        /// </summary>
        public virtual async Task<PagedResult<T>> GetPagedAsync<TKey>(
            int page, 
            int pageSize, 
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, TKey>>? orderBy = null,
            bool descending = false)
        {
            var query = _dbSet.AsQueryable();

            // 套用篩選條件
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // 計算總數
            var totalCount = await query.CountAsync();

            // 套用排序
            if (orderBy != null)
            {
                query = descending 
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            // 套用分頁
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new PagedResult<T>
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