using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 玩家市場項目回應
    /// </summary>
    public class PlayerMarketItemResponse
    {
        /// <summary>
        /// 項目 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 賣家 ID
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 賣家名稱
        /// </summary>
        public string SellerName { get; set; } = string.Empty;

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
        /// 商品類別
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 商品圖片 URL
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// 商品品質
        /// </summary>
        public string Quality { get; set; } = string.Empty;

        /// <summary>
        /// 是否可議價
        /// </summary>
        public bool IsNegotiable { get; set; }

        /// <summary>
        /// 上架時間
        /// </summary>
        public DateTime ListedAt { get; set; }

        /// <summary>
        /// 到期時間
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 玩家市場項目 DTO (舊版本相容性)
    /// </summary>
    public class PlayerMarketItemDto : PlayerMarketItemResponse
    {
    }

    /// <summary>
    /// 市場交易回應
    /// </summary>
    public class MarketTransactionResponse
    {
        /// <summary>
        /// 交易 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 買家 ID
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 買家名稱
        /// </summary>
        public string BuyerName { get; set; } = string.Empty;

        /// <summary>
        /// 賣家 ID
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 賣家名稱
        /// </summary>
        public string SellerName { get; set; } = string.Empty;

        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 市場交易 DTO (舊版本相容性)
    /// </summary>
    public class MarketTransactionDto : MarketTransactionResponse
    {
    }

    /// <summary>
    /// 市場評價回應
    /// </summary>
    public class MarketReviewResponse
    {
        /// <summary>
        /// 評價 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 交易 ID
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// 評價者 ID
        /// </summary>
        public int ReviewerId { get; set; }

        /// <summary>
        /// 評價者名稱
        /// </summary>
        public string ReviewerName { get; set; } = string.Empty;

        /// <summary>
        /// 被評價者 ID
        /// </summary>
        public int ReviewedId { get; set; }

        /// <summary>
        /// 被評價者名稱
        /// </summary>
        public string ReviewedName { get; set; } = string.Empty;

        /// <summary>
        /// 評分
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 評價內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 評價時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 市場評價 DTO (舊版本相容性)
    /// </summary>
    public class MarketReviewDto : MarketReviewResponse
    {
    }

    /// <summary>
    /// 上架商品請求
    /// </summary>
    public class ListItemRequest
    {
        /// <summary>
        /// 賣家 ID
        /// </summary>
        [Required(ErrorMessage = "賣家 ID 為必填")]
        public int SellerId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Required(ErrorMessage = "商品名稱為必填")]
        [StringLength(100, ErrorMessage = "商品名稱長度不能超過 100 字元")]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        [StringLength(1000, ErrorMessage = "商品描述長度不能超過 1000 字元")]
        public string? Description { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        [Required(ErrorMessage = "價格為必填")]
        [Range(0.01, double.MaxValue, ErrorMessage = "價格必須大於 0")]
        public decimal Price { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Required(ErrorMessage = "數量為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於 0")]
        public int Quantity { get; set; }

        /// <summary>
        /// 商品類別
        /// </summary>
        [Required(ErrorMessage = "商品類別為必填")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 商品品質
        /// </summary>
        public string Quality { get; set; } = string.Empty;

        /// <summary>
        /// 是否可議價
        /// </summary>
        public bool IsNegotiable { get; set; } = false;

        /// <summary>
        /// 商品圖片 URL
        /// </summary>
        [Url(ErrorMessage = "商品圖片 URL 格式不正確")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// 到期天數
        /// </summary>
        [Range(1, 365, ErrorMessage = "到期天數必須在 1-365 天之間")]
        public int ExpiryDays { get; set; } = 30;
    }

    /// <summary>
    /// 購買商品請求
    /// </summary>
    public class PurchaseItemRequest
    {
        /// <summary>
        /// 買家 ID
        /// </summary>
        [Required(ErrorMessage = "買家 ID 為必填")]
        public int BuyerId { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        [Required(ErrorMessage = "商品 ID 為必填")]
        public int ItemId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Required(ErrorMessage = "數量為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於 0")]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 搜尋商品請求
    /// </summary>
    public class SearchMarketItemsRequest
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// 最低價格
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "最低價格不能為負數")]
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// 最高價格
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "最高價格不能為負數")]
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// 品質
        /// </summary>
        public string? Quality { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "頁碼必須大於 0")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每頁大小
        /// </summary>
        [Range(1, 100, ErrorMessage = "每頁大小必須在 1-100 之間")]
        public int PageSize { get; set; } = 10;
    }
} 