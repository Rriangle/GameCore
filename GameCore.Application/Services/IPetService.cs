using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 寵物服務介面
    /// </summary>
    public interface IPetService
    {
        /// <summary>
        /// 取得用戶的寵物列表
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>寵物列表</returns>
        Task<Result<IEnumerable<PetResponse>>> GetUserPetsAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得寵物詳情
        /// </summary>
        /// <param name="petId">寵物 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>寵物詳情</returns>
        Task<Result<PetResponse>> GetPetAsync(int petId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 建立寵物
        /// </summary>
        /// <param name="request">建立寵物請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>寵物資訊</returns>
        Task<Result<PetResponse>> CreatePetAsync(CreatePetRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新寵物
        /// </summary>
        /// <param name="petId">寵物 ID</param>
        /// <param name="request">更新寵物請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新結果</returns>
        Task<Result<PetResponse>> UpdatePetAsync(int petId, UpdatePetRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 餵食寵物
        /// </summary>
        /// <param name="petId">寵物 ID</param>
        /// <param name="foodType">食物類型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>餵食結果</returns>
        Task<Result<PetStatsResponse>> FeedPetAsync(int petId, string foodType, CancellationToken cancellationToken = default);

        /// <summary>
        /// 與寵物玩耍
        /// </summary>
        /// <param name="petId">寵物 ID</param>
        /// <param name="gameType">遊戲類型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>玩耍結果</returns>
        Task<Result<PetStatsResponse>> PlayWithPetAsync(int petId, string gameType, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得寵物統計
        /// </summary>
        /// <param name="petId">寵物 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>寵物統計</returns>
        Task<Result<PetStatsResponse>> GetPetStatsAsync(int petId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 寵物回應
    /// </summary>
    public class PetResponse
    {
        /// <summary>
        /// 寵物 ID
        /// </summary>
        public int PetId { get; set; }

        /// <summary>
        /// 寵物名稱
        /// </summary>
        public string PetName { get; set; } = string.Empty;

        /// <summary>
        /// 寵物類型
        /// </summary>
        public string PetType { get; set; } = string.Empty;

        /// <summary>
        /// 等級
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 經驗值
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// 健康度
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// 飢餓度
        /// </summary>
        public int Hunger { get; set; }

        /// <summary>
        /// 心情
        /// </summary>
        public int Happiness { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? LastUpdated { get; set; }
    }

    /// <summary>
    /// 建立寵物請求
    /// </summary>
    public class CreatePetRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 寵物名稱
        /// </summary>
        public string PetName { get; set; } = string.Empty;

        /// <summary>
        /// 寵物類型
        /// </summary>
        public string PetType { get; set; } = string.Empty;
    }

    /// <summary>
    /// 更新寵物請求
    /// </summary>
    public class UpdatePetRequest
    {
        /// <summary>
        /// 寵物名稱
        /// </summary>
        public string? PetName { get; set; }
    }

    /// <summary>
    /// 寵物統計回應
    /// </summary>
    public class PetStatsResponse
    {
        /// <summary>
        /// 寵物 ID
        /// </summary>
        public int PetId { get; set; }

        /// <summary>
        /// 寵物名稱
        /// </summary>
        public string PetName { get; set; } = string.Empty;

        /// <summary>
        /// 等級
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 經驗值
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// 健康度
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// 飢餓度
        /// </summary>
        public int Hunger { get; set; }

        /// <summary>
        /// 心情
        /// </summary>
        public int Happiness { get; set; }

        /// <summary>
        /// 總遊戲次數
        /// </summary>
        public int TotalGamesPlayed { get; set; }

        /// <summary>
        /// 總餵食次數
        /// </summary>
        public int TotalFeedingCount { get; set; }

        /// <summary>
        /// 最後餵食時間
        /// </summary>
        public DateTime? LastFedAt { get; set; }

        /// <summary>
        /// 最後遊戲時間
        /// </summary>
        public DateTime? LastPlayedAt { get; set; }
    }
} 