using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ���~ Repository ���f
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// �ھڤ���������~
        /// </summary>
        /// <param name="category">����</param>
        /// <returns>���~�C��</returns>
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);

        /// <summary>
        /// �j�����~
        /// </summary>
        /// <param name="keyword">����r</param>
        /// <returns>���~�C��</returns>
        Task<IEnumerable<Product>> SearchAsync(string keyword);

        /// <summary>
        /// ����������~
        /// </summary>
        /// <param name="limit">�ƶq����</param>
        /// <returns>���~�C��</returns>
        Task<IEnumerable<Product>> GetPopularAsync(int limit);
    }
} 
