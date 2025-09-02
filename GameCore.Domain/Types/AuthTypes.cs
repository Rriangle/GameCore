using GameCore.Domain.Entities;

namespace GameCore.Core.Types
{
    /// <summary>
    /// 認證結果
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// JWT Token
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 刷新 Token
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// 用戶資訊
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// 建立成功的認證結果
        /// </summary>
        public static AuthResult Success(int userId, string token, string refreshToken, User user)
        {
            return new AuthResult
            {
                IsSuccess = true,
                UserId = userId,
                Token = token,
                RefreshToken = refreshToken,
                User = user
            };
        }

        /// <summary>
        /// 建立失敗的認證結果
        /// </summary>
        public static AuthResult Failure(string errorMessage)
        {
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
} 
