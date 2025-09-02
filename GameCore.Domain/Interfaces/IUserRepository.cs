using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �Τ�ܮw����
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// �ھڹq�l�l����o�Τ�
        /// </summary>
        /// <param name="email">�q�l�l��</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�Τ�</returns>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// �ھڥΤ�W���o�Τ�
        /// </summary>
        /// <param name="userName">�Τ�W</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�Τ�</returns>
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// �ˬd�q�l�l��O�_�s�b
        /// </summary>
        /// <param name="email">�q�l�l��</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�O�_�s�b</returns>
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// �ˬd�Τ�W�O�_�s�b
        /// </summary>
        /// <param name="userName">�Τ�W</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�O�_�s�b</returns>
        Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// ���o�ҥΪ��Τ�
        /// </summary>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�ҥΪ��Τ�C��</returns>
        Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// �ھڨ�����o�Τ�
        /// </summary>
        /// <param name="role">����</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�Τ�C��</returns>
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role, CancellationToken cancellationToken = default);

        /// <summary>
        /// �j�M�Τ�
        /// </summary>
        /// <param name="searchTerm">�j�M����r</param>
        /// <param name="pageNumber">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�Τ�C��</returns>
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// ���o�Τ�έp��T
        /// </summary>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�έp��T</returns>
        Task<object> GetUserStatsAsync(CancellationToken cancellationToken = default);
    }
} 
