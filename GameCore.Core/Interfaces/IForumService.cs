using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 論壇服務介面
    /// </summary>
    public interface IForumService
    {
        Task<Forum?> GetForumByIdAsync(int forumId);
        Task<Forum?> GetForumByKeyAsync(string key);
        Task<IEnumerable<Forum>> GetAllForumsAsync();
        Task<Forum> CreateForumAsync(int gameId, string name, string description);
        Task<bool> UpdateForumAsync(int forumId, string? name = null, string? description = null);
        Task<bool> DeleteForumAsync(int forumId);
        Task<int> GetPostCountAsync(int forumId);
    }
}