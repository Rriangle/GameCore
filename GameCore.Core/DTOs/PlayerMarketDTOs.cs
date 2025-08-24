using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 商品管理 DTOs

    /// <summary>
    /// 自由市場商品列表 DTO
    /// </summary>
    public class PlayerMarketProductListDto
    {
        /// <summary>商品ID</summary>
        public int PProductId { get; set; }

        /// <summary>商品類型</summary>
        public string? PProductType { get; set; }

        /// <summary>商品標題 (噱頭標語)</summary>
        public string? PProductTitle { get; set; }

        /// <summary>商品名稱</summary>
        public string? PProductName { get; set; }

        /// <summary>售價</summary>
        public decimal Price { get; set; }

        /// <summary>商品狀態</summary>
        public string? PStatus { get; set; }

        /// <summary>賣家ID</summary>
        public int SellerId { get; set; }

        /// <summary>賣家名稱</summary>
        public string? SellerName { get; set; }

        /// <summary>建立時間</summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>主要商品圖片</summary>
        public string? MainImageUrl { get; set; }

        /// <summary>關聯的官方商品ID</summary>
        public int? ProductId { get; set; }

        /// <summary>關聯的官方商品名稱</summary>
        public string? OfficialProductName { get; set; }

        /// <summary>狀態顯示</summary>
        public string StatusDisplay => PStatus switch
        {
            "active" => "上架中",
            "sold" => "已售出",
            "removed" => "已下架",
            "suspended" => "已暫停",
            _ => PStatus ?? "未知"
        };
    }

    /// <summary>
    /// 自由市場商品詳細 DTO
    /// </summary>
    public class PlayerMarketProductDetailDto
    {
        /// <summary>商品ID</summary>
        public int PProductId { get; set; }

        /// <summary>商品類型</summary>
        public string? PProductType { get; set; }

        /// <summary>商品標題</summary>
        public string? PProductTitle { get; set; }

        /// <summary>商品名稱</summary>
        public string? PProductName { get; set; }

        /// <summary>商品描述</summary>
        public string? PProductDescription { get; set; }

        /// <summary>售價</summary>
        public decimal Price { get; set; }

        /// <summary>商品狀態</summary>
        public string? PStatus { get; set; }

        /// <summary>賣家資訊</summary>
        public MarketSellerDto? Seller { get; set; }

        /// <summary>商品圖片</summary>
        public List<ProductImageDto> Images { get; set; } = new();

        /// <summary>關聯的官方商品</summary>
        public OfficialProductReferenceDto? OfficialProduct { get; set; }

        /// <summary>建立時間</summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>更新時間</summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>是否可購買</summary>
        public bool CanBuy => PStatus == "active";
    }

    /// <summary>
    /// 市場賣家 DTO
    /// </summary>
    public class MarketSellerDto
    {
        /// <summary>賣家ID</summary>
        public int SellerId { get; set; }

        /// <summary>賣家名稱</summary>
        public string? SellerName { get; set; }

        /// <summary>賣家暱稱</summary>
        public string? SellerNickname { get; set; }

        /// <summary>銷售權限</summary>
        public bool HasSalesAuthority { get; set; }

        /// <summary>總銷售筆數</summary>
        public int TotalSales { get; set; }

        /// <summary>評價分數</summary>
        public decimal? Rating { get; set; }
    }

    /// <summary>
    /// 商品圖片 DTO
    /// </summary>
    public class ProductImageDto
    {
        /// <summary>圖片ID</summary>
        public int PProductImgId { get; set; }

        /// <summary>圖片URL (Base64 或路徑)</summary>
        public string? PProductImgUrl { get; set; }

        /// <summary>是否為主圖片</summary>
        public bool IsMain { get; set; }

        /// <summary>排序順序</summary>
        public int SortOrder { get; set; }
    }

    /// <summary>
    /// 官方商品參考 DTO
    /// </summary>
    public class OfficialProductReferenceDto
    {
        /// <summary>官方商品ID</summary>
        public int ProductId { get; set; }

        /// <summary>官方商品名稱</summary>
        public string? ProductName { get; set; }

        /// <summary>官方商品類型</summary>
        public string? ProductType { get; set; }

        /// <summary>官方售價</summary>
        public decimal? OfficialPrice { get; set; }
    }

    /// <summary>
    /// 建立商品請求 DTO
    /// </summary>
    public class CreatePlayerMarketProductDto
    {
        /// <summary>商品類型</summary>
        [Required(ErrorMessage = "商品類型為必填")]
        [StringLength(100, ErrorMessage = "商品類型長度不能超過100字元")]
        public string PProductType { get; set; } = string.Empty;

        /// <summary>商品標題</summary>
        [Required(ErrorMessage = "商品標題為必填")]
        [StringLength(200, ErrorMessage = "商品標題長度不能超過200字元")]
        public string PProductTitle { get; set; } = string.Empty;

        /// <summary>商品名稱</summary>
        [Required(ErrorMessage = "商品名稱為必填")]
        [StringLength(200, ErrorMessage = "商品名稱長度不能超過200字元")]
        public string PProductName { get; set; } = string.Empty;

        /// <summary>商品描述</summary>
        [StringLength(1000, ErrorMessage = "商品描述長度不能超過1000字元")]
        public string? PProductDescription { get; set; }

        /// <summary>售價</summary>
        [Required(ErrorMessage = "售價為必填")]
        [Range(0.01, 999999.99, ErrorMessage = "售價必須在0.01到999999.99之間")]
        public decimal Price { get; set; }

        /// <summary>關聯的官方商品ID (可選)</summary>
        public int? ProductId { get; set; }

        /// <summary>商品圖片 (Base64 編碼)</summary>
        public List<string> Images { get; set; } = new();
    }

    /// <summary>
    /// 更新商品請求 DTO
    /// </summary>
    public class UpdatePlayerMarketProductDto
    {
        /// <summary>商品標題</summary>
        [StringLength(200, ErrorMessage = "商品標題長度不能超過200字元")]
        public string? PProductTitle { get; set; }

        /// <summary>商品描述</summary>
        [StringLength(1000, ErrorMessage = "商品描述長度不能超過1000字元")]
        public string? PProductDescription { get; set; }

        /// <summary>售價</summary>
        [Range(0.01, 999999.99, ErrorMessage = "售價必須在0.01到999999.99之間")]
        public decimal? Price { get; set; }

        /// <summary>商品狀態</summary>
        [StringLength(50, ErrorMessage = "商品狀態長度不能超過50字元")]
        public string? PStatus { get; set; }
    }

    /// <summary>
    /// 商品搜尋請求 DTO
    /// </summary>
    public class PlayerMarketSearchDto
    {
        /// <summary>關鍵字</summary>
        public string? Keyword { get; set; }

        /// <summary>商品類型</summary>
        public string? PProductType { get; set; }

        /// <summary>最低價格</summary>
        [Range(0, double.MaxValue, ErrorMessage = "最低價格不能為負數")]
        public decimal? MinPrice { get; set; }

        /// <summary>最高價格</summary>
        [Range(0, double.MaxValue, ErrorMessage = "最高價格不能為負數")]
        public decimal? MaxPrice { get; set; }

        /// <summary>賣家ID</summary>
        public int? SellerId { get; set; }

        /// <summary>商品狀態</summary>
        public string? PStatus { get; set; }

        /// <summary>關聯的官方商品ID</summary>
        public int? ProductId { get; set; }

        /// <summary>頁碼</summary>
        [Range(1, int.MaxValue, ErrorMessage = "頁碼必須大於0")]
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數</summary>
        [Range(1, 100, ErrorMessage = "每頁筆數必須在1到100之間")]
        public int PageSize { get; set; } = 20;

        /// <summary>排序欄位</summary>
        public string SortBy { get; set; } = "CreatedAt";

        /// <summary>排序方向</summary>
        public string SortDirection { get; set; } = "desc";
    }

    #endregion

    #region 訂單管理 DTOs

    /// <summary>
    /// 自由市場訂單 DTO
    /// </summary>
    public class PlayerMarketOrderDto
    {
        /// <summary>訂單ID</summary>
        public int POrderId { get; set; }

        /// <summary>商品ID</summary>
        public int PProductId { get; set; }

        /// <summary>商品資訊</summary>
        public PlayerMarketProductListDto? Product { get; set; }

        /// <summary>賣家ID</summary>
        public int SellerId { get; set; }

        /// <summary>賣家名稱</summary>
        public string? SellerName { get; set; }

        /// <summary>買家ID</summary>
        public int BuyerId { get; set; }

        /// <summary>買家名稱</summary>
        public string? BuyerName { get; set; }

        /// <summary>訂單日期</summary>
        public DateTime POrderDate { get; set; }

        /// <summary>訂單狀態</summary>
        public string? POrderStatus { get; set; }

        /// <summary>付款狀態</summary>
        public string? PPaymentStatus { get; set; }

        /// <summary>單價</summary>
        public decimal PUnitPrice { get; set; }

        /// <summary>數量</summary>
        public int PQuantity { get; set; }

        /// <summary>總額</summary>
        public decimal POrderTotal { get; set; }

        /// <summary>建立時間</summary>
        public DateTime POrderCreatedAt { get; set; }

        /// <summary>更新時間</summary>
        public DateTime? POrderUpdatedAt { get; set; }

        /// <summary>交易頁面ID</summary>
        public int? TradepageId { get; set; }

        /// <summary>訂單狀態顯示</summary>
        public string OrderStatusDisplay => POrderStatus switch
        {
            "Created" => "已建立",
            "Trading" => "交易中",
            "Completed" => "已完成",
            "Cancelled" => "已取消",
            _ => POrderStatus ?? "未知"
        };

        /// <summary>付款狀態顯示</summary>
        public string PaymentStatusDisplay => PPaymentStatus switch
        {
            "Pending" => "待付款",
            "Paid" => "已付款",
            "N/A" => "道具交換",
            _ => PPaymentStatus ?? "未知"
        };
    }

    /// <summary>
    /// 建立訂單請求 DTO
    /// </summary>
    public class CreatePlayerMarketOrderDto
    {
        /// <summary>商品ID</summary>
        [Required(ErrorMessage = "商品ID為必填")]
        public int PProductId { get; set; }

        /// <summary>數量</summary>
        [Required(ErrorMessage = "數量為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於0")]
        public int PQuantity { get; set; } = 1;

        /// <summary>備註</summary>
        [StringLength(500, ErrorMessage = "備註長度不能超過500字元")]
        public string? Notes { get; set; }
    }

    #endregion

    #region 交易頁面 DTOs

    /// <summary>
    /// 交易頁面 DTO
    /// </summary>
    public class PlayerMarketTradepageDto
    {
        /// <summary>交易頁面ID</summary>
        public int POrderTradepageId { get; set; }

        /// <summary>訂單ID</summary>
        public int POrderId { get; set; }

        /// <summary>訂單資訊</summary>
        public PlayerMarketOrderDto? Order { get; set; }

        /// <summary>商品ID</summary>
        public int PProductId { get; set; }

        /// <summary>平台抽成</summary>
        public decimal POrderPlatformFee { get; set; }

        /// <summary>賣家移交時間</summary>
        public DateTime? SellerTransferredAt { get; set; }

        /// <summary>買家接收時間</summary>
        public DateTime? BuyerReceivedAt { get; set; }

        /// <summary>交易完成時間</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>交易訊息</summary>
        public List<TradeMessageDto> Messages { get; set; } = new();

        /// <summary>交易狀態</summary>
        public string TradeStatus
        {
            get
            {
                if (CompletedAt.HasValue) return "已完成";
                if (SellerTransferredAt.HasValue && BuyerReceivedAt.HasValue) return "待系統確認";
                if (SellerTransferredAt.HasValue) return "等待買家確認";
                if (BuyerReceivedAt.HasValue) return "等待賣家移交";
                return "交易中";
            }
        }

        /// <summary>可否確認移交</summary>
        public bool CanSellerTransfer => !SellerTransferredAt.HasValue && !CompletedAt.HasValue;

        /// <summary>可否確認接收</summary>
        public bool CanBuyerReceive => !BuyerReceivedAt.HasValue && !CompletedAt.HasValue;
    }

    /// <summary>
    /// 交易訊息 DTO
    /// </summary>
    public class TradeMessageDto
    {
        /// <summary>訊息ID</summary>
        public int TradeMsgId { get; set; }

        /// <summary>交易頁面ID</summary>
        public int POrderTradepageId { get; set; }

        /// <summary>訊息來源</summary>
        public string? MsgFrom { get; set; }

        /// <summary>訊息內容</summary>
        public string? MessageText { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>發送者顯示</summary>
        public string MsgFromDisplay => MsgFrom switch
        {
            "seller" => "賣家",
            "buyer" => "買家",
            "system" => "系統",
            _ => MsgFrom ?? "未知"
        };
    }

    /// <summary>
    /// 發送交易訊息請求 DTO
    /// </summary>
    public class SendTradeMessageDto
    {
        /// <summary>訊息內容</summary>
        [Required(ErrorMessage = "訊息內容為必填")]
        [StringLength(500, ErrorMessage = "訊息內容長度不能超過500字元")]
        public string MessageText { get; set; } = string.Empty;
    }

    /// <summary>
    /// 確認移交請求 DTO
    /// </summary>
    public class ConfirmTransferDto
    {
        /// <summary>確認備註</summary>
        [StringLength(200, ErrorMessage = "確認備註長度不能超過200字元")]
        public string? Notes { get; set; }
    }

    #endregion

    #region 排行榜 DTOs

    /// <summary>
    /// 自由市場排行榜 DTO
    /// </summary>
    public class PlayerMarketRankingDto
    {
        /// <summary>排行榜ID</summary>
        public int PRankingId { get; set; }

        /// <summary>榜單型態</summary>
        public string? PPeriodType { get; set; }

        /// <summary>排行日期</summary>
        public DateTime PRankingDate { get; set; }

        /// <summary>商品ID</summary>
        public int PProductId { get; set; }

        /// <summary>商品名稱</summary>
        public string? ProductName { get; set; }

        /// <summary>排名指標</summary>
        public string? PRankingMetric { get; set; }

        /// <summary>名次</summary>
        public int PRankingPosition { get; set; }

        /// <summary>交易額</summary>
        public decimal PTradingAmount { get; set; }

        /// <summary>賣家名稱</summary>
        public string? SellerName { get; set; }

        /// <summary>榜單型態顯示</summary>
        public string PeriodTypeDisplay => PPeriodType switch
        {
            "daily" => "日榜",
            "weekly" => "週榜",
            "monthly" => "月榜",
            "quarterly" => "季榜",
            "yearly" => "年榜",
            _ => PPeriodType ?? "未知"
        };

        /// <summary>排名指標顯示</summary>
        public string RankingMetricDisplay => PRankingMetric switch
        {
            "trading_amount" => "交易額",
            "trading_volume" => "交易量",
            "popularity" => "熱門度",
            _ => PRankingMetric ?? "未知"
        };
    }

    /// <summary>
    /// 排行榜查詢請求 DTO
    /// </summary>
    public class PlayerMarketRankingQueryDto
    {
        /// <summary>榜單型態</summary>
        public string PPeriodType { get; set; } = "daily";

        /// <summary>排名指標</summary>
        public string PRankingMetric { get; set; } = "trading_amount";

        /// <summary>查詢日期</summary>
        public DateTime? Date { get; set; }

        /// <summary>限制筆數</summary>
        [Range(1, 100, ErrorMessage = "限制筆數必須在1到100之間")]
        public int Limit { get; set; } = 50;
    }

    #endregion

    #region 統計 DTOs

    /// <summary>
    /// 自由市場統計 DTO
    /// </summary>
    public class PlayerMarketStatisticsDto
    {
        /// <summary>總商品數</summary>
        public int TotalProducts { get; set; }

        /// <summary>上架中商品數</summary>
        public int ActiveProducts { get; set; }

        /// <summary>總訂單數</summary>
        public int TotalOrders { get; set; }

        /// <summary>已完成訂單數</summary>
        public int CompletedOrders { get; set; }

        /// <summary>總交易額</summary>
        public decimal TotalTradingAmount { get; set; }

        /// <summary>平台總抽成</summary>
        public decimal TotalPlatformFees { get; set; }

        /// <summary>活躍賣家數</summary>
        public int ActiveSellers { get; set; }

        /// <summary>活躍買家數</summary>
        public int ActiveBuyers { get; set; }

        /// <summary>今日訂單數</summary>
        public int TodayOrders { get; set; }

        /// <summary>今日交易額</summary>
        public decimal TodayTradingAmount { get; set; }

        /// <summary>熱門商品類型</summary>
        public List<MarketCategoryStatsDto> CategoryStats { get; set; } = new();

        /// <summary>訂單完成率</summary>
        public double OrderCompletionRate => TotalOrders > 0 ? (double)CompletedOrders / TotalOrders * 100 : 0;

        /// <summary>商品上架率</summary>
        public double ProductActiveRate => TotalProducts > 0 ? (double)ActiveProducts / TotalProducts * 100 : 0;

        /// <summary>平均平台抽成率</summary>
        public double AveragePlatformFeeRate => TotalTradingAmount > 0 ? (double)(TotalPlatformFees / TotalTradingAmount) * 100 : 0;
    }

    /// <summary>
    /// 市場分類統計 DTO
    /// </summary>
    public class MarketCategoryStatsDto
    {
        /// <summary>分類名稱</summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>商品數量</summary>
        public int ProductCount { get; set; }

        /// <summary>訂單數量</summary>
        public int OrderCount { get; set; }

        /// <summary>交易額</summary>
        public decimal TradingAmount { get; set; }

        /// <summary>平均價格</summary>
        public decimal AveragePrice { get; set; }
    }

    #endregion

    #region 分頁結果

    /// <summary>
    /// 分頁結果 DTO
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class PlayerMarketPagedResult<T>
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

    #region 服務結果

    /// <summary>
    /// 自由市場服務執行結果
    /// </summary>
    public class PlayerMarketServiceResult
    {
        /// <summary>是否成功</summary>
        public bool Success { get; set; }

        /// <summary>訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>錯誤清單</summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>建立成功結果</summary>
        public static PlayerMarketServiceResult CreateSuccess(string message = "操作成功")
        {
            return new PlayerMarketServiceResult { Success = true, Message = message };
        }

        /// <summary>建立失敗結果</summary>
        public static PlayerMarketServiceResult CreateFailure(string message, List<string>? errors = null)
        {
            return new PlayerMarketServiceResult 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    /// <summary>
    /// 帶資料的自由市場服務執行結果
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class PlayerMarketServiceResult<T> : PlayerMarketServiceResult
    {
        /// <summary>結果資料</summary>
        public T? Data { get; set; }

        /// <summary>建立成功結果</summary>
        public static PlayerMarketServiceResult<T> CreateSuccess(T data, string message = "操作成功")
        {
            return new PlayerMarketServiceResult<T> 
            { 
                Success = true, 
                Message = message, 
                Data = data 
            };
        }

        /// <summary>建立失敗結果</summary>
        public static new PlayerMarketServiceResult<T> CreateFailure(string message, List<string>? errors = null)
        {
            return new PlayerMarketServiceResult<T> 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    #endregion
}