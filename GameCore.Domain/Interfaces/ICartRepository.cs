using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �ʪ����ܮw����
    /// </summary>
    public interface ICartRepository : IRepository<ShoppingCart>
    {
        /// <summary>
        /// ���o�Τ᪺�ʪ���
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�ʪ���</returns>
        Task<ShoppingCart?> GetByUserIdAsync(int userId);

        /// <summary>
        /// ���o�ʪ�������
        /// </summary>
        /// <param name="cartId">�ʪ���ID</param>
        /// <returns>�ʪ������ئC��</returns>
        Task<IEnumerable<ShoppingCartItem>> GetCartItemsAsync(int cartId);

        /// <summary>
        /// �K�[�ӫ~���ʪ���
        /// </summary>
        /// <param name="cartId">�ʪ���ID</param>
        /// <param name="productId">�ӫ~ID</param>
        /// <param name="quantity">�ƶq</param>
        /// <returns>�K�[���G</returns>
        Task<bool> AddItemToCartAsync(int cartId, int productId, int quantity);

        /// <summary>
        /// ��s�ʪ������ؼƶq
        /// </summary>
        /// <param name="cartItemId">�ʪ�������ID</param>
        /// <param name="quantity">�s�ƶq</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity);

        /// <summary>
        /// �����ʪ�������
        /// </summary>
        /// <param name="cartItemId">�ʪ�������ID</param>
        /// <returns>�������G</returns>
        Task<bool> RemoveCartItemAsync(int cartItemId);

        /// <summary>
        /// �M���ʪ���
        /// </summary>
        /// <param name="cartId">�ʪ���ID</param>
        /// <returns>�M�ŵ��G</returns>
        Task<bool> ClearCartAsync(int cartId);

        /// <summary>
        /// ���o�ʪ����`���B
        /// </summary>
        /// <param name="cartId">�ʪ���ID</param>
        /// <returns>�`���B</returns>
        Task<decimal> GetCartTotalAsync(int cartId);
    }
} 
