using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �p��T�� Repository ����
    /// </summary>
    public interface IPrivateMessageRepository
    {
        /// <summary>
        /// �ھڨp��ID���o�T��
        /// </summary>
        Task<IEnumerable<PrivateMessage>> GetByPrivateChatIdAsync(int privateChatId, int page, int pageSize);

        /// <summary>
        /// �R���T��
        /// </summary>
        Task Delete(PrivateMessage message);

        /// <summary>
        /// �s�W�T��
        /// </summary>
        Task<PrivateMessage> AddAsync(PrivateMessage message);

        /// <summary>
        /// �ھڲ��ID���o�T��
        /// </summary>
        Task<IEnumerable<PrivateMessage>> GetMessagesByChatIdAsync(int chatId, int page, int pageSize);

        /// <summary>
        /// �ھ�ID���o�T��
        /// </summary>
        Task<PrivateMessage?> GetByIdAsync(int messageId);

        /// <summary>
        /// ���o��Ū�T���ƶq
        /// </summary>
        Task<int> GetUnreadCountAsync(int userId, int privateChatId);
    }
} 
