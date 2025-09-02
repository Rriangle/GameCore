using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �޲z�̸�Ʀs������
    /// </summary>
    public interface IManagerRepository
    {
        /// <summary>
        /// �ھ�ID���o�޲z��
        /// </summary>
        Task<ManagerData?> GetByIdAsync(int id);

        /// <summary>
        /// �ھڥΤ�W���o�޲z��
        /// </summary>
        Task<ManagerData?> GetByUsernameAsync(string username);

        /// <summary>
        /// �ھ�Email���o�޲z��
        /// </summary>
        Task<ManagerData?> GetByEmailAsync(string email);

        /// <summary>
        /// �ˬd�Τ�W�O�_�s�b
        /// </summary>
        Task<bool> ExistsByUsernameAsync(string username);

        /// <summary>
        /// �ˬdEmail�O�_�s�b
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// �s�W�޲z��
        /// </summary>
        Task<ManagerData> AddAsync(ManagerData manager);

        /// <summary>
        /// ��s�޲z��
        /// </summary>
        Task<ManagerData> UpdateAsync(ManagerData manager);

        /// <summary>
        /// �R���޲z��
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// ��s�޲z��
        /// </summary>
        Task<ManagerData> Update(ManagerData manager);

        /// <summary>
        /// ���o�Ҧ��޲z��
        /// </summary>
        Task<IEnumerable<ManagerData>> GetAllAsync();

        /// <summary>
        /// �ˬd�޲z�̬O�_�s�b
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// �s�W�޲z��
        /// </summary>
        Task<ManagerData> Add(ManagerData manager);
    }
}
