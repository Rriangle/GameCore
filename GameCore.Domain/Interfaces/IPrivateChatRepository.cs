using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 私人聊天 Repository 介面
    /// </summary>
    public interface IPrivateChatRepository : IRepository<PrivateChat>
    {
        /// <summary>
        /// 根據用戶ID取得私人聊天
        /// </summary>
        Task<IEnumerable<PrivateChat>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 根據兩個用戶ID取得私人聊天
        /// </summary>
        Task<PrivateChat?> GetByUsersAsync(int user1Id, int user2Id);

        /// <summary>
        /// 新增私人聊天
        /// </summary>
        Task<PrivateChat> AddAsync(PrivateChat privateChat);

        /// <summary>
        /// 更新私人聊天
        /// </summary>
        Task UpdateAsync(PrivateChat privateChat);

        /// <summary>
        /// 刪除私人聊天
        /// </summary>
        Task DeleteAsync(PrivateChat privateChat);

        /// <summary>
        /// 取得私人聊天
        /// </summary>
        Task<PrivateChat?> GetPrivateChatAsync(int user1Id, int user2Id);

        /// <summary>
        /// 新增私人聊天
        /// </summary>
        Task<PrivateChat> Add(PrivateChat privateChat);

        /// <summary>
        /// 更新私人聊天
        /// </summary>
        Task Update(PrivateChat privateChat);

        /// <summary>
        /// 根據用戶ID取得私人聊天列表
        /// </summary>
        Task<IEnumerable<PrivateChat>> GetPrivateChatsByUserIdAsync(int userId);
    }
} 
