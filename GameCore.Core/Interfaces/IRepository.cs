using System.Linq.Expressions;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 基礎 Repository 介面
    /// 定義所有 Repository 的通用方法
    /// </summary>
    /// <typeparam name="T">實體類型</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 取得所有實體
        /// </summary>
        /// <returns>實體列表</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// 根據 ID 取得實體
        /// </summary>
        /// <param name="id">實體 ID</param>
        /// <returns>實體或 null</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// 根據條件查詢實體
        /// </summary>
        /// <param name="predicate">查詢條件</param>
        /// <returns>實體列表</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 根據條件取得單一實體
        /// </summary>
        /// <param name="predicate">查詢條件</param>
        /// <returns>實體或 null</returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 新增實體
        /// </summary>
        /// <param name="entity">要新增的實體</param>
        /// <returns>新增後的實體</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 批次新增實體
        /// </summary>
        /// <param name="entities">要新增的實體列表</param>
        /// <returns>Task</returns>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// 更新實體
        /// </summary>
        /// <param name="entity">要更新的實體</param>
        /// <returns>Task</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// 刪除實體
        /// </summary>
        /// <param name="entity">要刪除的實體</param>
        /// <returns>Task</returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// 根據 ID 刪除實體
        /// </summary>
        /// <param name="id">實體 ID</param>
        /// <returns>Task</returns>
        Task DeleteByIdAsync(int id);

        /// <summary>
        /// 檢查實體是否存在
        /// </summary>
        /// <param name="predicate">檢查條件</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 計算符合條件的實體數量
        /// </summary>
        /// <param name="predicate">計算條件</param>
        /// <returns>實體數量</returns>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        /// <summary>
        /// 分頁查詢實體
        /// </summary>
        /// <param name="page">頁數 (從 1 開始)</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="predicate">查詢條件</param>
        /// <param name="orderBy">排序條件</param>
        /// <returns>分頁結果</returns>
        Task<PagedResult<T>> GetPagedAsync<TKey>(
            int page, 
            int pageSize, 
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, TKey>>? orderBy = null,
            bool descending = false);
    }

    /// <summary>
    /// 分頁結果
    /// </summary>
    /// <typeparam name="T">實體類型</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// 資料列表
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// 總記錄數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 當前頁數
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有上一頁
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// 是否有下一頁
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}