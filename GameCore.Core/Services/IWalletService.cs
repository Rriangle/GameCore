using GameCore.Core.DTOs;
using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 錢包服務介面 - 處理會員點數、交易流水和銷售錢包相關功能
    /// 按照規格要求，提供完整的點數系統、收支明細彙整和銷售管理功能
    /// </summary>
    public interface IWalletService
    {
        #region 基本錢包功能 (Basic Wallet Functions)
        
        /// <summary>
        /// 取得使用者錢包資訊，包含當前點數餘額和優惠券
        /// Get user wallet information including current point balance and coupons
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>錢包資訊</returns>
        Task<WalletDto> GetWalletAsync(int userId);

        /// <summary>
        /// 更新使用者點數餘額 (管理員功能)
        /// Update user point balance (Admin function)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="pointsDelta">點數變化量 (正數=增加, 負數=扣除)</param>
        /// <param name="reason">調整原因</param>
        /// <param name="adminId">操作管理員ID</param>
        /// <returns>更新後錢包資訊</returns>
        Task<WalletDto> AdjustPointsAsync(int userId, int pointsDelta, string reason, int adminId);

        /// <summary>
        /// 檢查使用者是否有足夠點數進行消費
        /// Check if user has sufficient points for spending
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="requiredPoints">需要的點數</param>
        /// <returns>是否有足夠點數</returns>
        Task<bool> HasSufficientPointsAsync(int userId, int requiredPoints);

        #endregion

        #region 收支明細彙整 (Transaction Ledger Aggregation)

        /// <summary>
        /// 取得使用者點數收支明細彙整 (依據規格從多個來源彙整)
        /// Get user point transaction ledger aggregated from multiple sources per spec
        /// 來源包含: 簽到(UserSignInStats)、小遊戲(MiniGame)、寵物換色(Pet)、管理調整(Notifications)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="request">查詢條件 (時間範圍、交易類型等)</param>
        /// <returns>分頁的交易明細清單</returns>
        Task<PagedResult<LedgerEntryDto>> GetLedgerHistoryAsync(int userId, LedgerQueryDto request);

        /// <summary>
        /// 取得使用者點數統計摘要 (本日、本週、本月的收入支出統計)
        /// Get user point statistics summary (daily, weekly, monthly income/expense stats)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>點數統計摘要</returns>
        Task<PointsStatisticsDto> GetPointsStatisticsAsync(int userId);

        #endregion

        #region 銷售功能管理 (Sales Management)

        /// <summary>
        /// 申請開通銷售功能 - 填寫銀行資料等
        /// Apply for sales functionality - fill in bank details etc.
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="salesProfileDto">銷售檔案資料</param>
        /// <returns>申請結果</returns>
        Task<ServiceResult<MemberSalesProfileDto>> ApplySalesProfileAsync(int userId, CreateSalesProfileDto salesProfileDto);

        /// <summary>
        /// 取得使用者銷售檔案資訊
        /// Get user sales profile information
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>銷售檔案資訊</returns>
        Task<MemberSalesProfileDto?> GetSalesProfileAsync(int userId);

        /// <summary>
        /// 取得使用者銷售錢包資訊 (銷售收入錢包)
        /// Get user sales wallet information (sales income wallet)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>銷售錢包資訊</returns>
        Task<UserSalesInformationDto?> GetSalesWalletAsync(int userId);

        /// <summary>
        /// 更新銷售錢包餘額 (交易結算時使用)
        /// Update sales wallet balance (used during transaction settlement)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="amount">金額變化 (正數=收入, 負數=提領)</param>
        /// <param name="source">來源說明</param>
        /// <returns>更新結果</returns>
        Task<ServiceResult<UserSalesInformationDto>> UpdateSalesWalletAsync(int userId, decimal amount, string source);

        #endregion

        #region 交易處理 (Transaction Processing)

        /// <summary>
        /// 執行點數消費交易 (如寵物換色、商城購買等)
        /// Execute point spending transaction (like pet color change, store purchase etc.)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="points">消費點數</param>
        /// <param name="purpose">消費目的</param>
        /// <param name="referenceId">關聯ID (如寵物ID、訂單ID等)</param>
        /// <returns>交易結果</returns>
        Task<ServiceResult<WalletTransactionDto>> SpendPointsAsync(int userId, int points, string purpose, string? referenceId = null);

        /// <summary>
        /// 執行點數獲得交易 (如簽到、小遊戲獎勵等)
        /// Execute point earning transaction (like sign-in, mini-game rewards etc.)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="points">獲得點數</param>
        /// <param name="source">來源說明</param>
        /// <param name="referenceId">關聯ID</param>
        /// <returns>交易結果</returns>
        Task<ServiceResult<WalletTransactionDto>> EarnPointsAsync(int userId, int points, string source, string? referenceId = null);

        #endregion
    }
}