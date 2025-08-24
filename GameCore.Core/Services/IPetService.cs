using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 虛擬寵物服務介面
    /// 定義完整的史萊姆寵物系統功能，包含5維屬性管理、互動行為、等級系統、換色功能等
    /// 嚴格按照規格實現一人一寵、每日衰減、健康檢查等業務邏輯
    /// </summary>
    public interface IPetService
    {
        #region 寵物基本管理

        /// <summary>
        /// 取得使用者的寵物資訊
        /// 包含完整的5維屬性、等級經驗、顏色設定等
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>寵物完整資訊</returns>
        Task<PetDto?> GetUserPetAsync(int userId);

        /// <summary>
        /// 為使用者建立新寵物 (一人一寵規則)
        /// 初始化所有5維屬性為100，等級1，經驗0
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="petName">寵物名稱 (預設: 小可愛)</param>
        /// <returns>建立的寵物資訊</returns>
        Task<PetDto> CreatePetAsync(int userId, string petName = "小可愛");

        /// <summary>
        /// 更新寵物基本資料 (名稱、外觀等，不含扣點換色)
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="updateRequest">更新請求</param>
        /// <returns>更新結果</returns>
        Task<ServiceResult<PetDto>> UpdatePetProfileAsync(int userId, UpdatePetProfileDto updateRequest);

        #endregion

        #region 寵物互動行為

        /// <summary>
        /// 餵食寵物 - 飢餓值 +10
        /// 包含屬性鉗位、健康檢查、升級處理
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>互動結果</returns>
        Task<PetInteractionResultDto> FeedPetAsync(int userId);

        /// <summary>
        /// 幫寵物洗澡 - 清潔值 +10
        /// 包含屬性鉗位、健康檢查、升級處理
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>互動結果</returns>
        Task<PetInteractionResultDto> BathePetAsync(int userId);

        /// <summary>
        /// 與寵物玩耍 - 心情值 +10
        /// 包含屬性鉗位、健康檢查、升級處理
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>互動結果</returns>
        Task<PetInteractionResultDto> PlayWithPetAsync(int userId);

        /// <summary>
        /// 讓寵物休息 - 體力值 +10
        /// 包含屬性鉗位、健康檢查、升級處理
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>互動結果</returns>
        Task<PetInteractionResultDto> RestPetAsync(int userId);

        #endregion

        #region 寵物顏色系統

        /// <summary>
        /// 取得可用的寵物顏色選項
        /// </summary>
        /// <returns>顏色選項列表</returns>
        Task<List<PetColorOptionDto>> GetAvailableColorsAsync();

        /// <summary>
        /// 寵物換色 (消耗2000點數)
        /// 包含點數檢查、扣款、通知發送
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="recolorRequest">換色請求</param>
        /// <returns>換色結果</returns>
        Task<ServiceResult<PetDto>> RecolorPetAsync(int userId, PetRecolorDto recolorRequest);

        /// <summary>
        /// 取得寵物換色歷史 (從通知記錄)
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>換色歷史</returns>
        Task<List<PetColorHistoryDto>> GetColorHistoryAsync(int userId);

        #endregion

        #region 寵物等級與經驗

        /// <summary>
        /// 為寵物增加經驗值
        /// 自動處理升級邏輯和獎勵發放
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="experience">增加的經驗值</param>
        /// <param name="source">經驗來源</param>
        /// <returns>經驗增加結果</returns>
        Task<PetExperienceResultDto> AddExperienceAsync(int userId, int experience, string source);

        /// <summary>
        /// 計算升級所需經驗值
        /// </summary>
        /// <param name="currentLevel">當前等級</param>
        /// <returns>升級所需經驗值</returns>
        int CalculateRequiredExperience(int currentLevel);

        /// <summary>
        /// 取得寵物等級統計
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>等級統計資訊</returns>
        Task<PetLevelStatsDto> GetLevelStatsAsync(int userId);

        #endregion

        #region 每日維護

        /// <summary>
        /// 執行每日屬性衰減 (Asia/Taipei 00:00)
        /// Hunger -20, Mood -30, Stamina -10, Cleanliness -20, Health -20
        /// </summary>
        /// <param name="userId">使用者ID (null表示所有使用者)</param>
        /// <returns>衰減處理結果</returns>
        Task<PetDailyDecayResultDto> ProcessDailyDecayAsync(int? userId = null);

        /// <summary>
        /// 檢查寵物是否可以進行冒險 (健康度和屬性檢查)
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>冒險可用性檢查結果</returns>
        Task<PetAdventureReadinessDto> CheckAdventureReadinessAsync(int userId);

        #endregion

        #region 寵物統計與分析

        /// <summary>
        /// 取得寵物完整統計資訊
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>寵物統計</returns>
        Task<PetStatsDto> GetPetStatisticsAsync(int userId);

        /// <summary>
        /// 取得所有寵物的排行榜資訊
        /// </summary>
        /// <param name="rankingType">排行類型 (level, experience, health等)</param>
        /// <param name="limit">限制筆數</param>
        /// <returns>排行榜資料</returns>
        Task<List<PetRankingDto>> GetPetRankingsAsync(PetRankingType rankingType, int limit = 50);

        #endregion

        #region 管理員功能

        /// <summary>
        /// 管理員重置寵物屬性
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="resetRequest">重置請求</param>
        /// <returns>重置結果</returns>
        Task<ServiceResult<PetDto>> AdminResetPetAsync(int petId, PetAdminResetDto resetRequest);

        /// <summary>
        /// 管理員取得寵物系統全域設定
        /// </summary>
        /// <returns>系統設定</returns>
        Task<PetSystemConfigDto> GetSystemConfigAsync();

        /// <summary>
        /// 管理員更新寵物系統設定
        /// </summary>
        /// <param name="config">新設定</param>
        /// <returns>更新結果</returns>
        Task<ServiceResult> UpdateSystemConfigAsync(PetSystemConfigDto config);

        #endregion
    }
}