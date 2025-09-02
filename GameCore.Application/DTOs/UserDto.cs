namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 用戶DTO
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// 暱稱
        /// </summary>
        public string? NickName { get; set; }
        
        /// <summary>
        /// 頭像URL
        /// </summary>
        public string? Avatar { get; set; }
        
        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LastLoginAt { get; set; }
        
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// 建立用戶DTO
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// 暱稱
        /// </summary>
        public string? NickName { get; set; }
    }
    
    /// <summary>
    /// 更新用戶DTO
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// 暱稱
        /// </summary>
        public string? NickName { get; set; }
        
        /// <summary>
        /// 頭像URL
        /// </summary>
        public string? Avatar { get; set; }
    }
    
    /// <summary>
    /// 登入DTO
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// 註冊DTO
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// 暱稱
        /// </summary>
        public string? NickName { get; set; }
    }
    
    /// <summary>
    /// 登入結果
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 用戶資料
        /// </summary>
        public UserDto User { get; set; } = new();
        
        /// <summary>
        /// 存取Token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
        
        /// <summary>
        /// 重新整理Token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
        
        /// <summary>
        /// Token過期時間
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
    
    /// <summary>
    /// 註冊結果
    /// </summary>
    public class RegisterResult
    {
        /// <summary>
        /// 用戶資料
        /// </summary>
        public UserDto User { get; set; } = new();
        
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
} 