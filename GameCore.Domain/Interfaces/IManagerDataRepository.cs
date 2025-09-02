using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 管理員資料倉庫介面
    /// </summary>
    public interface IManagerDataRepository : IRepository<ManagerData>
    {
        /// <summary>
        /// 根據管理員ID取得資料
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <returns>管理員資料列表</returns>
        Task<IEnumerable<ManagerData>> GetByManagerIdAsync(int managerId);

        /// <summary>
        /// 根據資料類型和鍵值取得資料
        /// </summary>
        /// <param name="dataType">資料類型</param>
        /// <param name="dataKey">資料鍵值</param>
        /// <returns>管理員資料</returns>
        Task<ManagerData?> GetByTypeAndKeyAsync(string dataType, string dataKey);

        /// <summary>
        /// 根據資料類型取得資料列表
        /// </summary>
        /// <param name="dataType">資料類型</param>
        /// <returns>管理員資料列表</returns>
        Task<IEnumerable<ManagerData>> GetByDataTypeAsync(string dataType);

        /// <summary>
        /// 設定管理員資料
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <param name="dataType">資料類型</param>
        /// <param name="dataKey">資料鍵值</param>
        /// <param name="dataValue">資料值</param>
        /// <returns>設定結果</returns>
        Task<bool> SetManagerDataAsync(int managerId, string dataType, string dataKey, string dataValue);

        /// <summary>
        /// 刪除管理員資料
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <param name="dataType">資料類型</param>
        /// <param name="dataKey">資料鍵值</param>
        /// <returns>刪除結果</returns>
        Task<bool> DeleteManagerDataAsync(int managerId, string dataType, string dataKey);
    }
} 
