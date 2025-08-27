using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    public interface IReplyRepository
    {
        Task<Reply> CreateAsync(Reply reply);
        Task<Reply> GetByIdAsync(int id);
        Task<IEnumerable<Reply>> GetByPostIdAsync(int postId, int page = 1, int pageSize = 20);
        Task<Reply> UpdateAsync(Reply reply);
        Task<bool> DeleteAsync(int id);
        Task<int> GetReplyCountByPostIdAsync(int postId);
        Task<IEnumerable<Reply>> GetByUserIdAsync(int userId, int page = 1, int pageSize = 20);
    }
} 