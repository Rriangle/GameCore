using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �޲z�̨����v�� Repository ����
    /// </summary>
    public interface IManagerRolePermissionRepository : IRepository<ManagerRolePermission>
    {
        /// <summary>
        /// �ھڨ�����o�v��
        /// </summary>
        Task<IEnumerable<ManagerRolePermission>> GetByRoleAsync(string role);

        /// <summary>
        /// �ھ��v�����o����
        /// </summary>
        Task<IEnumerable<ManagerRolePermission>> GetByPermissionAsync(string permission);

        /// <summary>
        /// �s�W�����v��
        /// </summary>
        Task<ManagerRolePermission> AddAsync(ManagerRolePermission permission);

        /// <summary>
        /// ��s�����v��
        /// </summary>
        Task UpdateAsync(ManagerRolePermission permission);

        /// <summary>
        /// �R�������v��
        /// </summary>
        Task DeleteAsync(ManagerRolePermission permission);
    }
} 
