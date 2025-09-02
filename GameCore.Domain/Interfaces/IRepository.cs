using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ��¦�ܮw����
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// �ھ� ID ���o����
        /// </summary>
        /// <param name="id">���� ID</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>����</returns>
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// ���o�Ҧ�����
        /// </summary>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>����C��</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// �ھڱ�����o����
        /// </summary>
        /// <param name="predicate">�����F��</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>����C��</returns>
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// �ھڱ�����o��@����
        /// </summary>
        /// <param name="predicate">�����F��</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>����</returns>
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// �s�W����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�s�W������</returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// ��s����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>��s���G</returns>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// �R������
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�R�����G</returns>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// �ھ� ID �R������
        /// </summary>
        /// <param name="id">���� ID</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�R�����G</returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// �ˬd����O�_�s�b
        /// </summary>
        /// <param name="id">���� ID</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�O�_�s�b</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// �ھڱ����ˬd����O�_�s�b
        /// </summary>
        /// <param name="predicate">�����F��</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�O�_�s�b</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// ���o����ƶq
        /// </summary>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>����ƶq</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// �ھڱ�����o����ƶq
        /// </summary>
        /// <param name="predicate">�����F��</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>����ƶq</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// �����d��
        /// </summary>
        /// <param name="pageNumber">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�������G</returns>
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// ��������d��
        /// </summary>
        /// <param name="predicate">�����F��</param>
        /// <param name="pageNumber">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <param name="cancellationToken">�����O�P</param>
        /// <returns>�������G</returns>
        Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
} 
