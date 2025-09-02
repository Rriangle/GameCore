using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ���a������Ʀs������
    /// </summary>
    public interface IPlayerMarketRepository
    {
        /// <summary>
        /// ���o�����ӫ~�C��
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsAsync();

        /// <summary>
        /// �ھ�ID���o�����ӫ~
        /// </summary>
        Task<PlayerMarketProductInfo?> GetMarketItemByIdAsync(int id);

        /// <summary>
        /// �j�M�����ӫ~
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> SearchMarketItemsAsync(string keyword);

        /// <summary>
        /// �ھ����O���o�����ӫ~
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsByCategoryAsync(string category);

        /// <summary>
        /// �ھڽ�aID���o�ӫ~
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetBySellerIdAsync(int sellerId);

        /// <summary>
        /// ���o�ӫ~���O
        /// </summary>
        Task<IEnumerable<string>> GetCategoriesAsync();

        /// <summary>
        /// �s�W�����ӫ~
        /// </summary>
        Task<PlayerMarketProductInfo> AddAsync(PlayerMarketProductInfo item);

        /// <summary>
        /// ��s�����ӫ~
        /// </summary>
        Task<PlayerMarketProductInfo> UpdateAsync(PlayerMarketProductInfo item);

        /// <summary>
        /// �R�������ӫ~
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// �T�{���
        /// </summary>
        Task<bool> ConfirmTransactionAsync(int transactionId);

        /// <summary>
        /// �������
        /// </summary>
        Task<bool> CancelTransactionAsync(int transactionId);

        /// <summary>
        /// �������
        /// </summary>
        Task<bool> ReviewTransactionAsync(int transactionId, int rating, string comment);

        /// <summary>
        /// ���o�����ӫ~�C��]�a�����^
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsAsync(int page, int pageSize);

        /// <summary>
        /// �j�M�����ӫ~�]�a�����^
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> SearchMarketItemsAsync(string keyword, int page, int pageSize);

        /// <summary>
        /// �ھ����O���o�����ӫ~�]�a�����^
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsByCategoryAsync(string category, int page, int pageSize);

        /// <summary>
        /// �s�W�����ӫ~
        /// </summary>
        Task<PlayerMarketProductInfo> Add(PlayerMarketProductInfo item);

        /// <summary>
        /// ��s�����ӫ~
        /// </summary>
        Task<PlayerMarketProductInfo> Update(PlayerMarketProductInfo item);

        /// <summary>
        /// ���o���D�ӫ~
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetActiveItemsAsync();

        /// <summary>
        /// �j�M�ӫ~
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> SearchItemsAsync(string keyword);

        /// <summary>
        /// �ھ�ID���o�ӫ~
        /// </summary>
        Task<PlayerMarketProductInfo?> GetByIdAsync(int id);

        /// <summary>
        /// �ھڽ�aID���o�ӫ~�]�a�����^
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetBySellerIdAsync(int sellerId, int page, int pageSize);

        /// <summary>
        /// �T�{���
        /// </summary>
        Task<bool> ConfirmTransactionAsync(int transactionId, int itemId);

        /// <summary>
        /// �������
        /// </summary>
        Task<bool> CancelTransactionAsync(int transactionId, int itemId);
    }
}
