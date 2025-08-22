using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 簡化版 Services 實作 - 暫時解決建置問題
    /// </summary>
    
    public class SimpleMiniGameService : IMiniGameService
    {
        private readonly ILogger<SimpleMiniGameService> _logger;

        public SimpleMiniGameService(ILogger<SimpleMiniGameService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> StartGameAsync(int userId)
        {
            _logger.LogInformation($"開始小遊戲，使用者ID: {userId}");
            return await Task.FromResult(true);
        }

        public async Task<bool> EndGameAsync(int userId, string result)
        {
            _logger.LogInformation($"結束小遊戲，使用者ID: {userId}，結果: {result}");
            return await Task.FromResult(true);
        }
    }

    public class SimpleForumService : IForumService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SimpleForumService> _logger;

        public SimpleForumService(IUnitOfWork unitOfWork, ILogger<SimpleForumService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<Forum>> GetAllForumsAsync()
        {
            var forums = await _unitOfWork.ForumRepository.GetAllAsync();
            return forums.ToList();
        }

        public async Task<Forum?> GetForumByGameIdAsync(int gameId)
        {
            var forums = await _unitOfWork.ForumRepository.GetAllAsync();
            return forums.FirstOrDefault(f => f.GameId == gameId);
        }

        public async Task<GameCore.Core.Entities.Thread> CreateThreadAsync(int forumId, int authorId, string title)
        {
            var thread = new GameCore.Core.Entities.Thread
            {
                ForumId = forumId,
                AuthorUserId = authorId,
                Title = title,
                Status = "normal",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"建立新主題: {title}");
            return await Task.FromResult(thread);
        }

        public async Task<ThreadPost> ReplyToThreadAsync(long threadId, int authorId, string content, long? parentPostId = null)
        {
            var post = new ThreadPost
            {
                ThreadId = threadId,
                AuthorUserId = authorId,
                ContentMd = content,
                ParentPostId = parentPostId,
                Status = "normal",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"回覆主題: {threadId}");
            return await Task.FromResult(post);
        }
    }

    public class SimpleStoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SimpleStoreService> _logger;

        public SimpleStoreService(IUnitOfWork unitOfWork, ILogger<SimpleStoreService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<ProductInfo>> GetPopularProductsAsync(int count = 10)
        {
            var products = await _unitOfWork.StoreRepository.GetAllAsync();
            return products.Take(count).ToList();
        }

        public async Task<List<ProductInfo>> GetProductsByTypeAsync(string productType)
        {
            var products = await _unitOfWork.StoreRepository.GetAllAsync();
            return products.Where(p => p.ProductType == productType).ToList();
        }

        public async Task<OrderInfo> CreateOrderAsync(int userId, List<OrderItem> items)
        {
            var order = new OrderInfo
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Created",
                PaymentStatus = "Pending",
                OrderTotal = items.Sum(i => i.Subtotal)
            };

            _logger.LogInformation($"建立訂單，使用者ID: {userId}");
            return await Task.FromResult(order);
        }

        public async Task<bool> ProcessPaymentAsync(int orderId, decimal amount)
        {
            _logger.LogInformation($"處理付款，訂單ID: {orderId}，金額: {amount}");
            return await Task.FromResult(true);
        }
    }

    public class SimplePlayerMarketService : IPlayerMarketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SimplePlayerMarketService> _logger;

        public SimplePlayerMarketService(IUnitOfWork unitOfWork, ILogger<SimplePlayerMarketService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<PlayerMarketProductInfo>> GetUserProductsAsync(int userId)
        {
            var products = await _unitOfWork.PlayerMarketRepository.GetAllAsync();
            return products.Where(p => p.SellerId == userId).ToList();
        }

        public async Task<PlayerMarketProductInfo> ListProductAsync(int sellerId, string title, string name, decimal price, string? description = null)
        {
            var product = new PlayerMarketProductInfo
            {
                SellerId = sellerId,
                PProductTitle = title,
                PProductName = name,
                PProductDescription = description,
                Price = price,
                PStatus = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"上架商品: {title}");
            return await Task.FromResult(product);
        }

        public async Task<PlayerMarketOrderInfo> CreateOrderAsync(int productId, int buyerId, int quantity)
        {
            var order = new PlayerMarketOrderInfo
            {
                PProductId = productId,
                BuyerId = buyerId,
                POrderDate = DateTime.UtcNow,
                POrderStatus = "Created",
                PPaymentStatus = "Pending",
                PQuantity = quantity,
                POrderCreatedAt = DateTime.UtcNow,
                POrderUpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"建立市場訂單，商品ID: {productId}");
            return await Task.FromResult(order);
        }

        public async Task<List<PlayerMarketProductInfo>> GetPopularProductsAsync(int count = 10)
        {
            var products = await _unitOfWork.PlayerMarketRepository.GetAllAsync();
            return products.Take(count).ToList();
        }
    }

    public class SimpleNotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SimpleNotificationService> _logger;

        public SimpleNotificationService(IUnitOfWork unitOfWork, ILogger<SimpleNotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            _logger.LogInformation($"取得未讀通知，使用者ID: {userId}");
            return await Task.FromResult(new List<Notification>());
        }

        public async Task<Notification> CreateNotificationAsync(int sourceId, int actionId, int senderId, string title, string message, List<int> recipientIds)
        {
            var notification = new Notification
            {
                SourceId = sourceId,
                ActionId = actionId,
                SenderId = senderId,
                NotificationTitle = title,
                NotificationMessage = message,
                CreatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"建立通知: {title}");
            return await Task.FromResult(notification);
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            _logger.LogInformation($"標記通知為已讀，通知ID: {notificationId}，使用者ID: {userId}");
            await Task.CompletedTask;
        }

        public async Task SendSystemNotificationAsync(string title, string message)
        {
            _logger.LogInformation($"發送系統通知: {title}");
            await Task.CompletedTask;
        }
    }

    public class SimpleChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SimpleChatService> _logger;

        public SimpleChatService(IUnitOfWork unitOfWork, ILogger<SimpleChatService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ChatMessage> SendPrivateMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ChatContent = content,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsSent = true
            };

            _logger.LogInformation($"發送私人訊息，發送者: {senderId}，接收者: {receiverId}");
            return await Task.FromResult(message);
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2)
        {
            _logger.LogInformation($"取得聊天記錄，使用者1: {userId1}，使用者2: {userId2}");
            return await Task.FromResult(new List<ChatMessage>());
        }

        public async Task<List<ChatMessage>> GetRecentChatsAsync(int userId)
        {
            _logger.LogInformation($"取得最新聊天記錄，使用者ID: {userId}");
            return await Task.FromResult(new List<ChatMessage>());
        }

        public async Task MarkMessagesAsReadAsync(int senderId, int receiverId)
        {
            _logger.LogInformation($"標記訊息為已讀，發送者: {senderId}，接收者: {receiverId}");
            await Task.CompletedTask;
        }
    }

    public class SimpleUserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SimpleUserService> _logger;

        public SimpleUserService(IUnitOfWork unitOfWork, ILogger<SimpleUserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _unitOfWork.UserRepository.GetUserWithAllDataAsync(userId);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _logger.LogInformation($"更新使用者資料，使用者ID: {user.UserId}");
            return await Task.FromResult(true);
        }
    }
}

