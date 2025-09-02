using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 小遊戲資料存取介面
    /// </summary>
    public interface IMiniGameRepository
    {
        /// <summary>
        /// 根據ID取得小遊戲記錄
        /// </summary>
        Task<MiniGame?> GetByIdAsync(int id);

        /// <summary>
        /// 根據用戶ID取得小遊戲記錄列表
        /// </summary>
        Task<IEnumerable<MiniGame>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 新增小遊戲記錄
        /// </summary>
        Task<MiniGame> AddAsync(MiniGame miniGame);

        /// <summary>
        /// 更新小遊戲記錄
        /// </summary>
        Task<MiniGame> UpdateAsync(MiniGame miniGame);

        /// <summary>
        /// 刪除小遊戲記錄
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 新增小遊戲記錄
        /// </summary>
        Task<MiniGame> Add(MiniGame miniGame);

        /// <summary>
        /// 檢查是否達到每日限制
        /// </summary>
        Task<bool> HasReachedDailyLimitAsync(int userId);

        /// <summary>
        /// 取得今日遊戲次數
        /// </summary>
        Task<int> GetTodayGameCountAsync(int userId);

        /// <summary>
        /// 取得遊戲設定
        /// </summary>
        Task<GameSettings?> GetGameSettingsAsync();

        /// <summary>
        /// 取得用戶遊戲記錄
        /// </summary>
        Task<IEnumerable<MiniGame>> GetUserGameRecordsAsync(int userId);

        /// <summary>
        /// 取得寵物遊戲記錄
        /// </summary>
        Task<IEnumerable<MiniGame>> GetPetGameRecordsAsync(int petId);
    }
}
