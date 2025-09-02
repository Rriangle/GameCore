using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �q����Ʀs������
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// �ھ�ID���o�q��
        /// </summary>
        Task<Notification?> GetByIdAsync(int id);

        /// <summary>
        /// �ھڥΤ�ID���o�q���C��
        /// </summary>
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);

        /// <summary>
        /// �s�W�q��
        /// </summary>
        Task<Notification> AddAsync(Notification notification);

        /// <summary>
        /// ��s�q��
        /// </summary>
        Task<Notification> UpdateAsync(Notification notification);

        /// <summary>
        /// �R���q��
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// �Ы��d������q��
        /// </summary>
        Task CreatePetColorChangeNotificationAsync(int userId, string oldColor, string newColor);

        /// <summary>
        /// �ھڥΤ�M�ʧ@���o�q��
        /// </summary>
        Task<Notification?> GetByUserAndActionAsync(int userId, string action);

        /// <summary>
        /// �аO�Ҧ��q�����wŪ
        /// </summary>
        Task MarkAllAsReadAsync(int userId);

        /// <summary>
        /// ���o��Ū�q���ƶq
        /// </summary>
        Task<int> GetUnreadCountAsync(int userId);
    }

    /// <summary>
    /// �q���ӷ��ܮw����
    /// </summary>
    public interface INotificationSourceRepository : IRepository<NotificationSource>
    {
        /// <summary>
        /// ���o�Ҧ��q���ӷ�
        /// </summary>
        /// <returns>�q���ӷ��C��</returns>
        Task<IEnumerable<NotificationSource>> GetAllSourcesAsync();

        /// <summary>
        /// ���o�q���ӷ�
        /// </summary>
        /// <param name="sourceName">�ӷ��W��</param>
        /// <returns>�q���ӷ�</returns>
        Task<NotificationSource?> GetSourceByNameAsync(string sourceName);
    }

    /// <summary>
    /// �q���ʧ@�ܮw����
    /// </summary>
    public interface INotificationActionRepository : IRepository<NotificationAction>
    {
        /// <summary>
        /// ���o�Ҧ��q���ʧ@
        /// </summary>
        /// <returns>�q���ʧ@�C��</returns>
        Task<IEnumerable<NotificationAction>> GetAllActionsAsync();

        /// <summary>
        /// ���o�q���ʧ@
        /// </summary>
        /// <param name="actionName">�ʧ@�W��</param>
        /// <returns>�q���ʧ@</returns>
        Task<NotificationAction?> GetActionByNameAsync(string actionName);
    }
}
