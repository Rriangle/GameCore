using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 使用者 Repository 介面
    /// 定義使用者相關的資料存取方法
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 根據帳號取得使用者
        /// </summary>
        /// <param name="account">使用者帳號</param>
        /// <returns>使用者實體或 null</returns>
        Task<User?> GetByAccountAsync(string account);

        /// <summary>
        /// 根據 Email 取得使用者
        /// </summary>
        /// <param name="email">電子郵件</param>
        /// <returns>使用者實體或 null</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// 取得使用者完整資料 (包含所有關聯資料)
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>完整使用者資料</returns>
        Task<User?> GetUserWithAllDataAsync(int userId);

        /// <summary>
        /// 取得使用者錢包
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>錢包資料</returns>
        Task<UserWallet?> GetWalletAsync(int userId);

        /// <summary>
        /// 更新使用者錢包
        /// </summary>
        /// <param name="wallet">錢包資料</param>
        /// <returns>Task</returns>
        Task UpdateWalletAsync(UserWallet wallet);

        /// <summary>
        /// 增加使用者點數
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="points">要增加的點數</param>
        /// <param name="reason">增加原因</param>
        /// <returns>Task</returns>
        Task AddPointsAsync(int userId, int points, string reason);

        /// <summary>
        /// 扣除使用者點數
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="points">要扣除的點數</param>
        /// <param name="reason">扣除原因</param>
        /// <returns>是否成功</returns>
        Task<bool> DeductPointsAsync(int userId, int points, string reason);

        /// <summary>
        /// 取得使用者權限
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>權限資料</returns>
        Task<UserRights?> GetUserRightsAsync(int userId);

        /// <summary>
        /// 更新使用者權限
        /// </summary>
        /// <param name="userRights">權限資料</param>
        /// <returns>Task</returns>
        Task UpdateUserRightsAsync(UserRights userRights);

        /// <summary>
        /// 檢查帳號是否存在
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns>是否存在</returns>
        Task<bool> AccountExistsAsync(string account);

        /// <summary>
        /// 檢查 Email 是否存在
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>是否存在</returns>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// 檢查暱稱是否存在
        /// </summary>
        /// <param name="nickname">暱稱</param>
        /// <returns>是否存在</returns>
        Task<bool> NicknameExistsAsync(string nickname);

        /// <summary>
        /// 取得使用者點數排行榜
        /// </summary>
        /// <param name="top">取前幾名</param>
        /// <returns>排行榜</returns>
        Task<IEnumerable<UserWallet>> GetPointsLeaderboardAsync(int top = 10);

        /// <summary>
        /// 搜尋使用者 (根據暱稱或帳號)
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>搜尋結果</returns>
        Task<PagedResult<User>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20);
    }
}