namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 通用 Repository 介面
    /// </summary>
    /// <typeparam name="T">實體類型</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// 根據ID取得實體
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// 取得所有實體
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// 新增實體
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 更新實體
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// 刪除實體
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 檢查實體是否存在
        /// </summary>
        Task<bool> ExistsAsync(int id);
    }
} 
