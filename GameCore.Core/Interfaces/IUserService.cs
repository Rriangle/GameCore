using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 使用者服務介面
    /// </summary>
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(string username, string email, string password);
        Task<bool> UpdateUserAsync(int userId, string? username = null, string? email = null, string? password = null);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
    }
}