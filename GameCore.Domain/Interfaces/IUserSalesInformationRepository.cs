using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �Τ�P���T Repository ����
    /// </summary>
    public interface IUserSalesInformationRepository : IRepository<UserSalesInformation>
    {
        /// <summary>
        /// �ھڥΤ�ID���o�P���T
        /// </summary>
        Task<UserSalesInformation?> GetByUserIdAsync(int userId);

        /// <summary>
        /// �s�W�P���T
        /// </summary>
        Task<UserSalesInformation> AddAsync(UserSalesInformation info);

        /// <summary>
        /// ��s�P���T
        /// </summary>
        Task UpdateAsync(UserSalesInformation info);

        /// <summary>
        /// �R���P���T
        /// </summary>
        Task DeleteAsync(UserSalesInformation info);
    }
} 
