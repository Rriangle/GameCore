using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �޲z����ƭܮw����
    /// </summary>
    public interface IManagerDataRepository : IRepository<ManagerData>
    {
        /// <summary>
        /// �ھں޲z��ID���o���
        /// </summary>
        /// <param name="managerId">�޲z��ID</param>
        /// <returns>�޲z����ƦC��</returns>
        Task<IEnumerable<ManagerData>> GetByManagerIdAsync(int managerId);

        /// <summary>
        /// �ھڸ�������M��Ȩ��o���
        /// </summary>
        /// <param name="dataType">�������</param>
        /// <param name="dataKey">������</param>
        /// <returns>�޲z�����</returns>
        Task<ManagerData?> GetByTypeAndKeyAsync(string dataType, string dataKey);

        /// <summary>
        /// �ھڸ���������o��ƦC��
        /// </summary>
        /// <param name="dataType">�������</param>
        /// <returns>�޲z����ƦC��</returns>
        Task<IEnumerable<ManagerData>> GetByDataTypeAsync(string dataType);

        /// <summary>
        /// �]�w�޲z�����
        /// </summary>
        /// <param name="managerId">�޲z��ID</param>
        /// <param name="dataType">�������</param>
        /// <param name="dataKey">������</param>
        /// <param name="dataValue">��ƭ�</param>
        /// <returns>�]�w���G</returns>
        Task<bool> SetManagerDataAsync(int managerId, string dataType, string dataKey, string dataValue);

        /// <summary>
        /// �R���޲z�����
        /// </summary>
        /// <param name="managerId">�޲z��ID</param>
        /// <param name="dataType">�������</param>
        /// <param name="dataKey">������</param>
        /// <returns>�R�����G</returns>
        Task<bool> DeleteManagerDataAsync(int managerId, string dataType, string dataKey);
    }
} 
