using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 用戶回應
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();
    }

    /// <summary>
    /// 建立用戶請求
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        [Required(ErrorMessage = "用戶名為必填")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "用戶名長度必須在 3-100 字元之間")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "用戶名只能包含字母、數字和底線")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required(ErrorMessage = "電子郵件為必填")]
        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "密碼為必填")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度必須至少 6 字元")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "密碼必須包含大小寫字母和數字")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();
    }

    /// <summary>
    /// 更新用戶請求
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        [StringLength(100, MinimumLength = 3, ErrorMessage = "用戶名長度必須在 3-100 字元之間")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "用戶名只能包含字母、數字和底線")]
        public string? UserName { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        public string? Email { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string>? Roles { get; set; }
    }

    /// <summary>
    /// 用戶查詢參數
    /// </summary>
    public class UserQueryParameters
    {
        /// <summary>
        /// 搜尋關鍵字
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// 建立時間開始
        /// </summary>
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        /// 建立時間結束
        /// </summary>
        public DateTime? CreatedBefore { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 排序欄位
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// 是否降序排序
        /// </summary>
        public bool SortDescending { get; set; } = false;
    }

    /// <summary>
    /// 用戶統計資訊
    /// </summary>
    public class UserStatsResponse
    {
        /// <summary>
        /// 總用戶數
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// 啟用用戶數
        /// </summary>
        public int ActiveUsers { get; set; }

        /// <summary>
        /// 停用用戶數
        /// </summary>
        public int InactiveUsers { get; set; }

        /// <summary>
        /// 今日新增用戶數
        /// </summary>
        public int NewUsersToday { get; set; }

        /// <summary>
        /// 本週新增用戶數
        /// </summary>
        public int NewUsersThisWeek { get; set; }

        /// <summary>
        /// 本月新增用戶數
        /// </summary>
        public int NewUsersThisMonth { get; set; }

        /// <summary>
        /// 角色統計
        /// </summary>
        public Dictionary<string, int> RoleStats { get; set; } = new Dictionary<string, int>();
    }
} 