using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �ө���Ʀs������
    /// </summary>
    public interface IStoreRepository
    {
        /// <summary>
        /// �ھ�ID���o�ӫ~
        /// </summary>
        Task<Product?> GetProductByIdAsync(int id);

        /// <summary>
        /// ���o�Ҧ��ӫ~
        /// </summary>
        Task<IEnumerable<Product>> GetAllProductsAsync();

        /// <summary>
        /// �ھ����O���o�ӫ~
        /// </summary>
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);

        /// <summary>
        /// �j�M�ӫ~
        /// </summary>
        Task<IEnumerable<Product>> SearchProductsAsync(string keyword);

        /// <summary>
        /// �s�W�ӫ~
        /// </summary>
        Task<Product> AddProductAsync(Product product);

        /// <summary>
        /// ��s�ӫ~
        /// </summary>
        Task<Product> UpdateProductAsync(Product product);

        /// <summary>
        /// �R���ӫ~
        /// </summary>
        Task DeleteProductAsync(int id);
    }
} 
