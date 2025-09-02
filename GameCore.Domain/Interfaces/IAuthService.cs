using GameCore.Core.DTOs;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �{�ҪA�Ȥ���
    /// ���ѥΤ���U�B�n�J�BOAuth���\��
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// �Τ���U
        /// </summary>
        /// <param name="request">���U�ШD</param>
        /// <returns>���U���G</returns>
        Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);

        /// <summary>
        /// �Τ�n�J
        /// </summary>
        /// <param name="request">�n�J�ШD</param>
        /// <returns>�n�J���G</returns>
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        /// <summary>
        /// OAuth�n�J
        /// </summary>
        /// <param name="request">OAuth�n�J�ШD</param>
        /// <returns>�n�J���G</returns>
        Task<LoginResponseDto> OAuthLoginAsync(OAuthLoginRequestDto request);

        /// <summary>
        /// ���o��e�Τ��T
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�Τ��T</returns>
        Task<UserInfoDto?> GetUserInfoAsync(int userId);

        /// <summary>
        /// ��s�ӤH���
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <param name="request">��s�ШD</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateProfileAsync(int userId, UpdateProfileRequestDto request);

        /// <summary>
        /// �ܧ�K�X
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <param name="request">�ܧ�K�X�ШD</param>
        /// <returns>�ܧ󵲪G</returns>
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequestDto request);

        /// <summary>
        /// ����JWT Token
        /// </summary>
        /// <param name="token">JWT Token</param>
        /// <returns>���ҵ��G</returns>
        Task<bool> ValidateTokenAsync(string token);

        /// <summary>
        /// ���s��zToken
        /// </summary>
        /// <param name="refreshToken">���s��zToken</param>
        /// <returns>�s��Token</returns>
        Task<string?> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// �n�X
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�n�X���G</returns>
        Task<bool> LogoutAsync(int userId);

        /// <summary>
        /// �ˬd�b���O�_�s�b
        /// </summary>
        /// <param name="account">�b��</param>
        /// <returns>�O�_�s�b</returns>
        Task<bool> IsAccountExistsAsync(string account);

        /// <summary>
        /// �ˬd�q�l�l��O�_�s�b
        /// </summary>
        /// <param name="email">�q�l�l��</param>
        /// <returns>�O�_�s�b</returns>
        Task<bool> IsEmailExistsAsync(string email);

        /// <summary>
        /// �ˬd�ʺ٬O�_�s�b
        /// </summary>
        /// <param name="nickName">�ʺ�</param>
        /// <returns>�O�_�s�b</returns>
        Task<bool> IsNickNameExistsAsync(string nickName);
    }
} 
