using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 自由市場服務介面 - 完整實現C2C交易功能
    /// 提供商品上架、交易頁面管理、即時訊息、排行榜分析等完整C2C服務
    /// 嚴格按照規格要求實現自由市場業務邏輯和狀態機管理
    /// </summary>
    public interface IPlayerMarketService
    {
        #region 商品管理

        /// <summary>
        /// 搜尋市場商品
        /// 支援關鍵字、分類、價格區間、賣家、狀態等多維度搜尋
        /// </summary>
        /// <param name="searchDto">搜尋條件</param>
        /// <returns>分頁搜尋結果</returns>
        Task<PlayerMarketPagedResult<PlayerMarketProductListDto>> SearchProductsAsync(PlayerMarketSearchDto searchDto);

        /// <summary>
        /// 取得商品詳細資訊
        /// 包含基本資訊、賣家資料、商品圖片、關聯官方商品等完整資訊
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品詳細資訊</returns>
        Task<PlayerMarketProductDetailDto?> GetProductDetailAsync(int productId);

        /// <summary>
        /// 上架商品 (需要銷售權限)
        /// 檢查銷售權限、建立商品記錄、處理圖片上傳
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="createDto">建立商品請求</param>
        /// <returns>操作結果和商品資訊</returns>
        Task<PlayerMarketServiceResult<PlayerMarketProductDetailDto>> CreateProductAsync(int userId, CreatePlayerMarketProductDto createDto);

        /// <summary>
        /// 更新商品資訊 (僅限商品擁有者)
        /// 檢查擁有者權限、更新商品資訊、記錄異動日誌
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="updateDto">更新商品請求</param>
        /// <returns>操作結果和更新後商品資訊</returns>
        Task<PlayerMarketServiceResult<PlayerMarketProductDetailDto>> UpdateProductAsync(int userId, int productId, UpdatePlayerMarketProductDto updateDto);

        /// <summary>
        /// 上傳商品圖片
        /// 檢查擁有者權限、處理圖片上傳、更新圖片記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="images">圖片資料 (Base64)</param>
        /// <returns>操作結果和圖片列表</returns>
        Task<PlayerMarketServiceResult<List<ProductImageDto>>> UploadProductImagesAsync(int userId, int productId, List<string> images);

        /// <summary>
        /// 下架商品 (僅限商品擁有者)
        /// 更新商品狀態為removed、檢查是否有進行中的交易
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns>操作結果</returns>
        Task<PlayerMarketServiceResult> RemoveProductAsync(int userId, int productId);

        #endregion

        #region 訂單管理

        /// <summary>
        /// 建立訂單
        /// 檢查商品狀態、買家權限、建立訂單記錄
        /// </summary>
        /// <param name="userId">買家ID</param>
        /// <param name="createOrderDto">建立訂單請求</param>
        /// <returns>操作結果和訂單資訊</returns>
        Task<PlayerMarketServiceResult<PlayerMarketOrderDto>> CreateOrderAsync(int userId, CreatePlayerMarketOrderDto createOrderDto);

        /// <summary>
        /// 取得訂單詳細資訊
        /// 檢查使用者權限 (買家或賣家)，返回訂單完整資訊
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="orderId">訂單ID</param>
        /// <returns>訂單詳細資訊</returns>
        Task<PlayerMarketOrderDto?> GetOrderAsync(int userId, int orderId);

        /// <summary>
        /// 取得使用者訂單列表
        /// 支援買家/賣家角色篩選、狀態篩選、分頁排序
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="role">角色篩選 (buyer/seller)</param>
        /// <param name="status">訂單狀態篩選</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁訂單列表</returns>
        Task<PlayerMarketPagedResult<PlayerMarketOrderDto>> GetUserOrdersAsync(int userId, string? role, string? status, int page, int pageSize);

        /// <summary>
        /// 取消訂單 (僅限買家，且訂單未進入交易狀態)
        /// 檢查取消條件、更新訂單狀態、通知賣家
        /// </summary>
        /// <param name="userId">買家ID</param>
        /// <param name="orderId">訂單ID</param>
        /// <returns>操作結果</returns>
        Task<PlayerMarketServiceResult> CancelOrderAsync(int userId, int orderId);

        #endregion

        #region 交易頁面管理

        /// <summary>
        /// 建立交易頁面
        /// 從訂單建立交易頁面、計算平台抽成、初始化交易狀態
        /// </summary>
        /// <param name="userId">使用者ID (買家或賣家)</param>
        /// <param name="orderId">訂單ID</param>
        /// <returns>操作結果和交易頁面資訊</returns>
        Task<PlayerMarketServiceResult<PlayerMarketTradepageDto>> CreateTradepageAsync(int userId, int orderId);

        /// <summary>
        /// 取得交易頁面詳細資訊
        /// 檢查使用者權限、返回交易頁面和訊息記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="tradepageId">交易頁面ID</param>
        /// <returns>交易頁面詳細資訊</returns>
        Task<PlayerMarketTradepageDto?> GetTradepageAsync(int userId, int tradepageId);

        /// <summary>
        /// 發送交易訊息
        /// 檢查交易參與權限、新增訊息記錄、即時通知對方
        /// </summary>
        /// <param name="userId">發送者ID</param>
        /// <param name="tradepageId">交易頁面ID</param>
        /// <param name="messageDto">訊息內容</param>
        /// <returns>操作結果和訊息資訊</returns>
        Task<PlayerMarketServiceResult<TradeMessageDto>> SendTradeMessageAsync(int userId, int tradepageId, SendTradeMessageDto messageDto);

        /// <summary>
        /// 賣家確認移交
        /// 檢查賣家權限、更新移交時間、檢查是否可完成交易
        /// </summary>
        /// <param name="userId">賣家ID</param>
        /// <param name="tradepageId">交易頁面ID</param>
        /// <param name="confirmDto">確認資訊</param>
        /// <returns>操作結果和更新後交易頁面</returns>
        Task<PlayerMarketServiceResult<PlayerMarketTradepageDto>> ConfirmSellerTransferAsync(int userId, int tradepageId, ConfirmTransferDto confirmDto);

        /// <summary>
        /// 買家確認接收
        /// 檢查買家權限、更新接收時間、完成交易並處理入帳
        /// </summary>
        /// <param name="userId">買家ID</param>
        /// <param name="tradepageId">交易頁面ID</param>
        /// <param name="confirmDto">確認資訊</param>
        /// <returns>操作結果和更新後交易頁面</returns>
        Task<PlayerMarketServiceResult<PlayerMarketTradepageDto>> ConfirmBuyerReceivedAsync(int userId, int tradepageId, ConfirmTransferDto confirmDto);

        #endregion

        #region 排行榜

        /// <summary>
        /// 取得自由市場排行榜
        /// 從PlayerMarketRanking表查詢指定期間和指標的排行榜
        /// </summary>
        /// <param name="queryDto">查詢條件</param>
        /// <returns>排行榜列表</returns>
        Task<List<PlayerMarketRankingDto>> GetRankingsAsync(PlayerMarketRankingQueryDto queryDto);

        /// <summary>
        /// 更新自由市場排行榜
        /// 依日/月/季/年計算商品交易額、交易量，寫入PlayerMarketRanking表
        /// </summary>
        /// <param name="periodType">榜單型態</param>
        /// <param name="date">計算日期</param>
        /// <returns>更新結果</returns>
        Task<PlayerMarketServiceResult> UpdateRankingsAsync(string periodType, DateTime date);

        #endregion

        #region 統計分析

        /// <summary>
        /// 取得自由市場統計資訊
        /// 計算商品數量、訂單統計、交易額、平台抽成、分類分析等綜合指標
        /// </summary>
        /// <returns>市場統計資訊</returns>
        Task<PlayerMarketStatisticsDto> GetStatisticsAsync();

        /// <summary>
        /// 取得賣家統計資訊
        /// 計算賣家的銷售業績、商品統計、評價分析
        /// </summary>
        /// <param name="sellerId">賣家ID</param>
        /// <returns>賣家統計資訊</returns>
        Task<MarketSellerDto> GetSellerStatisticsAsync(int sellerId);

        #endregion

        #region 管理員功能

        /// <summary>
        /// 管理員更新商品狀態
        /// 更新商品狀態、記錄管理操作、通知相關使用者
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="statusDto">狀態更新請求</param>
        /// <returns>操作結果和更新後商品資訊</returns>
        Task<PlayerMarketServiceResult<PlayerMarketProductDetailDto>> UpdateProductStatusAsync(int productId, UpdateProductStatusDto statusDto);

        /// <summary>
        /// 管理員處理交易爭議
        /// 管理員介入處理交易糾紛、強制完成或取消交易
        /// </summary>
        /// <param name="tradepageId">交易頁面ID</param>
        /// <param name="resolution">解決方案</param>
        /// <param name="notes">處理備註</param>
        /// <returns>操作結果</returns>
        Task<PlayerMarketServiceResult> ResolveTradeDisputeAsync(int tradepageId, string resolution, string notes);

        #endregion

        #region 交易結算

        /// <summary>
        /// 完成交易結算
        /// 計算平台抽成、更新買賣雙方錢包、更新商品狀態、發送通知
        /// </summary>
        /// <param name="tradepageId">交易頁面ID</param>
        /// <returns>結算結果</returns>
        Task<PlayerMarketServiceResult> CompleteTradeSettlementAsync(int tradepageId);

        /// <summary>
        /// 計算平台抽成
        /// 根據交易金額和平台費率計算抽成金額
        /// </summary>
        /// <param name="tradeAmount">交易金額</param>
        /// <param name="productType">商品類型</param>
        /// <returns>平台抽成金額</returns>
        Task<decimal> CalculatePlatformFeeAsync(decimal tradeAmount, string productType);

        #endregion

        #region 權限檢查

        /// <summary>
        /// 檢查使用者銷售權限
        /// 從User_Rights表檢查SalesAuthority
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>是否有銷售權限</returns>
        Task<bool> CheckSalesAuthorityAsync(int userId);

        /// <summary>
        /// 檢查商品擁有者權限
        /// 驗證使用者是否為商品的擁有者
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns>是否為商品擁有者</returns>
        Task<bool> CheckProductOwnershipAsync(int userId, int productId);

        /// <summary>
        /// 檢查交易參與權限
        /// 驗證使用者是否為交易的買家或賣家
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="tradepageId">交易頁面ID</param>
        /// <returns>參與角色 (buyer/seller/none)</returns>
        Task<string> CheckTradeParticipationAsync(int userId, int tradepageId);

        #endregion

        #region 狀態機驗證

        /// <summary>
        /// 驗證訂單狀態轉換
        /// 按照狀態機規則驗證狀態轉換的合法性
        /// </summary>
        /// <param name="currentStatus">當前狀態</param>
        /// <param name="newStatus">新狀態</param>
        /// <returns>是否允許轉換</returns>
        bool ValidateOrderStatusTransition(string currentStatus, string newStatus);

        /// <summary>
        /// 驗證商品狀態轉換
        /// 按照業務規則驗證商品狀態轉換的合法性
        /// </summary>
        /// <param name="currentStatus">當前狀態</param>
        /// <param name="newStatus">新狀態</param>
        /// <returns>是否允許轉換</returns>
        bool ValidateProductStatusTransition(string currentStatus, string newStatus);

        #endregion

        #region 通知整合

        /// <summary>
        /// 發送交易相關通知
        /// 整合通知系統發送交易狀態變更、訊息等通知
        /// </summary>
        /// <param name="type">通知類型</param>
        /// <param name="userId">接收者ID</param>
        /// <param name="data">通知資料</param>
        /// <returns>發送結果</returns>
        Task<PlayerMarketServiceResult> SendTradeNotificationAsync(string type, int userId, object data);

        #endregion

        #region 圖片處理

        /// <summary>
        /// 處理商品圖片
        /// Base64解碼、圖片壓縮、存儲處理
        /// </summary>
        /// <param name="base64Images">Base64圖片資料</param>
        /// <returns>處理後的圖片URL列表</returns>
        Task<List<string>> ProcessProductImagesAsync(List<string> base64Images);

        /// <summary>
        /// 刪除商品圖片
        /// 從存儲中刪除指定的商品圖片
        /// </summary>
        /// <param name="imageUrls">圖片URL列表</param>
        /// <returns>刪除結果</returns>
        Task<PlayerMarketServiceResult> DeleteProductImagesAsync(List<string> imageUrls);

        #endregion
    }
}