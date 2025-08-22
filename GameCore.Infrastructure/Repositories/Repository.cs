using Microsoft.EntityFrameworkCore;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
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
        /// 取得所有實體
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// 根據 ID 取得實體
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// 根據條件查詢實體
        /// </summary>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 根據條件取得單一實體
        /// </summary>
        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 新增實體
        /// </summary>
        public virtual async Task<T> AddAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        /// <summary>
        /// 批次新增實體
        /// </summary>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// 更新實體
        /// </summary>
        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 刪除實體
        /// </summary>
        public virtual Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 根據 ID 刪除實體
        /// </summary>
        public virtual async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        /// <summary>
        /// 檢查實體是否存在
        /// </summary>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// 計算符合條件的實體數量
        /// </summary>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            return await _dbSet.CountAsync(predicate);
        }

        /// <summary>
        /// 分頁查詢實體
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