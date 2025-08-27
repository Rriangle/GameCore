using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.DTOs;
using GameCore.Core.Services;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class PlayerMarketService : IPlayerMarketService
    {
        private readonly IPlayerMarketRepository _playerMarketRepository;
        private readonly IMarketTransactionRepository _marketTransactionRepository;
        private readonly IMarketReviewRepository _marketReviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PlayerMarketService> _logger;

        public PlayerMarketService(
            IPlayerMarketRepository playerMarketRepository,
            IMarketTransactionRepository marketTransactionRepository,
            IMarketReviewRepository marketReviewRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<PlayerMarketService> logger)
        {
            _playerMarketRepository = playerMarketRepository;
            _marketTransactionRepository = marketTransactionRepository;
            _marketReviewRepository = marketReviewRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<MarketItemDto>> GetMarketItemsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var marketItems = await _playerMarketRepository.GetMarketItemsAsync(page, pageSize);
                return marketItems.Select(mi => new MarketItemDto
                {
                    Id = mi.Id,
                    SellerId = mi.SellerId,
                    SellerName = mi.Seller?.Username ?? "未知賣家",
                    Title = mi.Title,
                    Description = mi.Description,
                    Price = mi.Price,
                    Category = mi.Category,
                    Condition = mi.Condition,
                    ImageUrl = mi.ImageUrl,
                    Status = mi.Status,
                    CreatedAt = mi.CreatedAt,
                    UpdatedAt = mi.UpdatedAt,
                    ExpiresAt = mi.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取市場商品列表失敗");
                return Enumerable.Empty<MarketItemDto>();
            }
        }

        public async Task<MarketItemDto> GetMarketItemByIdAsync(int marketItemId)
        {
            try
            {
                var marketItem = await _playerMarketRepository.GetMarketItemByIdAsync(marketItemId);
                if (marketItem == null) return null;

                return new MarketItemDto
                {
                    Id = marketItem.Id,
                    SellerId = marketItem.SellerId,
                    SellerName = marketItem.Seller?.Username ?? "未知賣家",
                    Title = marketItem.Title,
                    Description = marketItem.Description,
                    Price = marketItem.Price,
                    Category = marketItem.Category,
                    Condition = marketItem.Condition,
                    ImageUrl = marketItem.ImageUrl,
                    Status = marketItem.Status,
                    CreatedAt = marketItem.CreatedAt,
                    UpdatedAt = marketItem.UpdatedAt,
                    ExpiresAt = marketItem.ExpiresAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取市場商品詳情失敗: {MarketItemId}", marketItemId);
                return null;
            }
        }

        public async Task<IEnumerable<MarketItemDto>> SearchMarketItemsAsync(string keyword, int page = 1, int pageSize = 20)
        {
            try
            {
                var marketItems = await _playerMarketRepository.SearchMarketItemsAsync(keyword, page, pageSize);
                return marketItems.Select(mi => new MarketItemDto
                {
                    Id = mi.Id,
                    SellerId = mi.SellerId,
                    SellerName = mi.Seller?.Username ?? "未知賣家",
                    Title = mi.Title,
                    Description = mi.Description,
                    Price = mi.Price,
                    Category = mi.Category,
                    Condition = mi.Condition,
                    ImageUrl = mi.ImageUrl,
                    Status = mi.Status,
                    CreatedAt = mi.CreatedAt,
                    UpdatedAt = mi.UpdatedAt,
                    ExpiresAt = mi.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋市場商品失敗: {Keyword}", keyword);
                return Enumerable.Empty<MarketItemDto>();
            }
        }

        public async Task<IEnumerable<MarketItemDto>> GetMarketItemsByCategoryAsync(string category, int page = 1, int pageSize = 20)
        {
            try
            {
                var marketItems = await _playerMarketRepository.GetMarketItemsByCategoryAsync(category, page, pageSize);
                return marketItems.Select(mi => new MarketItemDto
                {
                    Id = mi.Id,
                    SellerId = mi.SellerId,
                    SellerName = mi.Seller?.Username ?? "未知賣家",
                    Title = mi.Title,
                    Description = mi.Description,
                    Price = mi.Price,
                    Category = mi.Category,
                    Condition = mi.Condition,
                    ImageUrl = mi.ImageUrl,
                    Status = mi.Status,
                    CreatedAt = mi.CreatedAt,
                    UpdatedAt = mi.UpdatedAt,
                    ExpiresAt = mi.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "按分類獲取市場商品失敗: {Category}", category);
                return Enumerable.Empty<MarketItemDto>();
            }
        }

        public async Task<IEnumerable<MarketItemDto>> GetUserMarketItemsAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var marketItems = await _playerMarketRepository.GetMarketItemsBySellerIdAsync(userId, page, pageSize);
                return marketItems.Select(mi => new MarketItemDto
                {
                    Id = mi.Id,
                    SellerId = mi.SellerId,
                    SellerName = mi.Seller?.Username ?? "未知賣家",
                    Title = mi.Title,
                    Description = mi.Description,
                    Price = mi.Price,
                    Category = mi.Category,
                    Condition = mi.Condition,
                    ImageUrl = mi.ImageUrl,
                    Status = mi.Status,
                    CreatedAt = mi.CreatedAt,
                    UpdatedAt = mi.UpdatedAt,
                    ExpiresAt = mi.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶市場商品失敗: {UserId}", userId);
                return Enumerable.Empty<MarketItemDto>();
            }
        }

        public async Task<MarketItemCreateResult> CreateMarketItemAsync(MarketItemCreateDto createDto)
        {
            try
            {
                var marketItem = new MarketItem
                {
                    SellerId = createDto.SellerId,
                    Title = createDto.Title,
                    Description = createDto.Description,
                    Price = createDto.Price,
                    Category = createDto.Category,
                    Condition = createDto.Condition,
                    ImageUrl = createDto.ImageUrl,
                    Status = MarketItemStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(30) // 30天後過期
                };

                _playerMarketRepository.Add(marketItem);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("市場商品創建成功: {MarketItemId}, 賣家: {SellerId}", marketItem.Id, marketItem.SellerId);

                return new MarketItemCreateResult
                {
                    Success = true,
                    Message = "市場商品創建成功",
                    MarketItemId = marketItem.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建市場商品失敗: 賣家 {SellerId}", createDto.SellerId);
                return new MarketItemCreateResult
                {
                    Success = false,
                    Message = "創建失敗"
                };
            }
        }

        public async Task<MarketItemUpdateResult> UpdateMarketItemAsync(int marketItemId, int sellerId, MarketItemUpdateDto updateDto)
        {
            try
            {
                var marketItem = await _playerMarketRepository.GetMarketItemByIdAsync(marketItemId);
                if (marketItem == null)
                {
                    return new MarketItemUpdateResult
                    {
                        Success = false,
                        Message = "市場商品不存在"
                    };
                }

                if (marketItem.SellerId != sellerId)
                {
                    return new MarketItemUpdateResult
                    {
                        Success = false,
                        Message = "無權限修改此商品"
                    };
                }

                if (marketItem.Status != MarketItemStatus.Active)
                {
                    return new MarketItemUpdateResult
                    {
                        Success = false,
                        Message = "只能修改活躍狀態的商品"
                    };
                }

                marketItem.Title = updateDto.Title;
                marketItem.Description = updateDto.Description;
                marketItem.Price = updateDto.Price;
                marketItem.Category = updateDto.Category;
                marketItem.Condition = updateDto.Condition;
                marketItem.ImageUrl = updateDto.ImageUrl;
                marketItem.UpdatedAt = DateTime.UtcNow;

                _playerMarketRepository.Update(marketItem);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("市場商品更新成功: {MarketItemId}", marketItemId);

                return new MarketItemUpdateResult
                {
                    Success = true,
                    Message = "市場商品更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新市場商品失敗: {MarketItemId}", marketItemId);
                return new MarketItemUpdateResult
                {
                    Success = false,
                    Message = "更新失敗"
                };
            }
        }

        public async Task<MarketItemUpdateResult> DeactivateMarketItemAsync(int marketItemId, int sellerId)
        {
            try
            {
                var marketItem = await _playerMarketRepository.GetMarketItemByIdAsync(marketItemId);
                if (marketItem == null)
                {
                    return new MarketItemUpdateResult
                    {
                        Success = false,
                        Message = "市場商品不存在"
                    };
                }

                if (marketItem.SellerId != sellerId)
                {
                    return new MarketItemUpdateResult
                    {
                        Success = false,
                        Message = "無權限停用此商品"
                    };
                }

                marketItem.Status = MarketItemStatus.Inactive;
                marketItem.UpdatedAt = DateTime.UtcNow;

                _playerMarketRepository.Update(marketItem);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("市場商品停用成功: {MarketItemId}", marketItemId);

                return new MarketItemUpdateResult
                {
                    Success = true,
                    Message = "市場商品已停用"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停用市場商品失敗: {MarketItemId}", marketItemId);
                return new MarketItemUpdateResult
                {
                    Success = false,
                    Message = "停用失敗"
                };
            }
        }

        public async Task<PurchaseResult> PurchaseMarketItemAsync(int marketItemId, int buyerId)
        {
            try
            {
                var marketItem = await _playerMarketRepository.GetMarketItemByIdAsync(marketItemId);
                if (marketItem == null)
                {
                    return new PurchaseResult
                    {
                        Success = false,
                        Message = "市場商品不存在"
                    };
                }

                if (marketItem.Status != MarketItemStatus.Active)
                {
                    return new PurchaseResult
                    {
                        Success = false,
                        Message = "商品已不可購買"
                    };
                }

                if (marketItem.SellerId == buyerId)
                {
                    return new PurchaseResult
                    {
                        Success = false,
                        Message = "不能購買自己的商品"
                    };
                }

                // 檢查買家是否有足夠的點數
                var buyer = await _userRepository.GetByIdAsync(buyerId);
                if (buyer == null || buyer.Points < marketItem.Price)
                {
                    return new PurchaseResult
                    {
                        Success = false,
                        Message = "點數不足"
                    };
                }

                // 檢查賣家是否存在
                var seller = await _userRepository.GetByIdAsync(marketItem.SellerId);
                if (seller == null)
                {
                    return new PurchaseResult
                    {
                        Success = false,
                        Message = "賣家不存在"
                    };
                }

                // 創建交易記錄
                var transaction = new MarketTransaction
                {
                    MarketItemId = marketItemId,
                    BuyerId = buyerId,
                    SellerId = marketItem.SellerId,
                    Amount = marketItem.Price,
                    Status = TransactionStatus.Completed,
                    CreatedAt = DateTime.UtcNow,
                    CompletedAt = DateTime.UtcNow
                };

                _marketTransactionRepository.Add(transaction);

                // 更新商品狀態
                marketItem.Status = MarketItemStatus.Sold;
                marketItem.UpdatedAt = DateTime.UtcNow;
                _playerMarketRepository.Update(marketItem);

                // 轉移點數
                buyer.Points -= marketItem.Price;
                seller.Points += marketItem.Price;
                _userRepository.Update(buyer);
                _userRepository.Update(seller);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("市場商品購買成功: {MarketItemId}, 買家: {BuyerId}, 賣家: {SellerId}", 
                    marketItemId, buyerId, marketItem.SellerId);

                return new PurchaseResult
                {
                    Success = true,
                    Message = "購買成功",
                    TransactionId = transaction.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "購買市場商品失敗: {MarketItemId}, 買家: {BuyerId}", marketItemId, buyerId);
                return new PurchaseResult
                {
                    Success = false,
                    Message = "購買失敗"
                };
            }
        }

        public async Task<IEnumerable<TransactionDto>> GetUserTransactionsAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var transactions = await _marketTransactionRepository.GetByUserIdAsync(userId, page, pageSize);
                return transactions.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    MarketItemId = t.MarketItemId,
                    MarketItemTitle = t.MarketItem?.Title ?? "未知商品",
                    BuyerId = t.BuyerId,
                    BuyerName = t.Buyer?.Username ?? "未知買家",
                    SellerId = t.SellerId,
                    SellerName = t.Seller?.Username ?? "未知賣家",
                    Amount = t.Amount,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    CompletedAt = t.CompletedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶交易記錄失敗: {UserId}", userId);
                return Enumerable.Empty<TransactionDto>();
            }
        }

        public async Task<ReviewCreateResult> CreateReviewAsync(ReviewCreateDto createDto)
        {
            try
            {
                // 檢查是否已經評價過
                var existingReview = await _marketReviewRepository.GetByTransactionIdAsync(createDto.TransactionId);
                if (existingReview != null)
                {
                    return new ReviewCreateResult
                    {
                        Success = false,
                        Message = "此交易已評價過"
                    };
                }

                var review = new MarketReview
                {
                    TransactionId = createDto.TransactionId,
                    ReviewerId = createDto.ReviewerId,
                    RevieweeId = createDto.RevieweeId,
                    Rating = createDto.Rating,
                    Comment = createDto.Comment,
                    CreatedAt = DateTime.UtcNow
                };

                _marketReviewRepository.Add(review);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("市場評價創建成功: {ReviewId}, 評價者: {ReviewerId}", review.Id, review.ReviewerId);

                return new ReviewCreateResult
                {
                    Success = true,
                    Message = "評價創建成功",
                    ReviewId = review.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建市場評價失敗: 評價者 {ReviewerId}", createDto.ReviewerId);
                return new ReviewCreateResult
                {
                    Success = false,
                    Message = "評價創建失敗"
                };
            }
        }

        public async Task<IEnumerable<ReviewDto>> GetUserReviewsAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var reviews = await _marketReviewRepository.GetByRevieweeIdAsync(userId, page, pageSize);
                return reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    TransactionId = r.TransactionId,
                    ReviewerId = r.ReviewerId,
                    ReviewerName = r.Reviewer?.Username ?? "未知用戶",
                    RevieweeId = r.RevieweeId,
                    RevieweeName = r.Reviewee?.Username ?? "未知用戶",
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶評價失敗: {UserId}", userId);
                return Enumerable.Empty<ReviewDto>();
            }
        }

        public async Task<decimal> GetUserAverageRatingAsync(int userId)
        {
            try
            {
                return await _marketReviewRepository.GetAverageRatingByRevieweeIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶平均評分失敗: {UserId}", userId);
                return 0;
            }
        }

        public async Task<bool> IsItemExpiredAsync(int marketItemId)
        {
            try
            {
                var marketItem = await _playerMarketRepository.GetMarketItemByIdAsync(marketItemId);
                if (marketItem == null) return true;

                return marketItem.ExpiresAt.HasValue && marketItem.ExpiresAt.Value < DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查商品過期狀態失敗: {MarketItemId}", marketItemId);
                return true;
            }
        }

        public async Task<bool> ExtendItemExpirationAsync(int marketItemId, int sellerId, int additionalDays)
        {
            try
            {
                var marketItem = await _playerMarketRepository.GetMarketItemByIdAsync(marketItemId);
                if (marketItem == null) return false;

                if (marketItem.SellerId != sellerId)
                {
                    return false;
                }

                if (marketItem.Status != MarketItemStatus.Active)
                {
                    return false;
                }

                marketItem.ExpiresAt = marketItem.ExpiresAt?.AddDays(additionalDays) ?? DateTime.UtcNow.AddDays(additionalDays);
                marketItem.UpdatedAt = DateTime.UtcNow;

                _playerMarketRepository.Update(marketItem);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("市場商品過期時間延長成功: {MarketItemId}, 延長 {AdditionalDays} 天", 
                    marketItemId, additionalDays);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "延長市場商品過期時間失敗: {MarketItemId}", marketItemId);
                return false;
            }
        }

        public async Task<IEnumerable<Entities.PlayerMarketItem>> GetActiveItemsAsync(string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20)
        {
            try
            {
                return await _playerMarketRepository.GetActiveItemsAsync(category, minPrice, maxPrice, page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取活躍商品失敗");
                return Enumerable.Empty<Entities.PlayerMarketItem>();
            }
        }

        public async Task<IEnumerable<Entities.PlayerMarketItem>> SearchItemsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20)
        {
            try
            {
                return await _playerMarketRepository.SearchItemsAsync(keyword, category, minPrice, maxPrice, page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋商品失敗");
                return Enumerable.Empty<Entities.PlayerMarketItem>();
            }
        }

        public async Task<Entities.PlayerMarketItem?> GetItemAsync(int itemId)
        {
            try
            {
                return await _playerMarketRepository.GetByIdAsync(itemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品失敗: {ItemId}", itemId);
                return null;
            }
        }

        public async Task<ItemCreateResult> CreateItemAsync(int userId, ItemCreate itemCreate)
        {
            try
            {
                var item = new PlayerMarketItem
                {
                    SellerId = userId,
                    Title = itemCreate.Title,
                    Description = itemCreate.Description,
                    Price = itemCreate.Price,
                    Category = itemCreate.Category,
                    Condition = itemCreate.Condition,
                    ImageUrl = itemCreate.ImageUrl,
                    Status = MarketItemStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(30)
                };

                _playerMarketRepository.Add(item);
                await _unitOfWork.SaveChangesAsync();

                return new ItemCreateResult
                {
                    Success = true,
                    Message = "商品創建成功",
                    ItemId = item.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建商品失敗: {UserId}", userId);
                return new ItemCreateResult
                {
                    Success = false,
                    Message = "商品創建失敗"
                };
            }
        }

        public async Task<bool> UpdateItemAsync(int itemId, int userId, ItemUpdate itemUpdate)
        {
            try
            {
                var item = await _playerMarketRepository.GetByIdAsync(itemId);
                if (item == null || item.SellerId != userId) return false;

                item.Title = itemUpdate.Title;
                item.Description = itemUpdate.Description;
                item.Price = itemUpdate.Price;
                item.Category = itemUpdate.Category;
                item.Condition = itemUpdate.Condition;
                item.ImageUrl = itemUpdate.ImageUrl;
                item.UpdatedAt = DateTime.UtcNow;

                _playerMarketRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新商品失敗: {ItemId}", itemId);
                return false;
            }
        }

        public async Task<bool> DeactivateItemAsync(int itemId, int userId)
        {
            try
            {
                var item = await _playerMarketRepository.GetByIdAsync(itemId);
                if (item == null || item.SellerId != userId) return false;

                item.Status = MarketItemStatus.Inactive;
                item.UpdatedAt = DateTime.UtcNow;

                _playerMarketRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "下架商品失敗: {ItemId}", itemId);
                return false;
            }
        }

        public async Task<IEnumerable<Entities.PlayerMarketItem>> GetItemsByUserAsync(int userId, int page, int pageSize)
        {
            try
            {
                return await _playerMarketRepository.GetBySellerIdAsync(userId, page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶商品失敗: {UserId}", userId);
                return Enumerable.Empty<Entities.PlayerMarketItem>();
            }
        }

        public async Task<IEnumerable<string>> GetItemCategoriesAsync()
        {
            try
            {
                return await _playerMarketRepository.GetCategoriesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品分類失敗");
                return Enumerable.Empty<string>();
            }
        }

        public async Task<bool> PurchaseItemAsync(int buyerId, int itemId)
        {
            try
            {
                return await _playerMarketRepository.PurchaseItemAsync(buyerId, itemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "購買商品失敗: {BuyerId}, {ItemId}", buyerId, itemId);
                return false;
            }
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserAsync(int userId, int page, int pageSize)
        {
            try
            {
                var transactions = await _playerMarketRepository.GetTransactionsByUserAsync(userId, page, pageSize);
                return transactions.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    ItemId = t.ItemId,
                    SellerId = t.SellerId,
                    BuyerId = t.BuyerId,
                    Price = t.Price,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    CompletedAt = t.CompletedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶交易失敗: {UserId}", userId);
                return Enumerable.Empty<TransactionDto>();
            }
        }

        public async Task<bool> ConfirmTransactionAsync(int userId, int transactionId)
        {
            try
            {
                return await _playerMarketRepository.ConfirmTransactionAsync(userId, transactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "確認交易失敗: {UserId}, {TransactionId}", userId, transactionId);
                return false;
            }
        }

        public async Task<bool> CancelTransactionAsync(int userId, int transactionId)
        {
            try
            {
                return await _playerMarketRepository.CancelTransactionAsync(userId, transactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取消交易失敗: {UserId}, {TransactionId}", userId, transactionId);
                return false;
            }
        }

        public async Task<bool> ReviewTransactionAsync(int userId, int transactionId, TransactionReview review)
        {
            try
            {
                return await _playerMarketRepository.ReviewTransactionAsync(userId, transactionId, review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "評價交易失敗: {UserId}, {TransactionId}", userId, transactionId);
                return false;
            }
        }
    }
}