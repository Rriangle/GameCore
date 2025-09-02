using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 管理者資料存取介面
    /// </summary>
    public interface IManagerRepository
    {
        /// <summary>
        /// 根據ID取得管理者
        /// </summary>
        Task<ManagerData?> GetByIdAsync(int id);

        /// <summary>
        /// 根據用戶名取得管理者
        /// </summary>
        Task<ManagerData?> GetByUsernameAsync(string username);

        /// <summary>
        /// 根據Email取得管理者
        /// </summary>
        Task<ManagerData?> GetByEmailAsync(string email);

        /// <summary>
        /// 檢查用戶名是否存在
        /// </summary>
        Task<bool> ExistsByUsernameAsync(string username);

        /// <summary>
        /// 檢查Email是否存在
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// 新增管理者
        /// </summary>
        Task<ManagerData> AddAsync(ManagerData manager);

        /// <summary>
        /// 更新管理者
        /// </summary>
        Task<ManagerData> UpdateAsync(ManagerData manager);

        /// <summary>
        /// 刪除管理者
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 更新管理者
        /// </summary>
        Task<ManagerData> Update(ManagerData manager);

        /// <summary>
        /// 取得所有管理者
        /// </summary>
        Task<IEnumerable<ManagerData>> GetAllAsync();

        /// <summary>
        /// 檢查管理者是否存在
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// 新增管理者
        /// </summary>
        Task<ManagerData> Add(ManagerData manager);
    }
}
