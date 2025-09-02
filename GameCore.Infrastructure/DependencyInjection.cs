using GameCore.Application.Services;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Repositories;
using GameCore.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace GameCore.Infrastructure
{
    /// <summary>
    /// 依賴注入擴展
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// 註冊基礎設施服務
        /// </summary>
        /// <param name="services">服務集合</param>
        /// <returns>服務集合</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // 註冊 Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<ISignInRepository, SignInRepository>();
            services.AddScoped<IMiniGameRepository, MiniGameRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostReplyRepository, PostReplyRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IPlayerMarketRepository, PlayerMarketRepository>();
            services.AddScoped<IMarketTransactionRepository, MarketTransactionRepository>();
            services.AddScoped<IMarketReviewRepository, MarketReviewRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationSourceRepository, NotificationSourceRepository>();
            services.AddScoped<INotificationActionRepository, NotificationActionRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IPrivateChatRepository, PrivateChatRepository>();
            services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IManagerRolePermissionRepository, ManagerRolePermissionRepository>();
            services.AddScoped<IReplyRepository, ReplyRepository>();
            services.AddScoped<IMemberSalesProfileRepository, MemberSalesProfileRepository>();
            services.AddScoped<IUserSalesInformationRepository, UserSalesInformationRepository>();
            services.AddScoped<IUserWalletRepository, UserWalletRepository>();
            services.AddScoped<IUserRightsRepository, UserRightsRepository>();
            services.AddScoped<IPlayerMarketOrderRepository, PlayerMarketOrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IGameSettingsRepository, GameSettingsRepository>();
            services.AddScoped<IManagerDataRepository, ManagerDataRepository>();
            services.AddScoped<IMarketItemRepository, MarketItemRepository>();
            
            // 註冊 UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
} 