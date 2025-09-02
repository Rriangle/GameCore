using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �����ӫ~�ܮw����
    /// </summary>
    public interface IMarketItemRepository : IRepository<MarketItem>
    {
        /// <summary>
        /// ���o���D�������ӫ~
        /// </summary>
        /// <param name="category">�ӫ~����</param>
        /// <param name="minPrice">�̧C����</param>
        /// <param name="maxPrice">�̰�����</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <returns>�����ӫ~�C��</returns>
        Task<IEnumerable<MarketItem>> GetActiveItemsAsync(string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// �j�M�����ӫ~
        /// </summary>
        /// <param name="keyword">����r</param>
        /// <param name="category">�ӫ~����</param>
        /// <param name="minPrice">�̧C����</param>
        /// <param name="maxPrice">�̰�����</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <returns>�j�M���G</returns>
        Task<IEnumerable<MarketItem>> SearchItemsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// ���o��a���ӫ~
        /// </summary>
        /// <param name="sellerId">��aID</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <returns>�ӫ~�C��</returns>
        Task<IEnumerable<MarketItem>> GetBySellerAsync(int sellerId, int page = 1, int pageSize = 20);

        /// <summary>
        /// ���o�ӫ~�Ա�
        /// </summary>
        /// <param name="productId">�ӫ~ID</param>
        /// <returns>�ӫ~�Ա�</returns>
        Task<MarketItem?> GetByIdWithDetailsAsync(int productId);

        /// <summary>
        /// ��s�ӫ~���A
        /// </summary>
        /// <param name="productId">�ӫ~ID</param>
        /// <param name="status">�s���A</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateStatusAsync(int productId, string status);

        /// <summary>
        /// ���o�����ӫ~
        /// </summary>
        /// <param name="limit">�ƶq����</param>
        /// <returns>�����ӫ~�C��</returns>
        Task<IEnumerable<MarketItem>> GetPopularItemsAsync(int limit = 10);
    }
} 
