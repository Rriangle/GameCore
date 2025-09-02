using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 購物車倉庫介面
    /// </summary>
    public interface ICartRepository : IRepository<ShoppingCart>
    {
        /// <summary>
        /// 取得用戶的購物車
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>購物車</returns>
        Task<ShoppingCart?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 取得購物車項目
        /// </summary>
        /// <param name="cartId">購物車ID</param>
        /// <returns>購物車項目列表</returns>
        Task<IEnumerable<ShoppingCartItem>> GetCartItemsAsync(int cartId);

        /// <summary>
        /// 添加商品到購物車
        /// </summary>
        /// <param name="cartId">購物車ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">數量</param>
        /// <returns>添加結果</returns>
        Task<bool> AddItemToCartAsync(int cartId, int productId, int quantity);

        /// <summary>
        /// 更新購物車項目數量
        /// </summary>
        /// <param name="cartItemId">購物車項目ID</param>
        /// <param name="quantity">新數量</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity);

        /// <summary>
        /// 移除購物車項目
        /// </summary>
        /// <param name="cartItemId">購物車項目ID</param>
        /// <returns>移除結果</returns>
        Task<bool> RemoveCartItemAsync(int cartItemId);

        /// <summary>
        /// 清空購物車
        /// </summary>
        /// <param name="cartId">購物車ID</param>
        /// <returns>清空結果</returns>
        Task<bool> ClearCartAsync(int cartId);

        /// <summary>
        /// 取得購物車總金額
        /// </summary>
        /// <param name="cartId">購物車ID</param>
        /// <returns>總金額</returns>
        Task<decimal> GetCartTotalAsync(int cartId);
    }
} 
