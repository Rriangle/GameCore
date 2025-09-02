using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �^�� Repository ����
    /// </summary>
    public interface IReplyRepository : IRepository<Reply>
    {
        /// <summary>
        /// �ھڥD�DID���o�^��
        /// </summary>
        Task<IEnumerable<Reply>> GetByThreadIdAsync(long threadId);

        /// <summary>
        /// �ھڧ@��ID���o�^��
        /// </summary>
        Task<IEnumerable<Reply>> GetByAuthorIdAsync(int authorId);

        /// <summary>
        /// �s�W�^��
        /// </summary>
        Task<Reply> AddAsync(Reply reply);

        /// <summary>
        /// ��s�^��
        /// </summary>
        Task UpdateAsync(Reply reply);

        /// <summary>
        /// �R���^��
        /// </summary>
        Task DeleteAsync(Reply reply);
    }
} 
