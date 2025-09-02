using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �p�H��� Repository ����
    /// </summary>
    public interface IPrivateChatRepository : IRepository<PrivateChat>
    {
        /// <summary>
        /// �ھڥΤ�ID���o�p�H���
        /// </summary>
        Task<IEnumerable<PrivateChat>> GetByUserIdAsync(int userId);

        /// <summary>
        /// �ھڨ�ӥΤ�ID���o�p�H���
        /// </summary>
        Task<PrivateChat?> GetByUsersAsync(int user1Id, int user2Id);

        /// <summary>
        /// �s�W�p�H���
        /// </summary>
        Task<PrivateChat> AddAsync(PrivateChat privateChat);

        /// <summary>
        /// ��s�p�H���
        /// </summary>
        Task UpdateAsync(PrivateChat privateChat);

        /// <summary>
        /// �R���p�H���
        /// </summary>
        Task DeleteAsync(PrivateChat privateChat);

        /// <summary>
        /// ���o�p�H���
        /// </summary>
        Task<PrivateChat?> GetPrivateChatAsync(int user1Id, int user2Id);

        /// <summary>
        /// �s�W�p�H���
        /// </summary>
        Task<PrivateChat> Add(PrivateChat privateChat);

        /// <summary>
        /// ��s�p�H���
        /// </summary>
        Task Update(PrivateChat privateChat);

        /// <summary>
        /// �ھڥΤ�ID���o�p�H��ѦC��
        /// </summary>
        Task<IEnumerable<PrivateChat>> GetPrivateChatsByUserIdAsync(int userId);
    }
} 
