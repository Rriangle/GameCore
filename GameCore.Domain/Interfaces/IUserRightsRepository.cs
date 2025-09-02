using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �Τ��v�� Repository ����
    /// </summary>
    public interface IUserRightsRepository : IRepository<UserRights>
    {
        /// <summary>
        /// �ھڥΤ�ID���o�v��
        /// </summary>
        Task<UserRights?> GetByUserIdAsync(int userId);

        /// <summary>
        /// �ھ��v���������o�v��
        /// </summary>
        Task<IEnumerable<UserRights>> GetByRightTypeAsync(string rightType);

        /// <summary>
        /// �s�W�v��
        /// </summary>
        Task<UserRights> AddAsync(UserRights rights);

        /// <summary>
        /// ��s�v��
        /// </summary>
        Task UpdateAsync(UserRights rights);

        /// <summary>
        /// �R���v��
        /// </summary>
        Task DeleteAsync(UserRights rights);

        /// <summary>
        /// ���o�Ҧ��v��
        /// </summary>
        Task<IEnumerable<UserRights>> GetAll();
    }
} 
