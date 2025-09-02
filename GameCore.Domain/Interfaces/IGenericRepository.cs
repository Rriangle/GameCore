namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �q�� Repository ����
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// �ھ�ID���o����
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// ���o�Ҧ�����
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// �s�W����
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// ��s����
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// �R������
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// �ˬd����O�_�s�b
        /// </summary>
        Task<bool> ExistsAsync(int id);
    }
} 
