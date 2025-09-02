using GameCore.Core.DTOs;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �׾ªA�Ȥ���
    /// ���ѽ׾¬������~���޿�
    /// </summary>
    public interface IForumService
    {
        /// <summary>
        /// ���o�׾ª����C��
        /// </summary>
        /// <param name="request">�d�߱���</param>
        /// <returns>�׾ª����C��</returns>
        Task<PagedResponse<ForumInfo>> GetForumsAsync(ForumQueryRequest request);

        /// <summary>
        /// ���o�׾ª����Ա�
        /// </summary>
        /// <param name="forumId">�׾ª�ID</param>
        /// <returns>�׾ª����Ա�</returns>
        Task<ForumInfo?> GetForumByIdAsync(int forumId);

        /// <summary>
        /// ���o�D�D�C��
        /// </summary>
        /// <param name="request">�d�߱���</param>
        /// <returns>�D�D�C��</returns>
        Task<PagedResponse<ThreadListItem>> GetThreadsAsync(ThreadQueryRequest request);

        /// <summary>
        /// ���o�D�D�Ա�
        /// </summary>
        /// <param name="threadId">�D�DID</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�D�D�Ա�</returns>
        Task<ThreadDetail?> GetThreadByIdAsync(long threadId, int currentUserId);

        /// <summary>
        /// �إ߷s�D�D
        /// </summary>
        /// <param name="request">�إߥD�D�ШD</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�s�إߪ��D�DID</returns>
        Task<long> CreateThreadAsync(CreateThreadRequest request, int currentUserId);

        /// <summary>
        /// �إ߷s�^��
        /// </summary>
        /// <param name="request">�إߦ^�нШD</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�s�إߪ��^��ID</returns>
        Task<long> CreatePostAsync(CreatePostRequest request, int currentUserId);

        /// <summary>
        /// ��s�D�D
        /// </summary>
        /// <param name="threadId">�D�DID</param>
        /// <param name="title">�s���D</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> UpdateThreadAsync(long threadId, string title, int currentUserId);

        /// <summary>
        /// ��s�^��
        /// </summary>
        /// <param name="postId">�^��ID</param>
        /// <param name="content">�s���e</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> UpdatePostAsync(long postId, string content, int currentUserId);

        /// <summary>
        /// �R���D�D
        /// </summary>
        /// <param name="threadId">�D�DID</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> DeleteThreadAsync(long threadId, int currentUserId);

        /// <summary>
        /// �R���^��
        /// </summary>
        /// <param name="postId">�^��ID</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> DeletePostAsync(long postId, int currentUserId);

        /// <summary>
        /// �s�W�����]�g�B�����^
        /// </summary>
        /// <param name="request">�����ШD</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> AddReactionAsync(ReactionRequest request, int currentUserId);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="request">�����ШD</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> RemoveReactionAsync(ReactionRequest request, int currentUserId);

        /// <summary>
        /// �s�W����
        /// </summary>
        /// <param name="request">���ýШD</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> AddBookmarkAsync(BookmarkRequest request, int currentUserId);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="request">���ýШD</param>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> RemoveBookmarkAsync(BookmarkRequest request, int currentUserId);

        /// <summary>
        /// ���o�Τ᪺���æC��
        /// </summary>
        /// <param name="currentUserId">��e�Τ�ID</param>
        /// <param name="targetType">�ؼ�����</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���ƶq</param>
        /// <returns>���æC��</returns>
        Task<PagedResponse<object>> GetUserBookmarksAsync(int currentUserId, string targetType, int page = 1, int pageSize = 20);

        /// <summary>
        /// �j�M�D�D�M�^��
        /// </summary>
        /// <param name="keyword">����r</param>
        /// <param name="forumId">�׾ª�ID�]�i��^</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���ƶq</param>
        /// <returns>�j�M���G</returns>
        Task<PagedResponse<object>> SearchAsync(string keyword, int? forumId = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// �W�[�D�D�s������
        /// </summary>
        /// <param name="threadId">�D�DID</param>
        /// <returns>�O�_���\</returns>
        Task<bool> IncrementViewCountAsync(long threadId);

        /// <summary>
        /// ���o�����D�D
        /// </summary>
        /// <param name="forumId">�׾ª�ID�]�i��^</param>
        /// <param name="limit">�ƶq����</param>
        /// <returns>�����D�D�C��</returns>
        Task<List<ThreadListItem>> GetPopularThreadsAsync(int? forumId = null, int limit = 10);

        /// <summary>
        /// ���o�̷s�D�D
        /// </summary>
        /// <param name="forumId">�׾ª�ID�]�i��^</param>
        /// <param name="limit">�ƶq����</param>
        /// <returns>�̷s�D�D�C��</returns>
        Task<List<ThreadListItem>> GetLatestThreadsAsync(int? forumId = null, int limit = 10);
    }
} 
