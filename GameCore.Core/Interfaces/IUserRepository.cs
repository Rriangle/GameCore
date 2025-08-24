using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UpdateAsync(User user);
        Task<bool> AddPointsAsync(int userId, int points, string reason);
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<IEnumerable<User>> GetUsersAsync(int page = 1, int pageSize = 20);
        Task<int> GetUserCountAsync();
    }
}