using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 每日簽到服務介面
    /// 定義完整的簽到系統功能，包含Asia/Taipei時區處理、連續獎勵、月度統計等
    /// </summary>
    public interface IDailySignInService
    {
        /// <summary>
        /// 取得使用者今日簽到狀態和統計資訊
        /// 包含是否已簽到、當前連續天數、月度出席情況等
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>簽到狀態資訊</returns>
        Task<SignInStatusDto> GetSignInStatusAsync(int userId);

        /// <summary>
        /// 執行每日簽到
        /// 計算所有獎勵包含基礎獎勵、連續獎勵、月度獎勵
        /// 更新錢包點數和寵物經驗
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>簽到結果</returns>
        Task<SignInResultDto> PerformSignInAsync(int userId);

        /// <summary>
        /// 取得指定月份的簽到統計
        /// 包含簽到天數、獲得點數經驗、出席率等
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>月度簽到統計</returns>
        Task<MonthlyAttendanceDto> GetMonthAttendanceAsync(int userId, int year, int month);

        /// <summary>
        /// 取得使用者簽到歷史記錄 (分頁)
        /// 支援時間範圍篩選和排序
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="query">查詢條件</param>
        /// <returns>分頁的簽到記錄</returns>
        Task<PagedResult<SignInRecordDto>> GetSignInHistoryAsync(int userId, SignInHistoryQueryDto query);
    }
}