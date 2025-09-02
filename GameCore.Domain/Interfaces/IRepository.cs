using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 基礎倉庫介面
    /// </summary>
    /// <typeparam name="T">實體類型</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 根據 ID 取得實體
        /// </summary>
        /// <param name="id">實體 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>實體</returns>
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得所有實體
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>實體列表</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據條件取得實體
        /// </summary>
        /// <param name="predicate">條件表達式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>實體列表</returns>
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據條件取得單一實體
        /// </summary>
        /// <param name="predicate">條件表達式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>實體</returns>
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 新增實體
        /// </summary>
        /// <param name="entity">實體</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>新增的實體</returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新實體
        /// </summary>
        /// <param name="entity">實體</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新結果</returns>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 刪除實體
        /// </summary>
        /// <param name="entity">實體</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>刪除結果</returns>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據 ID 刪除實體
        /// </summary>
        /// <param name="id">實體 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>刪除結果</returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 檢查實體是否存在
        /// </summary>
        /// <param name="id">實體 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據條件檢查實體是否存在
        /// </summary>
        /// <param name="predicate">條件表達式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得實體數量
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>實體數量</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據條件取得實體數量
        /// </summary>
        /// <param name="predicate">條件表達式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>實體數量</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分頁查詢
        /// </summary>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>分頁結果</returns>
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 條件分頁查詢
        /// </summary>
        /// <param name="predicate">條件表達式</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>分頁結果</returns>
        Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
} 
