using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 市場商品 DTO
    /// </summary>
    public class MarketItemDto
    {
        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 賣家 ID
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品類型
        /// </summary>
        public string ItemType { get; set; } = string.Empty;

        /// <summary>
        /// 商品品質
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// 是否可交易
        /// </summary>
        public bool IsTradeable { get; set; }

        /// <summary>
        /// 上架時間
        /// </summary>
        public DateTime ListedAt { get; set; }

        /// <summary>
        /// 賣家名稱
        /// </summary>
        public string SellerName { get; set; } = string.Empty;
    }

    /// <summary>
    /// 市場商品創建請求 DTO
    /// </summary>
    public class MarketItemCreateDto
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// 商品類型
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ItemType { get; set; } = string.Empty;

        /// <summary>
        /// 商品品質
        /// </summary>
        [Range(1, 100)]
        public int Quality { get; set; } = 1;
    }

    /// <summary>
    /// 市場商品創建結果 DTO
    /// </summary>
    public class MarketItemCreateResult
    {
        /// <summary>
        /// 是否創建成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 創建的商品資訊
        /// </summary>
        public MarketItemDto? Item { get; set; }
    }

    /// <summary>
    /// 市場商品更新請求 DTO
    /// </summary>
    public class MarketItemUpdateDto
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        [StringLength(100)]
        public string? ItemName { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? Quantity { get; set; }

        /// <summary>
        /// 商品品質
        /// </summary>
        [Range(1, 100)]
        public int? Quality { get; set; }
    }

    /// <summary>
    /// 市場商品更新結果 DTO
    /// </summary>
    public class MarketItemUpdateResult
    {
        /// <summary>
        /// 是否更新成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 更新後的商品資訊
        /// </summary>
        public MarketItemDto? Item { get; set; }
    }
} 