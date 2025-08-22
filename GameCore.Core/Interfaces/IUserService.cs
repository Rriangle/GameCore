namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 使用者服務介面
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 取得使用者資料
        /// </summary>
        Task<GameCore.Core.Entities.User?> GetUserAsync(int userId);

        /// <summary>
        /// 更新使用者資料
        /// </summary>
        Task<bool> UpdateUserAsync(GameCore.Core.Entities.User user);
    }
}


