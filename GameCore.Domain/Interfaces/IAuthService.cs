using GameCore.Core.DTOs;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 認證服務介面
    /// 提供用戶註冊、登入、OAuth等功能
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 用戶註冊
        /// </summary>
        /// <param name="request">註冊請求</param>
        /// <returns>註冊結果</returns>
        Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);

        /// <summary>
        /// 用戶登入
        /// </summary>
        /// <param name="request">登入請求</param>
        /// <returns>登入結果</returns>
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        /// <summary>
        /// OAuth登入
        /// </summary>
        /// <param name="request">OAuth登入請求</param>
        /// <returns>登入結果</returns>
        Task<LoginResponseDto> OAuthLoginAsync(OAuthLoginRequestDto request);

        /// <summary>
        /// 取得當前用戶資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>用戶資訊</returns>
        Task<UserInfoDto?> GetUserInfoAsync(int userId);

        /// <summary>
        /// 更新個人資料
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="request">更新請求</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateProfileAsync(int userId, UpdateProfileRequestDto request);

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="request">變更密碼請求</param>
        /// <returns>變更結果</returns>
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequestDto request);

        /// <summary>
        /// 驗證JWT Token
        /// </summary>
        /// <param name="token">JWT Token</param>
        /// <returns>驗證結果</returns>
        Task<bool> ValidateTokenAsync(string token);

        /// <summary>
        /// 重新整理Token
        /// </summary>
        /// <param name="refreshToken">重新整理Token</param>
        /// <returns>新的Token</returns>
        Task<string?> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>登出結果</returns>
        Task<bool> LogoutAsync(int userId);

        /// <summary>
        /// 檢查帳號是否存在
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns>是否存在</returns>
        Task<bool> IsAccountExistsAsync(string account);

        /// <summary>
        /// 檢查電子郵件是否存在
        /// </summary>
        /// <param name="email">電子郵件</param>
        /// <returns>是否存在</returns>
        Task<bool> IsEmailExistsAsync(string email);

        /// <summary>
        /// 檢查暱稱是否存在
        /// </summary>
        /// <param name="nickName">暱稱</param>
        /// <returns>是否存在</returns>
        Task<bool> IsNickNameExistsAsync(string nickName);
    }
} 
