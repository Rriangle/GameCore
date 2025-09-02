using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 寵物回應
    /// </summary>
    public class PetResponse
    {
        /// <summary>
        /// 寵物 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 寵物名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 寵物類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

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
        public int Mood { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 是否活躍
        /// </summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 寵物 DTO (舊版本相容性)
    /// </summary>
    public class PetDto : PetResponse
    {
    }

    /// <summary>
    /// 建立寵物請求
    /// </summary>
    public class CreatePetRequest
    {
        /// <summary>
        /// 寵物名稱
        /// </summary>
        [Required(ErrorMessage = "寵物名稱為必填")]
        [StringLength(50, ErrorMessage = "寵物名稱長度不能超過 50 字元")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 寵物類型
        /// </summary>
        [Required(ErrorMessage = "寵物類型為必填")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 更新寵物請求
    /// </summary>
    public class UpdatePetRequest
    {
        /// <summary>
        /// 寵物名稱
        /// </summary>
        [StringLength(50, ErrorMessage = "寵物名稱長度不能超過 50 字元")]
        public string? Name { get; set; }

        /// <summary>
        /// 寵物類型
        /// </summary>
        public string? Type { get; set; }
    }

    /// <summary>
    /// 寵物統計回應
    /// </summary>
    public class PetStatisticsDto
    {
        /// <summary>
        /// 總寵物數量
        /// </summary>
        public int TotalPets { get; set; }

        /// <summary>
        /// 活躍寵物數量
        /// </summary>
        public int ActivePets { get; set; }

        /// <summary>
        /// 平均等級
        /// </summary>
        public double AverageLevel { get; set; }

        /// <summary>
        /// 平均健康度
        /// </summary>
        public double AverageHealth { get; set; }

        /// <summary>
        /// 平均心情
        /// </summary>
        public double AverageMood { get; set; }
    }

    /// <summary>
    /// 餵食寵物請求
    /// </summary>
    public class FeedPetRequest
    {
        /// <summary>
        /// 寵物 ID
        /// </summary>
        [Required(ErrorMessage = "寵物 ID 為必填")]
        public int PetId { get; set; }

        /// <summary>
        /// 食物類型
        /// </summary>
        [Required(ErrorMessage = "食物類型為必填")]
        public string FoodType { get; set; } = string.Empty;
    }

    /// <summary>
    /// 與寵物玩耍請求
    /// </summary>
    public class PlayWithPetRequest
    {
        /// <summary>
        /// 寵物 ID
        /// </summary>
        [Required(ErrorMessage = "寵物 ID 為必填")]
        public int PetId { get; set; }

        /// <summary>
        /// 遊戲類型
        /// </summary>
        [Required(ErrorMessage = "遊戲類型為必填")]
        public string GameType { get; set; } = string.Empty;
    }
} 