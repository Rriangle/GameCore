using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ��ѰT�� Repository ����
    /// </summary>
    public interface IChatMessageRepository
    {
        /// <summary>
        /// �ھڲ�ѫ�ID���o�T��
        /// </summary>
        Task<IEnumerable<ChatMessage>> GetByChatRoomIdAsync(int chatRoomId, int page, int pageSize);

        /// <summary>
        /// �s�W�T��
        /// </summary>
        Task<ChatMessage> AddAsync(ChatMessage message);

        /// <summary>
        /// �ھڲ�ѫǨ��o�T��
        /// </summary>
        Task<IEnumerable<ChatMessage>> GetMessagesByRoomAsync(int roomId);

        /// <summary>
        /// ���o��Ū�T���ƶq
        /// </summary>
        Task<int> GetUnreadCountAsync(int roomId, int userId);

        /// <summary>
        /// �аO���wŪ
        /// </summary>
        Task MarkAsReadAsync(int messageId);

        /// <summary>
        /// �s�W�T��
        /// </summary>
        Task<ChatMessage> Add(ChatMessage message);
    }
} 
