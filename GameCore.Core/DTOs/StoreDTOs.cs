using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 商品管理 DTOs

    /// <summary>
    /// 商品列表 DTO
    /// </summary>
    public class ProductListDto
    {
        /// <summary>商品ID</summary>
        public int ProductId { get; set; }

        /// <summary>商品名稱</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>商品類型</summary>
        public string ProductType { get; set; } = string.Empty;

        /// <summary>售價</summary>
        public decimal Price { get; set; }

        /// <summary>貨幣代碼</summary>
        public string CurrencyCode { get; set; } = string.Empty;

        /// <summary>庫存數量</summary>
        public int StockQuantity { get; set; }

        /// <summary>供應商名稱</summary>
        public string? SupplierName { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>是否有庫存</summary>
        public bool InStock => StockQuantity > 0;

        /// <summary>顯示價格</summary>
        public string DisplayPrice => $"{Price:F2} {CurrencyCode}";
    }

    /// <summary>
    /// 商品詳細資訊 DTO
    /// </summary>
    public class ProductDetailDto
    {
        /// <summary>商品ID</summary>
        public int ProductId { get; set; }

        /// <summary>商品名稱</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>商品類型</summary>
        public string ProductType { get; set; } = string.Empty;

        /// <summary>商品描述</summary>
        public string? ProductDescription { get; set; }

        /// <summary>售價</summary>
        public decimal Price { get; set; }

        /// <summary>貨幣代碼</summary>
        public string CurrencyCode { get; set; } = string.Empty;

        /// <summary>庫存數量</summary>
        public int StockQuantity { get; set; }

        /// <summary>供應商資訊</summary>
        public SupplierDto? Supplier { get; set; }

        /// <summary>遊戲商品詳細 (若為遊戲類型)</summary>
        public GameProductDetailDto? GameDetails { get; set; }

        /// <summary>其他商品詳細 (若為非遊戲類型)</summary>
        public OtherProductDetailDto? OtherDetails { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>更新時間</summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>是否有庫存</summary>
        public bool InStock => StockQuantity > 0;

        /// <summary>顯示價格</summary>
        public string DisplayPrice => $"{Price:F2} {CurrencyCode}";
    }

    /// <summary>
    /// 供應商 DTO
    /// </summary>
    public class SupplierDto
    {
        /// <summary>供應商ID</summary>
        public int SupplierId { get; set; }

        /// <summary>供應商名稱</summary>
        public string? SupplierName { get; set; }
    }

    /// <summary>
    /// 遊戲商品詳細 DTO
    /// </summary>
    public class GameProductDetailDto
    {
        /// <summary>商品ID</summary>
        public int ProductId { get; set; }

        /// <summary>商品名稱</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>商品描述</summary>
        public string? ProductDescription { get; set; }

        /// <summary>遊戲平台ID</summary>
        public int? PlatformId { get; set; }

        /// <summary>遊戲ID</summary>
        public int? GameId { get; set; }

        /// <summary>下載連結</summary>
        public string? DownloadLink { get; set; }
    }

    /// <summary>
    /// 其他商品詳細 DTO
    /// </summary>
    public class OtherProductDetailDto
    {
        /// <summary>商品ID</summary>
        public int ProductId { get; set; }

        /// <summary>商品名稱</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>商品描述</summary>
        public string? ProductDescription { get; set; }

        /// <summary>數位序號兌換碼</summary>
        public string? DigitalCode { get; set; }

        /// <summary>尺寸</summary>
        public string? Size { get; set; }

        /// <summary>顏色</summary>
        public string? Color { get; set; }

        /// <summary>材質</summary>
        public string? Material { get; set; }
    }

    #endregion

    #region 購物車 DTOs

    /// <summary>
    /// 購物車項目 DTO
    /// </summary>
    public class CartItemDto
    {
        /// <summary>商品ID</summary>
        public int ProductId { get; set; }

        /// <summary>商品名稱</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>單價</summary>
        public decimal UnitPrice { get; set; }

        /// <summary>數量</summary>
        public int Quantity { get; set; }

        /// <summary>小計</summary>
        public decimal Subtotal => UnitPrice * Quantity;

        /// <summary>貨幣代碼</summary>
        public string CurrencyCode { get; set; } = string.Empty;

        /// <summary>庫存數量</summary>
        public int StockQuantity { get; set; }

        /// <summary>是否有足夠庫存</summary>
        public bool HasSufficientStock => Quantity <= StockQuantity;

        /// <summary>顯示小計</summary>
        public string DisplaySubtotal => $"{Subtotal:F2} {CurrencyCode}";
    }

    /// <summary>
    /// 購物車 DTO
    /// </summary>
    public class ShoppingCartDto
    {
        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>購物車項目</summary>
        public List<CartItemDto> Items { get; set; } = new();

        /// <summary>總計</summary>
        public decimal Total => Items.Sum(item => item.Subtotal);

        /// <summary>項目數量</summary>
        public int ItemCount => Items.Sum(item => item.Quantity);

        /// <summary>是否為空</summary>
        public bool IsEmpty => !Items.Any();

        /// <summary>是否有庫存問題</summary>
        public bool HasStockIssues => Items.Any(item => !item.HasSufficientStock);

        /// <summary>顯示總計</summary>
        public string DisplayTotal => Items.Any() ? $"{Total:F2} {Items.First().CurrencyCode}" : "0.00";
    }

    /// <summary>
    /// 加入購物車請求 DTO
    /// </summary>
    public class AddToCartDto
    {
        /// <summary>商品ID</summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>數量</summary>
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於0")]
        public int Quantity { get; set; } = 1;
    }

    /// <summary>
    /// 更新購物車項目請求 DTO
    /// </summary>
    public class UpdateCartItemDto
    {
        /// <summary>商品ID</summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>新數量</summary>
        [Range(0, int.MaxValue, ErrorMessage = "數量不能為負數")]
        public int Quantity { get; set; }
    }

    #endregion

    #region 訂單 DTOs

    /// <summary>
    /// 訂單資訊 DTO
    /// </summary>
    public class OrderDto
    {
        /// <summary>訂單ID</summary>
        public int OrderId { get; set; }

        /// <summary>下單會員ID</summary>
        public int UserId { get; set; }

        /// <summary>下單日期</summary>
        public DateTime OrderDate { get; set; }

        /// <summary>訂單狀態</summary>
        public string OrderStatus { get; set; } = string.Empty;

        /// <summary>付款狀態</summary>
        public string PaymentStatus { get; set; } = string.Empty;

        /// <summary>訂單總額</summary>
        public decimal OrderTotal { get; set; }

        /// <summary>付款時間</summary>
        public DateTime? PaymentAt { get; set; }

        /// <summary>出貨時間</summary>
        public DateTime? ShippedAt { get; set; }

        /// <summary>完成時間</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>訂單項目</summary>
        public List<OrderItemDto> Items { get; set; } = new();

        /// <summary>訂單狀態顯示</summary>
        public string OrderStatusDisplay => OrderStatus switch
        {
            "Created" => "已建立",
            "ToShip" => "待出貨",
            "Shipped" => "已出貨",
            "Completed" => "已完成",
            "Cancelled" => "已取消",
            _ => OrderStatus
        };

        /// <summary>付款狀態顯示</summary>
        public string PaymentStatusDisplay => PaymentStatus switch
        {
            "Placed" => "下單",
            "Pending" => "待付款",
            "Paid" => "已付款",
            "Failed" => "付款失敗",
            _ => PaymentStatus
        };
    }

    /// <summary>
    /// 訂單項目 DTO
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>項目ID</summary>
        public int ItemId { get; set; }

        /// <summary>訂單ID</summary>
        public int OrderId { get; set; }

        /// <summary>商品ID</summary>
        public int ProductId { get; set; }

        /// <summary>商品名稱</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>項目編號</summary>
        public int LineNo { get; set; }

        /// <summary>單價</summary>
        public decimal UnitPrice { get; set; }

        /// <summary>數量</summary>
        public int Quantity { get; set; }

        /// <summary>小計</summary>
        public decimal Subtotal { get; set; }

        /// <summary>貨幣代碼</summary>
        public string CurrencyCode { get; set; } = string.Empty;

        /// <summary>顯示小計</summary>
        public string DisplaySubtotal => $"{Subtotal:F2} {CurrencyCode}";
    }

    /// <summary>
    /// 建立訂單請求 DTO
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>訂單項目</summary>
        [Required]
        [MinLength(1, ErrorMessage = "訂單必須包含至少一個項目")]
        public List<CreateOrderItemDto> Items { get; set; } = new();

        /// <summary>備註</summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 建立訂單項目請求 DTO
    /// </summary>
    public class CreateOrderItemDto
    {
        /// <summary>商品ID</summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>數量</summary>
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於0")]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 訂單查詢請求 DTO
    /// </summary>
    public class OrderQueryDto
    {
        /// <summary>訂單狀態篩選</summary>
        public string? OrderStatus { get; set; }

        /// <summary>付款狀態篩選</summary>
        public string? PaymentStatus { get; set; }

        /// <summary>開始日期</summary>
        public DateTime? FromDate { get; set; }

        /// <summary>結束日期</summary>
        public DateTime? ToDate { get; set; }

        /// <summary>頁碼</summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數</summary>
        [Range(1, 100)]
        public int PageSize { get; set; } = 20;

        /// <summary>排序欄位</summary>
        public string SortBy { get; set; } = "OrderDate";

        /// <summary>排序方向</summary>
        public string SortDirection { get; set; } = "desc";
    }

    #endregion

    #region 排行榜 DTOs

    /// <summary>
    /// 商城排行榜 DTO
    /// </summary>
    public class StoreRankingDto
    {
        /// <summary>排行榜ID</summary>
        public int RankingId { get; set; }

        /// <summary>榜單型態</summary>
        public string PeriodType { get; set; } = string.Empty;

        /// <summary>榜單日期</summary>
        public DateTime RankingDate { get; set; }

        /// <summary>商品ID</summary>
        public int ProductId { get; set; }

        /// <summary>商品名稱</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>排名指標</summary>
        public string RankingMetric { get; set; } = string.Empty;

        /// <summary>名次</summary>
        public int RankingPosition { get; set; }

        /// <summary>交易額</summary>
        public decimal TradingAmount { get; set; }

        /// <summary>榜單型態顯示</summary>
        public string PeriodTypeDisplay => PeriodType switch
        {
            "daily" => "日榜",
            "weekly" => "週榜",
            "monthly" => "月榜",
            "quarterly" => "季榜",
            "yearly" => "年榜",
            _ => PeriodType
        };

        /// <summary>排名指標顯示</summary>
        public string RankingMetricDisplay => RankingMetric switch
        {
            "trading_amount" => "交易額",
            "trading_volume" => "交易量",
            "popularity" => "熱門度",
            _ => RankingMetric
        };
    }

    /// <summary>
    /// 排行榜查詢請求 DTO
    /// </summary>
    public class RankingQueryDto
    {
        /// <summary>榜單型態</summary>
        public string PeriodType { get; set; } = "daily";

        /// <summary>排名指標</summary>
        public string RankingMetric { get; set; } = "trading_amount";

        /// <summary>查詢日期</summary>
        public DateTime? Date { get; set; }

        /// <summary>限制筆數</summary>
        [Range(1, 100)]
        public int Limit { get; set; } = 50;
    }

    #endregion

    #region 商品搜尋 DTOs

    /// <summary>
    /// 商品搜尋請求 DTO
    /// </summary>
    public class ProductSearchDto
    {
        /// <summary>關鍵字</summary>
        public string? Keyword { get; set; }

        /// <summary>商品類型</summary>
        public string? ProductType { get; set; }

        /// <summary>最低價格</summary>
        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }

        /// <summary>最高價格</summary>
        [Range(0, double.MaxValue)]
        public decimal? MaxPrice { get; set; }

        /// <summary>貨幣代碼</summary>
        public string? CurrencyCode { get; set; }

        /// <summary>供應商ID</summary>
        public int? SupplierId { get; set; }

        /// <summary>是否有庫存</summary>
        public bool? InStock { get; set; }

        /// <summary>頁碼</summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數</summary>
        [Range(1, 100)]
        public int PageSize { get; set; } = 20;

        /// <summary>排序欄位</summary>
        public string SortBy { get; set; } = "CreatedAt";

        /// <summary>排序方向</summary>
        public string SortDirection { get; set; } = "desc";
    }

    #endregion

    #region 統計 DTOs

    /// <summary>
    /// 商城統計 DTO
    /// </summary>
    public class StoreStatisticsDto
    {
        /// <summary>總商品數</summary>
        public int TotalProducts { get; set; }

        /// <summary>有庫存商品數</summary>
        public int ProductsInStock { get; set; }

        /// <summary>總訂單數</summary>
        public int TotalOrders { get; set; }

        /// <summary>已完成訂單數</summary>
        public int CompletedOrders { get; set; }

        /// <summary>總交易額</summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>今日訂單數</summary>
        public int TodayOrders { get; set; }

        /// <summary>今日交易額</summary>
        public decimal TodayRevenue { get; set; }

        /// <summary>熱門商品類型</summary>
        public List<CategoryStatsDto> CategoryStats { get; set; } = new();

        /// <summary>訂單完成率</summary>
        public double OrderCompletionRate => TotalOrders > 0 ? (double)CompletedOrders / TotalOrders * 100 : 0;

        /// <summary>庫存率</summary>
        public double StockRate => TotalProducts > 0 ? (double)ProductsInStock / TotalProducts * 100 : 0;
    }

    /// <summary>
    /// 分類統計 DTO
    /// </summary>
    public class CategoryStatsDto
    {
        /// <summary>分類名稱</summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>商品數量</summary>
        public int ProductCount { get; set; }

        /// <summary>訂單數量</summary>
        public int OrderCount { get; set; }

        /// <summary>交易額</summary>
        public decimal Revenue { get; set; }
    }

    #endregion

    #region 分頁結果

    /// <summary>
    /// 分頁結果 DTO
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class PagedResult<T>
    {
        /// <summary>當前頁碼</summary>
        public int Page { get; set; }

        /// <summary>每頁筆數</summary>
        public int PageSize { get; set; }

        /// <summary>總筆數</summary>
        public int TotalCount { get; set; }

        /// <summary>總頁數</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>是否有上一頁</summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>是否有下一頁</summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>資料列表</summary>
        public List<T> Data { get; set; } = new();

        /// <summary>是否為空結果</summary>
        public bool IsEmpty => !Data.Any();
    }

    #endregion
}