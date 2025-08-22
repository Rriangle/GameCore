namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 小遊戲服務介面
    /// </summary>
    public interface IMiniGameService
    {
        /// <summary>
        /// 開始小遊戲
        /// </summary>
        Task<bool> StartGameAsync(int userId);

        /// <summary>
        /// 結束小遊戲
        /// </summary>
        Task<bool> EndGameAsync(int userId, string result);
    }
}


