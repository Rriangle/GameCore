using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 管理員服務介面 - 完整實現後台管理功能
    /// 提供管理員認證、角色權限管理、使用者治理、系統監控等完整後台管理功能
    /// 嚴格按照規格要求實現ManagerData、ManagerRolePermission、ManagerRole的完整管理體系
    /// </summary>
    public interface IAdminService
    {
        #region 管理員認證與基本管理

        /// <summary>
        /// 管理員登入
        /// 驗證帳號密碼，更新last_login，產生JWT Token
        /// </summary>
        /// <param name="loginDto">登入請求</param>
        /// <returns>登入結果包含Token和權限資訊</returns>
        Task<AdminServiceResult<ManagerLoginResponseDto>> LoginAsync(ManagerLoginDto loginDto);

        /// <summary>
        /// 取得管理員列表
        /// 支援分頁和條件篩選
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="activeOnly">是否只顯示啟用的管理員</param>
        /// <returns>分頁管理員列表</returns>
        Task<AdminPagedResult<ManagerDataDto>> GetManagersAsync(int page = 1, int pageSize = 20, bool activeOnly = true);

        /// <summary>
        /// 取得管理員詳細資訊
        /// 包含角色權限和最後登入資訊
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <returns>管理員詳細資訊</returns>
        Task<ManagerDataDto?> GetManagerDetailAsync(int managerId);

        /// <summary>
        /// 建立管理員
        /// 管理者權限管理限定功能
        /// </summary>
        /// <param name="createDto">建立管理員請求</param>
        /// <returns>操作結果和管理員資訊</returns>
        Task<AdminServiceResult<ManagerDataDto>> CreateManagerAsync(CreateManagerDto createDto);

        /// <summary>
        /// 更新管理員
        /// 管理者權限管理限定功能
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <param name="updateDto">更新管理員請求</param>
        /// <returns>操作結果和更新後管理員資訊</returns>
        Task<AdminServiceResult<ManagerDataDto>> UpdateManagerAsync(int managerId, UpdateManagerDto updateDto);

        /// <summary>
        /// 更新管理員登入追蹤
        /// 記錄到Admins.last_login
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> UpdateLastLoginAsync(int managerId);

        #endregion

        #region 角色權限管理

        /// <summary>
        /// 取得角色權限列表
        /// 返回所有可指派的職能開關
        /// </summary>
        /// <returns>角色權限列表</returns>
        Task<List<ManagerRolePermissionDto>> GetRolePermissionsAsync();

        /// <summary>
        /// 取得角色權限詳細資訊
        /// 包含指派的管理員統計
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色權限詳細資訊</returns>
        Task<ManagerRolePermissionDto?> GetRolePermissionDetailAsync(int roleId);

        /// <summary>
        /// 建立角色權限
        /// 管理者權限管理限定功能
        /// </summary>
        /// <param name="createDto">建立角色權限請求</param>
        /// <returns>操作結果和角色權限資訊</returns>
        Task<AdminServiceResult<ManagerRolePermissionDto>> CreateRolePermissionAsync(CreateManagerRolePermissionDto createDto);

        /// <summary>
        /// 更新角色權限
        /// 管理者權限管理限定功能
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="updateDto">更新角色權限請求</param>
        /// <returns>操作結果和更新後角色權限資訊</returns>
        Task<AdminServiceResult<ManagerRolePermissionDto>> UpdateRolePermissionAsync(int roleId, CreateManagerRolePermissionDto updateDto);

        /// <summary>
        /// 指派角色給管理員
        /// 管理ManagerRole多對多關係
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <param name="roleIds">角色ID列表</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> AssignRolesToManagerAsync(int managerId, List<int> roleIds);

        /// <summary>
        /// 取得管理員的角色權限摘要
        /// 合併所有指派角色的權限
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <returns>權限摘要</returns>
        Task<ManagerPermissionSummaryDto> GetManagerPermissionsAsync(int managerId);

        #endregion

        #region 使用者治理

        /// <summary>
        /// 取得使用者管理列表
        /// 聯合Users、User_Introduce、User_Rights、User_wallet等表
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="searchKeyword">搜尋關鍵字</param>
        /// <param name="accountStatus">帳號狀態篩選</param>
        /// <returns>分頁使用者管理列表</returns>
        Task<AdminPagedResult<UserManagementDto>> GetUsersForManagementAsync(
            int page = 1, int pageSize = 20, string? searchKeyword = null, int? accountStatus = null);

        /// <summary>
        /// 取得使用者詳細檔案
        /// 包含個資、權限、錢包、銷售資訊
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>使用者詳細檔案</returns>
        Task<UserManagementDto?> GetUserDetailForManagementAsync(int userId);

        /// <summary>
        /// 調整使用者權限
        /// 寫入User_Rights，記錄操作日誌
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="updateDto">權限調整請求</param>
        /// <param name="operatorId">操作者ID</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> UpdateUserRightsAsync(int userId, UpdateUserRightsDto updateDto, int operatorId);

        /// <summary>
        /// 調整使用者點數
        /// 更新User_wallet.User_Point，發送通知，記錄審計日誌
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="adjustDto">點數調整請求</param>
        /// <param name="operatorId">操作者ID</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> AdjustUserPointsAsync(int userId, AdjustUserPointsDto adjustDto, int operatorId);

        /// <summary>
        /// 調整使用者銷售錢包
        /// 更新User_Sales_Information.UserSales_Wallet，記錄審計日誌
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="adjustDto">銷售錢包調整請求</param>
        /// <param name="operatorId">操作者ID</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> AdjustUserSalesWalletAsync(int userId, AdjustUserPointsDto adjustDto, int operatorId);

        #endregion

        #region 內容管理

        /// <summary>
        /// 取得論壇內容管理列表
        /// 管理threads、thread_posts的狀態
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="contentType">內容類型 (thread/post)</param>
        /// <param name="status">狀態篩選</param>
        /// <returns>分頁內容管理列表</returns>
        Task<AdminPagedResult<object>> GetForumContentForModerationAsync(
            int page = 1, int pageSize = 20, string? contentType = null, string? status = null);

        /// <summary>
        /// 更新論壇內容狀態
        /// 設定threads.status、thread_posts.status
        /// </summary>
        /// <param name="contentType">內容類型</param>
        /// <param name="contentId">內容ID</param>
        /// <param name="newStatus">新狀態</param>
        /// <param name="operatorId">操作者ID</param>
        /// <param name="reason">操作原因</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> UpdateForumContentStatusAsync(
            string contentType, long contentId, string newStatus, int operatorId, string? reason = null);

        /// <summary>
        /// 取得洞察貼文管理列表
        /// 管理posts的狀態和置頂
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="status">狀態篩選</param>
        /// <returns>分頁洞察貼文列表</returns>
        Task<AdminPagedResult<object>> GetInsightPostsForModerationAsync(
            int page = 1, int pageSize = 20, string? status = null);

        /// <summary>
        /// 設定洞察貼文置頂
        /// 更新posts.pinned
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <param name="isPinned">是否置頂</param>
        /// <param name="operatorId">操作者ID</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> SetInsightPostPinnedAsync(int postId, bool isPinned, int operatorId);

        #endregion

        #region 商務管理

        /// <summary>
        /// 取得官方商城訂單管理列表
        /// 查詢OrderInfo、OrderItems
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="orderStatus">訂單狀態篩選</param>
        /// <param name="paymentStatus">付款狀態篩選</param>
        /// <returns>分頁訂單管理列表</returns>
        Task<AdminPagedResult<object>> GetOrdersForManagementAsync(
            int page = 1, int pageSize = 20, string? orderStatus = null, string? paymentStatus = null);

        /// <summary>
        /// 更新訂單狀態
        /// 支援退款、出貨、完成等操作
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <param name="newStatus">新狀態</param>
        /// <param name="operatorId">操作者ID</param>
        /// <param name="note">操作備註</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> UpdateOrderStatusAsync(int orderId, string newStatus, int operatorId, string? note = null);

        /// <summary>
        /// 取得玩家市場商品管理列表
        /// 管理PMP的p_status
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="status">商品狀態篩選</param>
        /// <returns>分頁市場商品列表</returns>
        Task<AdminPagedResult<object>> GetPlayerMarketProductsForModerationAsync(
            int page = 1, int pageSize = 20, string? status = null);

        /// <summary>
        /// 審核玩家市場商品
        /// 調整PMP.p_status
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="newStatus">新狀態</param>
        /// <param name="operatorId">操作者ID</param>
        /// <param name="reason">審核原因</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> ReviewPlayerMarketProductAsync(
            int productId, string newStatus, int operatorId, string? reason = null);

        #endregion

        #region 系統設定管理

        /// <summary>
        /// 取得系統設定
        /// 應用層設定管理
        /// </summary>
        /// <param name="category">設定分類</param>
        /// <returns>設定分組列表</returns>
        Task<List<SystemConfigGroupDto>> GetSystemConfigsAsync(string? category = null);

        /// <summary>
        /// 更新系統設定
        /// 管理者權限管理限定功能
        /// </summary>
        /// <param name="category">設定分類</param>
        /// <param name="key">設定鍵</param>
        /// <param name="updateDto">更新設定請求</param>
        /// <param name="operatorId">操作者ID</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> UpdateSystemConfigAsync(
            string category, string key, UpdateSystemConfigDto updateDto, int operatorId);

        /// <summary>
        /// 取得禁言項目列表
        /// 管理Mutes表
        /// </summary>
        /// <param name="activeOnly">是否只顯示啟用的項目</param>
        /// <returns>禁言項目列表</returns>
        Task<List<MuteDto>> GetMutesAsync(bool activeOnly = true);

        /// <summary>
        /// 建立禁言項目
        /// 論壇權限管理限定功能
        /// </summary>
        /// <param name="createDto">建立禁言項目請求</param>
        /// <param name="operatorId">操作者ID</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult<MuteDto>> CreateMuteAsync(CreateMuteDto createDto, int operatorId);

        /// <summary>
        /// 取得樣式項目列表
        /// 管理Styles表
        /// </summary>
        /// <returns>樣式項目列表</returns>
        Task<List<StyleDto>> GetStylesAsync();

        /// <summary>
        /// 建立樣式項目
        /// 論壇權限管理限定功能
        /// </summary>
        /// <param name="createDto">建立樣式項目請求</param>
        /// <param name="operatorId">操作者ID</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult<StyleDto>> CreateStyleAsync(CreateStyleDto createDto, int operatorId);

        #endregion

        #region 統計報表

        /// <summary>
        /// 取得管理員儀表板統計
        /// 計算綜合統計指標用於儀表板展示
        /// </summary>
        /// <returns>儀表板統計資訊</returns>
        Task<AdminDashboardDto> GetAdminDashboardAsync();

        /// <summary>
        /// 取得使用者統計報表
        /// 分析使用者註冊、活躍度、留存等指標
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>使用者統計報表</returns>
        Task<Dictionary<string, object>> GetUserStatisticsAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 取得營收統計報表
        /// 分析商城和市場營收趨勢
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>營收統計報表</returns>
        Task<Dictionary<string, object>> GetRevenueStatisticsAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 取得系統健康狀態
        /// 監控系統運行狀況
        /// </summary>
        /// <returns>系統健康狀態</returns>
        Task<SystemHealthDto> GetSystemHealthAsync();

        #endregion

        #region 操作日誌

        /// <summary>
        /// 記錄操作日誌
        /// 稽核管理員操作行為
        /// </summary>
        /// <param name="operationType">操作類型</param>
        /// <param name="module">操作模組</param>
        /// <param name="operatorId">操作者ID</param>
        /// <param name="targetResource">目標資源</param>
        /// <param name="targetId">目標ID</param>
        /// <param name="description">操作描述</param>
        /// <param name="details">操作詳情</param>
        /// <param name="result">操作結果</param>
        /// <returns>操作結果</returns>
        Task<AdminServiceResult> LogOperationAsync(
            string operationType, string module, int operatorId, string? targetResource = null,
            string? targetId = null, string? description = null, object? details = null, string? result = null);

        /// <summary>
        /// 查詢操作日誌
        /// 支援多維度篩選和分頁
        /// </summary>
        /// <param name="queryDto">查詢條件</param>
        /// <returns>分頁操作日誌</returns>
        Task<AdminPagedResult<OperationLogDto>> GetOperationLogsAsync(OperationLogQueryDto queryDto);

        #endregion

        #region 權限檢查

        /// <summary>
        /// 檢查管理員權限
        /// 驗證管理員是否具備指定操作權限
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <param name="requiredPermission">所需權限</param>
        /// <returns>是否具備權限</returns>
        Task<bool> CheckManagerPermissionAsync(int managerId, string requiredPermission);

        /// <summary>
        /// 檢查多項權限
        /// 驗證管理員是否具備所有指定權限
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <param name="requiredPermissions">所需權限列表</param>
        /// <returns>權限檢查結果</returns>
        Task<Dictionary<string, bool>> CheckMultiplePermissionsAsync(int managerId, List<string> requiredPermissions);

        #endregion

        #region 批量操作

        /// <summary>
        /// 批量更新使用者權限
        /// 支援批量停權、解封等操作
        /// </summary>
        /// <param name="userIds">使用者ID列表</param>
        /// <param name="updateDto">權限調整請求</param>
        /// <param name="operatorId">操作者ID</param>
        /// <returns>批量操作結果</returns>
        Task<AdminServiceResult> BatchUpdateUserRightsAsync(
            List<int> userIds, UpdateUserRightsDto updateDto, int operatorId);

        /// <summary>
        /// 批量處理審核項目
        /// 支援批量通過、拒絕等操作
        /// </summary>
        /// <param name="auditIds">審核ID列表</param>
        /// <param name="decision">審核決定</param>
        /// <param name="operatorId">操作者ID</param>
        /// <param name="batchComment">批量操作備註</param>
        /// <returns>批量操作結果</returns>
        Task<AdminServiceResult> BatchProcessAuditsAsync(
            List<string> auditIds, string decision, int operatorId, string? batchComment = null);

        #endregion
    }
}