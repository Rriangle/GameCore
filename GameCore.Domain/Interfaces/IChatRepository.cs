using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ��ѫ� Repository ����
    /// </summary>
    public interface IChatRepository
    {
        /// <summary>
        /// �ھ�ID���o��ѫ�
        /// </summary>
        Task<ChatRoom?> GetByIdAsync(int id);

        /// <summary>
        /// ���o��ѫǦ���
        /// </summary>
        Task<ChatRoomMember?> GetRoomMemberAsync(int chatRoomId, int userId);

        /// <summary>
        /// ���o��ѫǩҦ�����
        /// </summary>
        Task<IEnumerable<ChatRoomMember>> GetRoomMembersAsync(int chatRoomId);

        /// <summary>
        /// ������ѫǦ���
        /// </summary>
        Task RemoveMember(int chatRoomId, int userId);

        /// <summary>
        /// �ھڲ�ѫ�ID���o�T��
        /// </summary>
        Task<IEnumerable<ChatMessage>> GetByChatRoomIdAsync(int chatRoomId, int page, int pageSize);

        /// <summary>
        /// �s�W��ѫ�
        /// </summary>
        Task<ChatRoom> AddAsync(ChatRoom chatRoom);

        /// <summary>
        /// �s�W��ѫǦ���
        /// </summary>
        Task<ChatRoomMember> AddMember(ChatRoomMember member);

        /// <summary>
        /// ��s��ѫǦ���
        /// </summary>
        Task<ChatRoomMember> UpdateRoomMemberAsync(ChatRoomMember member);

        /// <summary>
        /// ��s��ѫ�
        /// </summary>
        Task<ChatRoom> Update(ChatRoom chatRoom);

        /// <summary>
        /// ���o��ѫǦ���
        /// </summary>
        Task<ChatRoomMember?> GetChatRoomMemberAsync(int chatRoomId, int userId);

        /// <summary>
        /// �ھڥΤ�ID���o��ѫ�
        /// </summary>
        Task<IEnumerable<ChatRoom>> GetChatRoomsByUserIdAsync(int userId);

        /// <summary>
        /// �s�W��ѫ�
        /// </summary>
        Task<ChatRoom> Add(ChatRoom chatRoom);
    }
}
