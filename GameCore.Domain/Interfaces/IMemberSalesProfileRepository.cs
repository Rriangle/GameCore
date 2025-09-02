using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �|���P���� Repository ����
    /// </summary>
    public interface IMemberSalesProfileRepository : IRepository<MemberSalesProfile>
    {
        /// <summary>
        /// �ھڥΤ�ID���o�P����
        /// </summary>
        Task<MemberSalesProfile?> GetByUserIdAsync(int userId);

        /// <summary>
        /// �ھڪ��A���o�P����
        /// </summary>
        Task<IEnumerable<MemberSalesProfile>> GetByStatusAsync(string status);

        /// <summary>
        /// �s�W�P����
        /// </summary>
        Task<MemberSalesProfile> AddAsync(MemberSalesProfile profile);

        /// <summary>
        /// ��s�P����
        /// </summary>
        Task UpdateAsync(MemberSalesProfile profile);

        /// <summary>
        /// �R���P����
        /// </summary>
        Task DeleteAsync(MemberSalesProfile profile);

        /// <summary>
        /// ���o�Ҧ��P����
        /// </summary>
        Task<IEnumerable<MemberSalesProfile>> GetAll();
    }
} 
