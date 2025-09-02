using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �׾� Repository ���f
    /// </summary>
    public interface IForumRepository : IRepository<Forum>
    {
        /// <summary>
        /// ������D�׾�
        /// </summary>
        /// <param name="limit">�ƶq����</param>
        /// <returns>�׾¦C��</returns>
        Task<IEnumerable<Forum>> GetActiveForumsAsync(int limit);

        /// <summary>
        /// �ھڤ�������׾�
        /// </summary>
        /// <param name="category">����</param>
        /// <returns>�׾¦C��</returns>
        Task<IEnumerable<Forum>> GetByCategoryAsync(string category);
    }
} 
