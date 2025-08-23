using GameCore.Core.Entities;
using GameCore.Core.Interfaces;

namespace GameCore.Core.Services
{
    public class ForumService : IForumService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ForumService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Forum?> GetForumByIdAsync(int forumId)
        {
            return await _unitOfWork.ForumRepository.GetByIdAsync(forumId);
        }

        public async Task<Forum?> GetForumByKeyAsync(string key)
        {
            var forums = await _unitOfWork.ForumRepository.GetAllAsync();
            return forums.FirstOrDefault(f => f.Name == key);
        }

        public async Task<IEnumerable<Forum>> GetAllForumsAsync()
        {
            return await _unitOfWork.ForumRepository.GetAllAsync();
        }

        public async Task<Forum> CreateForumAsync(int gameId, string name, string description)
        {
            var forum = new Forum
            {
                GameId = gameId,
                Name = name,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _unitOfWork.ForumRepository.AddAsync(forum);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdateForumAsync(int forumId, string? name = null, string? description = null)
        {
            var forum = await GetForumByIdAsync(forumId);
            if (forum == null) return false;

            if (!string.IsNullOrEmpty(name))
                forum.Name = name;

            if (!string.IsNullOrEmpty(description))
                forum.Description = description;

            await _unitOfWork.ForumRepository.UpdateAsync(forum);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteForumAsync(int forumId)
        {
            var forum = await GetForumByIdAsync(forumId);
            if (forum == null) return false;

            await _unitOfWork.ForumRepository.DeleteByIdAsync(forumId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetPostCountAsync(int forumId)
        {
            var forums = await _unitOfWork.ForumRepository.GetAllAsync();
            var forum = forums.FirstOrDefault(f => f.ForumId == forumId);
            return forum?.Threads?.Count ?? 0;
        }
    }
}