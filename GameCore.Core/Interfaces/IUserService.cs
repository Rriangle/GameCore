using GameCore.Core.Models;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 用戶服務接口 - 定義用戶管理相關的方法
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 用戶註冊
        /// </summary>
        /// <param name="request">註冊請求</param>
        /// <returns>註冊結果</returns>
        Task<ServiceResult<UserRegistrationResultDto>> RegisterAsync(UserRegistrationDto request);

        /// <summary>
        /// 用戶登入
        /// </summary>
        /// <param name="request">登入請求</param>
        /// <returns>登入結果</returns>
        Task<ServiceResult<UserLoginResultDto>> LoginAsync(UserLoginDto request);

        /// <summary>
        /// 第三方登入
        /// </summary>
        /// <param name="request">第三方登入請求</param>
        /// <returns>登入結果</returns>
        Task<ServiceResult<UserLoginResultDto>> ThirdPartyLoginAsync(ThirdPartyLoginDto request);

        /// <summary>
        /// 取得用戶資料
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>用戶資料</returns>
        Task<ServiceResult<UserProfileDto>> GetProfileAsync(int userId);

        /// <summary>
        /// 更新用戶資料
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="request">更新請求</param>
        /// <returns>更新後的用戶資料</returns>
        Task<ServiceResult<UserProfileDto>> UpdateProfileAsync(int userId, UserProfileUpdateDto request);

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="request">變更密碼請求</param>
        /// <returns>變更結果</returns>
        Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto request);

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="request">忘記密碼請求</param>
        /// <returns>處理結果</returns>
        Task<ServiceResult> ForgotPasswordAsync(ForgotPasswordDto request);

        /// <summary>
        /// 重置密碼
        /// </summary>
        /// <param name="request">重置密碼請求</param>
        /// <returns>重置結果</returns>
        Task<ServiceResult> ResetPasswordAsync(ResetPasswordDto request);

        /// <summary>
        /// 驗證 JWT Token
        /// </summary>
        /// <param name="token">JWT Token</param>
        /// <returns>驗證結果</returns>
        Task<ServiceResult<TokenValidationResultDto>> ValidateTokenAsync(string token);

        /// <summary>
        /// 刷新 JWT Token
        /// </summary>
        /// <param name="refreshToken">刷新 Token</param>
        /// <returns>刷新結果</returns>
        Task<ServiceResult<TokenRefreshResultDto>> RefreshTokenAsync(string refreshToken);
    }
}