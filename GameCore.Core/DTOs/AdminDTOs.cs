using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 管理員資料 DTOs

    /// <summary>
    /// 管理員資料 DTO
    /// </summary>
    public class ManagerDataDto
    {
        /// <summary>管理員ID</summary>
        public int ManagerId { get; set; }

        /// <summary>管理員姓名</summary>
        public string? ManagerName { get; set; }

        /// <summary>管理員帳號</summary>
        public string? ManagerAccount { get; set; }

        /// <summary>註冊時間</summary>
        public DateTime AdministratorRegistrationDate { get; set; }

        /// <summary>最後登入時間</summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; }

        /// <summary>指派的角色列表</summary>
        public List<ManagerRolePermissionDto> AssignedRoles { get; set; } = new();

        /// <summary>權限摘要</summary>
        public ManagerPermissionSummaryDto PermissionSummary { get; set; } = new();
    }

    /// <summary>
    /// 建立管理員請求 DTO
    /// </summary>
    public class CreateManagerDto
    {
        /// <summary>管理員姓名</summary>
        [Required(ErrorMessage = "管理員姓名為必填")]
        [StringLength(100, ErrorMessage = "管理員姓名長度不能超過100字元")]
        public string ManagerName { get; set; } = string.Empty;

        /// <summary>管理員帳號</summary>
        [Required(ErrorMessage = "管理員帳號為必填")]
        [StringLength(50, ErrorMessage = "管理員帳號長度不能超過50字元")]
        public string ManagerAccount { get; set; } = string.Empty;

        /// <summary>管理員密碼</summary>
        [Required(ErrorMessage = "管理員密碼為必填")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "密碼長度必須在8到100字元之間")]
        public string ManagerPassword { get; set; } = string.Empty;

        /// <summary>指派的角色ID列表</summary>
        public List<int> RoleIds { get; set; } = new();
    }

    /// <summary>
    /// 更新管理員請求 DTO
    /// </summary>
    public class UpdateManagerDto
    {
        /// <summary>管理員姓名</summary>
        [StringLength(100, ErrorMessage = "管理員姓名長度不能超過100字元")]
        public string? ManagerName { get; set; }

        /// <summary>是否啟用</summary>
        public bool? IsActive { get; set; }

        /// <summary>指派的角色ID列表</summary>
        public List<int>? RoleIds { get; set; }
    }

    /// <summary>
    /// 管理員登入請求 DTO
    /// </summary>
    public class ManagerLoginDto
    {
        /// <summary>管理員帳號</summary>
        [Required(ErrorMessage = "管理員帳號為必填")]
        public string ManagerAccount { get; set; } = string.Empty;

        /// <summary>管理員密碼</summary>
        [Required(ErrorMessage = "管理員密碼為必填")]
        public string ManagerPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 管理員登入回應 DTO
    /// </summary>
    public class ManagerLoginResponseDto
    {
        /// <summary>登入Token</summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>管理員資訊</summary>
        public ManagerDataDto Manager { get; set; } = new();

        /// <summary>權限清單</summary>
        public ManagerPermissionSummaryDto Permissions { get; set; } = new();

        /// <summary>Token過期時間</summary>
        public DateTime TokenExpiry { get; set; }
    }

    #endregion

    #region 角色權限管理 DTOs

    /// <summary>
    /// 管理員角色權限 DTO
    /// </summary>
    public class ManagerRolePermissionDto
    {
        /// <summary>角色ID</summary>
        public int ManagerRoleId { get; set; }

        /// <summary>角色名稱</summary>
        public string? RoleName { get; set; }

        /// <summary>管理者權限管理</summary>
        public bool AdministratorPrivilegesManagement { get; set; }

        /// <summary>使用者狀態管理</summary>
        public bool UserStatusManagement { get; set; }

        /// <summary>商城權限管理</summary>
        public bool ShoppingPermissionManagement { get; set; }

        /// <summary>論壇權限管理</summary>
        public bool MessagePermissionManagement { get; set; }

        /// <summary>銷售權限管理</summary>
        public bool SalesPermissionManagement { get; set; }

        /// <summary>客服權限管理</summary>
        public bool CustomerService { get; set; }

        /// <summary>指派的管理員數量</summary>
        public int AssignedManagersCount { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 建立角色權限請求 DTO
    /// </summary>
    public class CreateManagerRolePermissionDto
    {
        /// <summary>角色名稱</summary>
        [Required(ErrorMessage = "角色名稱為必填")]
        [StringLength(100, ErrorMessage = "角色名稱長度不能超過100字元")]
        public string RoleName { get; set; } = string.Empty;

        /// <summary>管理者權限管理</summary>
        public bool AdministratorPrivilegesManagement { get; set; }

        /// <summary>使用者狀態管理</summary>
        public bool UserStatusManagement { get; set; }

        /// <summary>商城權限管理</summary>
        public bool ShoppingPermissionManagement { get; set; }

        /// <summary>論壇權限管理</summary>
        public bool MessagePermissionManagement { get; set; }

        /// <summary>銷售權限管理</summary>
        public bool SalesPermissionManagement { get; set; }

        /// <summary>客服權限管理</summary>
        public bool CustomerService { get; set; }
    }

    /// <summary>
    /// 權限摘要 DTO
    /// </summary>
    public class ManagerPermissionSummaryDto
    {
        /// <summary>管理者權限管理</summary>
        public bool AdministratorPrivilegesManagement { get; set; }

        /// <summary>使用者狀態管理</summary>
        public bool UserStatusManagement { get; set; }

        /// <summary>商城權限管理</summary>
        public bool ShoppingPermissionManagement { get; set; }

        /// <summary>論壇權限管理</summary>
        public bool MessagePermissionManagement { get; set; }

        /// <summary>銷售權限管理</summary>
        public bool SalesPermissionManagement { get; set; }

        /// <summary>客服權限管理</summary>
        public bool CustomerService { get; set; }

        /// <summary>所有權限清單</summary>
        public List<string> AllPermissions
        {
            get
            {
                var permissions = new List<string>();
                if (AdministratorPrivilegesManagement) permissions.Add("管理者權限管理");
                if (UserStatusManagement) permissions.Add("使用者狀態管理");
                if (ShoppingPermissionManagement) permissions.Add("商城權限管理");
                if (MessagePermissionManagement) permissions.Add("論壇權限管理");
                if (SalesPermissionManagement) permissions.Add("銷售權限管理");
                if (CustomerService) permissions.Add("客服權限管理");
                return permissions;
            }
        }
    }

    #endregion

    #region 使用者治理 DTOs

    /// <summary>
    /// 使用者管理概覽 DTO
    /// </summary>
    public class UserManagementDto
    {
        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>使用者帳號</summary>
        public string? UserAccount { get; set; }

        /// <summary>使用者名稱</summary>
        public string? UserName { get; set; }

        /// <summary>暱稱</summary>
        public string? UserNickname { get; set; }

        /// <summary>電子郵件</summary>
        public string? Email { get; set; }

        /// <summary>註冊時間</summary>
        public DateTime? UserRegistrationTime { get; set; }

        /// <summary>最後登入時間</summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>使用者權限</summary>
        public UserRightsDto UserRights { get; set; } = new();

        /// <summary>錢包資訊</summary>
        public UserWalletSummaryDto WalletSummary { get; set; } = new();

        /// <summary>銷售資訊</summary>
        public UserSalesSummaryDto SalesSummary { get; set; } = new();

        /// <summary>帳號狀態</summary>
        public string AccountStatus { get; set; } = "normal";
    }

    /// <summary>
    /// 使用者權限 DTO
    /// </summary>
    public class UserRightsDto
    {
        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>帳號狀態 (0-停權 1-正常)</summary>
        public int? AccountStatus { get; set; }

        /// <summary>留言權限 (0-禁止 1-允許)</summary>
        public int? CommentPermission { get; set; }

        /// <summary>購物權限 (0-禁止 1-允許)</summary>
        public int? ShoppingPermission { get; set; }

        /// <summary>銷售權限 (0-禁止 1-允許)</summary>
        public int? SalesAuthority { get; set; }

        /// <summary>權限狀態描述</summary>
        public string StatusDescription
        {
            get
            {
                var statuses = new List<string>();
                if (AccountStatus == 0) statuses.Add("停權");
                if (CommentPermission == 0) statuses.Add("禁言");
                if (ShoppingPermission == 0) statuses.Add("禁購");
                if (SalesAuthority == 0) statuses.Add("禁售");
                return statuses.Any() ? string.Join("、", statuses) : "正常";
            }
        }
    }

    /// <summary>
    /// 錢包摘要 DTO
    /// </summary>
    public class UserWalletSummaryDto
    {
        /// <summary>使用者點數</summary>
        public int UserPoint { get; set; }

        /// <summary>銷售錢包餘額</summary>
        public int? SalesWallet { get; set; }

        /// <summary>最後更新時間</summary>
        public DateTime? LastUpdated { get; set; }
    }

    /// <summary>
    /// 銷售摘要 DTO
    /// </summary>
    public class UserSalesSummaryDto
    {
        /// <summary>是否為銷售會員</summary>
        public bool IsSalesMember { get; set; }

        /// <summary>銷售權限</summary>
        public bool HasSalesAuthority { get; set; }

        /// <summary>銷售會員申請時間</summary>
        public DateTime? SalesApplicationTime { get; set; }

        /// <summary>總銷售額</summary>
        public decimal TotalSalesAmount { get; set; }

        /// <summary>銷售商品數</summary>
        public int TotalProductsSold { get; set; }
    }

    /// <summary>
    /// 使用者權限調整請求 DTO
    /// </summary>
    public class UpdateUserRightsDto
    {
        /// <summary>帳號狀態</summary>
        public int? AccountStatus { get; set; }

        /// <summary>留言權限</summary>
        public int? CommentPermission { get; set; }

        /// <summary>購物權限</summary>
        public int? ShoppingPermission { get; set; }

        /// <summary>銷售權限</summary>
        public int? SalesAuthority { get; set; }

        /// <summary>調整原因</summary>
        [Required(ErrorMessage = "調整原因為必填")]
        [StringLength(500, ErrorMessage = "調整原因長度不能超過500字元")]
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// 使用者點數調整請求 DTO
    /// </summary>
    public class AdjustUserPointsDto
    {
        /// <summary>調整金額（正數為增加，負數為扣除）</summary>
        [Required(ErrorMessage = "調整金額為必填")]
        public int Delta { get; set; }

        /// <summary>調整原因</summary>
        [Required(ErrorMessage = "調整原因為必填")]
        [StringLength(500, ErrorMessage = "調整原因長度不能超過500字元")]
        public string Reason { get; set; } = string.Empty;
    }

    #endregion

    #region 系統設定管理 DTOs

    /// <summary>
    /// 系統設定 DTO
    /// </summary>
    public class SystemConfigDto
    {
        /// <summary>設定分類</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>設定鍵</summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>設定值</summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>設定描述</summary>
        public string? Description { get; set; }

        /// <summary>資料類型</summary>
        public string DataType { get; set; } = "string";

        /// <summary>是否為敏感資料</summary>
        public bool IsSensitive { get; set; }

        /// <summary>最後更新時間</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>更新者</summary>
        public string? UpdatedBy { get; set; }
    }

    /// <summary>
    /// 系統設定分組 DTO
    /// </summary>
    public class SystemConfigGroupDto
    {
        /// <summary>分類名稱</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>分類描述</summary>
        public string? CategoryDescription { get; set; }

        /// <summary>設定項目</summary>
        public List<SystemConfigDto> Configs { get; set; } = new();
    }

    /// <summary>
    /// 更新系統設定請求 DTO
    /// </summary>
    public class UpdateSystemConfigDto
    {
        /// <summary>設定值</summary>
        [Required(ErrorMessage = "設定值為必填")]
        public string Value { get; set; } = string.Empty;

        /// <summary>更新原因</summary>
        public string? UpdateReason { get; set; }
    }

    #endregion

    #region 禁言和樣式管理 DTOs

    /// <summary>
    /// 禁言項目 DTO
    /// </summary>
    public class MuteDto
    {
        /// <summary>禁言ID</summary>
        public int MuteId { get; set; }

        /// <summary>禁言名稱</summary>
        public string? MuteName { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; }

        /// <summary>設置者ID</summary>
        public int ManagerId { get; set; }

        /// <summary>設置者名稱</summary>
        public string? ManagerName { get; set; }
    }

    /// <summary>
    /// 建立禁言項目請求 DTO
    /// </summary>
    public class CreateMuteDto
    {
        /// <summary>禁言名稱</summary>
        [Required(ErrorMessage = "禁言名稱為必填")]
        [StringLength(100, ErrorMessage = "禁言名稱長度不能超過100字元")]
        public string MuteName { get; set; } = string.Empty;

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// 樣式項目 DTO
    /// </summary>
    public class StyleDto
    {
        /// <summary>樣式ID</summary>
        public int StyleId { get; set; }

        /// <summary>樣式名稱</summary>
        public string? StyleName { get; set; }

        /// <summary>效果說明</summary>
        public string? EffectDesc { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>設置者ID</summary>
        public int ManagerId { get; set; }

        /// <summary>設置者名稱</summary>
        public string? ManagerName { get; set; }
    }

    /// <summary>
    /// 建立樣式項目請求 DTO
    /// </summary>
    public class CreateStyleDto
    {
        /// <summary>樣式名稱</summary>
        [Required(ErrorMessage = "樣式名稱為必填")]
        [StringLength(100, ErrorMessage = "樣式名稱長度不能超過100字元")]
        public string StyleName { get; set; } = string.Empty;

        /// <summary>效果說明</summary>
        [StringLength(500, ErrorMessage = "效果說明長度不能超過500字元")]
        public string? EffectDesc { get; set; }
    }

    #endregion

    #region 審核管理 DTOs

    /// <summary>
    /// 審核項目 DTO
    /// </summary>
    public class AuditItemDto
    {
        /// <summary>審核ID</summary>
        public string AuditId { get; set; } = string.Empty;

        /// <summary>審核類型</summary>
        public string AuditType { get; set; } = string.Empty;

        /// <summary>目標ID</summary>
        public string TargetId { get; set; } = string.Empty;

        /// <summary>目標類型</summary>
        public string TargetType { get; set; } = string.Empty;

        /// <summary>審核狀態</summary>
        public string Status { get; set; } = "pending";

        /// <summary>申請者ID</summary>
        public int? ApplicantId { get; set; }

        /// <summary>申請者名稱</summary>
        public string? ApplicantName { get; set; }

        /// <summary>申請時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>審核者ID</summary>
        public int? ReviewerId { get; set; }

        /// <summary>審核者名稱</summary>
        public string? ReviewerName { get; set; }

        /// <summary>審核時間</summary>
        public DateTime? ReviewedAt { get; set; }

        /// <summary>審核意見</summary>
        public string? ReviewComment { get; set; }

        /// <summary>審核內容</summary>
        public Dictionary<string, object>? Content { get; set; }
    }

    /// <summary>
    /// 審核處理請求 DTO
    /// </summary>
    public class ProcessAuditDto
    {
        /// <summary>審核結果 (approved/rejected)</summary>
        [Required(ErrorMessage = "審核結果為必填")]
        public string Decision { get; set; } = string.Empty;

        /// <summary>審核意見</summary>
        [StringLength(1000, ErrorMessage = "審核意見長度不能超過1000字元")]
        public string? ReviewComment { get; set; }
    }

    #endregion

    #region 統計報表 DTOs

    /// <summary>
    /// 管理員儀表板統計 DTO
    /// </summary>
    public class AdminDashboardDto
    {
        /// <summary>總使用者數</summary>
        public int TotalUsers { get; set; }

        /// <summary>今日新增使用者</summary>
        public int TodayNewUsers { get; set; }

        /// <summary>活躍使用者數 (本月)</summary>
        public int ActiveUsers { get; set; }

        /// <summary>總訂單數</summary>
        public int TotalOrders { get; set; }

        /// <summary>今日訂單數</summary>
        public int TodayOrders { get; set; }

        /// <summary>待審核項目數</summary>
        public int PendingAudits { get; set; }

        /// <summary>系統警告數</summary>
        public int SystemAlerts { get; set; }

        /// <summary>總營收</summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>本月營收</summary>
        public decimal MonthlyRevenue { get; set; }

        /// <summary>熱門遊戲排行</summary>
        public List<GameRankingDto> TopGames { get; set; } = new();

        /// <summary>近期使用者註冊趨勢</summary>
        public List<TrendDataDto> UserRegistrationTrend { get; set; } = new();

        /// <summary>近期訂單趨勢</summary>
        public List<TrendDataDto> OrderTrend { get; set; } = new();

        /// <summary>系統健康狀態</summary>
        public SystemHealthDto SystemHealth { get; set; } = new();
    }

    /// <summary>
    /// 遊戲排名 DTO
    /// </summary>
    public class GameRankingDto
    {
        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>排名</summary>
        public int Rank { get; set; }

        /// <summary>分數</summary>
        public decimal Score { get; set; }

        /// <summary>變化趨勢</summary>
        public string Trend { get; set; } = "stable";
    }

    /// <summary>
    /// 趨勢數據 DTO
    /// </summary>
    public class TrendDataDto
    {
        /// <summary>日期</summary>
        public DateTime Date { get; set; }

        /// <summary>數值</summary>
        public decimal Value { get; set; }

        /// <summary>標籤</summary>
        public string? Label { get; set; }
    }

    /// <summary>
    /// 系統健康狀態 DTO
    /// </summary>
    public class SystemHealthDto
    {
        /// <summary>整體健康度 (0-100)</summary>
        public int OverallHealth { get; set; }

        /// <summary>CPU使用率</summary>
        public double CpuUsage { get; set; }

        /// <summary>記憶體使用率</summary>
        public double MemoryUsage { get; set; }

        /// <summary>磁碟使用率</summary>
        public double DiskUsage { get; set; }

        /// <summary>資料庫連線狀態</summary>
        public bool DatabaseConnected { get; set; }

        /// <summary>外部服務狀態</summary>
        public Dictionary<string, bool> ExternalServices { get; set; } = new();

        /// <summary>最後檢查時間</summary>
        public DateTime LastCheckTime { get; set; }
    }

    #endregion

    #region 操作日誌 DTOs

    /// <summary>
    /// 操作日誌 DTO
    /// </summary>
    public class OperationLogDto
    {
        /// <summary>日誌ID</summary>
        public long LogId { get; set; }

        /// <summary>操作類型</summary>
        public string? OperationType { get; set; }

        /// <summary>操作模組</summary>
        public string? Module { get; set; }

        /// <summary>操作者ID</summary>
        public int? OperatorId { get; set; }

        /// <summary>操作者名稱</summary>
        public string? OperatorName { get; set; }

        /// <summary>操作者類型 (user/manager)</summary>
        public string? OperatorType { get; set; }

        /// <summary>目標資源</summary>
        public string? TargetResource { get; set; }

        /// <summary>目標ID</summary>
        public string? TargetId { get; set; }

        /// <summary>操作描述</summary>
        public string? Description { get; set; }

        /// <summary>操作詳情 (JSON)</summary>
        public string? Details { get; set; }

        /// <summary>操作結果</summary>
        public string? Result { get; set; }

        /// <summary>IP位址</summary>
        public string? IpAddress { get; set; }

        /// <summary>用戶代理</summary>
        public string? UserAgent { get; set; }

        /// <summary>操作時間</summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 操作日誌查詢請求 DTO
    /// </summary>
    public class OperationLogQueryDto
    {
        /// <summary>開始時間</summary>
        public DateTime? StartTime { get; set; }

        /// <summary>結束時間</summary>
        public DateTime? EndTime { get; set; }

        /// <summary>操作類型</summary>
        public string? OperationType { get; set; }

        /// <summary>操作模組</summary>
        public string? Module { get; set; }

        /// <summary>操作者ID</summary>
        public int? OperatorId { get; set; }

        /// <summary>操作者類型</summary>
        public string? OperatorType { get; set; }

        /// <summary>目標資源</summary>
        public string? TargetResource { get; set; }

        /// <summary>操作結果</summary>
        public string? Result { get; set; }

        /// <summary>頁碼</summary>
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數</summary>
        public int PageSize { get; set; } = 50;
    }

    #endregion

    #region 分頁和服務結果 DTOs

    /// <summary>
    /// 管理員分頁結果 DTO
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class AdminPagedResult<T>
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

        /// <summary>附加資訊</summary>
        public Dictionary<string, object>? Meta { get; set; }
    }

    /// <summary>
    /// 管理員服務執行結果
    /// </summary>
    public class AdminServiceResult
    {
        /// <summary>是否成功</summary>
        public bool Success { get; set; }

        /// <summary>訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>錯誤清單</summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>附加資料</summary>
        public Dictionary<string, object>? Data { get; set; }

        /// <summary>建立成功結果</summary>
        public static AdminServiceResult CreateSuccess(string message = "操作成功", Dictionary<string, object>? data = null)
        {
            return new AdminServiceResult { Success = true, Message = message, Data = data };
        }

        /// <summary>建立失敗結果</summary>
        public static AdminServiceResult CreateFailure(string message, List<string>? errors = null)
        {
            return new AdminServiceResult { Success = false, Message = message, Errors = errors ?? new() };
        }
    }

    /// <summary>
    /// 帶資料的管理員服務執行結果
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class AdminServiceResult<T> : AdminServiceResult
    {
        /// <summary>結果資料</summary>
        public T? Result { get; set; }

        /// <summary>建立成功結果</summary>
        public static AdminServiceResult<T> CreateSuccess(T result, string message = "操作成功")
        {
            return new AdminServiceResult<T> { Success = true, Message = message, Result = result };
        }

        /// <summary>建立失敗結果</summary>
        public static new AdminServiceResult<T> CreateFailure(string message, List<string>? errors = null)
        {
            return new AdminServiceResult<T> { Success = false, Message = message, Errors = errors ?? new() };
        }
    }

    #endregion
}