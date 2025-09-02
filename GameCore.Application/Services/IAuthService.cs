using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 認證服務介面
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 用戶登入
        /// </summary>
        /// <param name="request">登入請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>登入結果</returns>
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 用戶註冊
        /// </summary>
        /// <param name="request">註冊請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>註冊結果</returns>
        Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 用戶登出
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>登出結果</returns>
        Task<OperationResult> LogoutAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="request">變更密碼請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>變更結果</returns>
        Task<OperationResult> ChangePasswordAsync(int userId, ChangePasswordRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 重新整理令牌
        /// </summary>
        /// <param name="request">重新整理令牌請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>重新整理結果</returns>
        Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 驗證令牌
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>驗證結果</returns>
        Task<Result<UserResponse>> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得當前用戶
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>用戶資訊</returns>
        Task<Result<UserResponse>> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default);
    }
} 