using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ñ��ܮw����
    /// </summary>
    public interface ISignInRepository : IRepository<SignInRecord>
    {
        /// <summary>
        /// �ˬd�ϥΪ̬O�_�w�b���w���ñ��
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <param name="date">ñ����</param>
        /// <returns>�O�_�wñ��</returns>
        Task<bool> HasSignedInTodayAsync(int userId, DateTime date);

        /// <summary>
        /// ���o�ϥΪ̪�ñ��έp
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <param name="year">�~��</param>
        /// <param name="month">���</param>
        /// <returns>ñ��έp</returns>
        Task<SignInStatistics?> GetStatisticsAsync(int userId, int year, int month);

        /// <summary>
        /// ���o�ϥΪ̪��s��ñ��Ѽ�
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <returns>�s��ñ��Ѽ�</returns>
        Task<int> GetConsecutiveDaysAsync(int userId);

        /// <summary>
        /// ���o�ϥΪ̪����ñ��O��
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <param name="year">�~��</param>
        /// <param name="month">���</param>
        /// <returns>ñ��O���C��</returns>
        Task<IEnumerable<SignInRecord>> GetMonthlyRecordsAsync(int userId, int year, int month);

        /// <summary>
        /// �إߩΧ�sñ��έp
        /// </summary>
        /// <param name="statistics">ñ��έp</param>
        /// <returns>�ާ@���G</returns>
        Task<bool> UpsertStatisticsAsync(SignInStatistics statistics);

        /// <summary>
        /// �ھڥΤ�ID���oñ��O��
        /// </summary>
        Task<SignInRecord?> GetByUserIdAsync(int userId);
    }
}
