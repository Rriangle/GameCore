using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class CartRepository : Repository<ShoppingCart>, ICartRepository
    {
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(GameCoreDbContext context, ILogger<CartRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<ShoppingCart?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(c => c.Items)
                .ThenInclude(i => i.StoreProduct)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetCartItemsAsync(int cartId)
        {
            var cart = await _dbSet
                .Include(c => c.Items)
                .ThenInclude(i => i.StoreProduct)
                .FirstOrDefaultAsync(c => c.CartId == cartId);
            
            return cart?.Items ?? Enumerable.Empty<ShoppingCartItem>();
        }

        public async Task<bool> AddItemToCartAsync(int cartId, int productId, int quantity)
        {
            var cart = await _dbSet.FindAsync(cartId);
            if (cart == null) return false;

            var product = await _context.Set<StoreProduct>().FindAsync(productId);
            if (product == null) return false;

            var cartItem = new ShoppingCartItem
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price,
                AddedAt = DateTime.UtcNow
            };

            await _context.Set<ShoppingCartItem>().AddAsync(cartItem);
            return true;
        }

        public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.Set<ShoppingCartItem>().FindAsync(cartItemId);
            if (cartItem == null) return false;

            cartItem.Quantity = quantity;
            _context.Set<ShoppingCartItem>().Update(cartItem);
            return true;
        }

        public async Task<bool> RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.Set<ShoppingCartItem>().FindAsync(cartItemId);
            if (cartItem == null) return false;

            _context.Set<ShoppingCartItem>().Remove(cartItem);
            return true;
        }

        public async Task<bool> ClearCartAsync(int cartId)
        {
            var cart = await _dbSet.FindAsync(cartId);
            if (cart == null) return false;

            var items = await _context.Set<ShoppingCartItem>()
                .Where(i => i.CartId == cartId)
                .ToListAsync();

            _context.Set<ShoppingCartItem>().RemoveRange(items);
            return true;
        }

        public async Task<decimal> GetCartTotalAsync(int cartId)
        {
            var items = await _context.Set<ShoppingCartItem>()
                .Where(i => i.CartId == cartId)
                .ToListAsync();

            return items.Sum(i => i.UnitPrice * i.Quantity);
        }
    }
} 