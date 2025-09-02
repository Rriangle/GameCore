using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 遊戲設定倉庫介面
    /// </summary>
    public interface IGameSettingsRepository : IRepository<GameSettings>
    {
        /// <summary>
        /// 根據設定名稱取得設定值
        /// </summary>
        /// <param name="settingName">設定名稱</param>
        /// <returns>設定值</returns>
        Task<string?> GetSettingValueAsync(string settingName);

        /// <summary>
        /// 設定設定值
        /// </summary>
        /// <param name="settingName">設定名稱</param>
        /// <param name="settingValue">設定值</param>
        /// <returns>設定結果</returns>
        Task<bool> SetSettingValueAsync(string settingName, string settingValue);

        /// <summary>
        /// 取得所有啟用的設定
        /// </summary>
        /// <returns>設定列表</returns>
        Task<IEnumerable<GameSettings>> GetActiveSettingsAsync();

        /// <summary>
        /// 檢查設定是否存在
        /// </summary>
        /// <param name="settingName">設定名稱</param>
        /// <returns>是否存在</returns>
        Task<bool> SettingExistsAsync(string settingName);

        /// <summary>
        /// 刪除設定
        /// </summary>
        /// <param name="settingName">設定名稱</param>
        /// <returns>刪除結果</returns>
        Task<bool> DeleteSettingAsync(string settingName);

        /// <summary>
        /// 取得遊戲設定
        /// </summary>
        Task<GameSettings?> GetGameSettingsAsync();

        /// <summary>
        /// 取得用戶遊戲記錄
        /// </summary>
        Task<IEnumerable<MiniGameRecord>> GetUserGameRecordsAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 取得寵物遊戲記錄
        /// </summary>
        Task<IEnumerable<MiniGameRecord>> GetPetGameRecordsAsync(int petId, int page, int pageSize);

        /// <summary>
        /// 檢查是否達到每日限制
        /// </summary>
        Task<bool> HasReachedDailyLimitAsync(int userId, int gameId);

        /// <summary>
        /// 取得今日遊戲次數
        /// </summary>
        Task<int> GetTodayGameCountAsync(int userId, int gameId);
    }
} 
