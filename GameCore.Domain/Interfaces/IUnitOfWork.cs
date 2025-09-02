using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �u�@�椸����
    /// �Τ@�޲z�Ҧ� Repository�A�T�O��Ƥ@�P��
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Repository �ݩ�
        /// <summary>
        /// �ϥΪ� Repository
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// �d�� Repository
        /// </summary>
        IPetRepository PetRepository { get; }

        /// <summary>
        /// ñ�� Repository
        /// </summary>
        ISignInRepository SignInRepository { get; }

        /// <summary>
        /// �p�C�� Repository
        /// </summary>
        IMiniGameRepository MiniGameRepository { get; }

        /// <summary>
        /// �C�� Repository
        /// </summary>
        IGameRepository GameRepository { get; }

        /// <summary>
        /// �׾� Repository
        /// </summary>
        IForumRepository ForumRepository { get; }

        /// <summary>
        /// �ӫ� Repository
        /// </summary>
        IStoreRepository StoreRepository { get; }

        /// <summary>
        /// ���a���� Repository
        /// </summary>
        IPlayerMarketRepository PlayerMarketRepository { get; }

        /// <summary>
        /// �q�� Repository
        /// </summary>
        INotificationRepository NotificationRepository { get; }

        /// <summary>
        /// ��� Repository
        /// </summary>
        IChatRepository ChatRepository { get; }

        /// <summary>
        /// �޲z�� Repository
        /// </summary>
        IManagerRepository ManagerRepository { get; }

        /// <summary>
        /// ��ѰT�� Repository
        /// </summary>
        IChatMessageRepository ChatMessageRepository { get; }

        /// <summary>
        /// �p�H��� Repository
        /// </summary>
        IPrivateChatRepository PrivateChatRepository { get; }

        /// <summary>
        /// �p�H�T�� Repository
        /// </summary>
        IPrivateMessageRepository PrivateMessageRepository { get; }

        /// <summary>
        /// �^�� Repository
        /// </summary>
        IReplyRepository ReplyRepository { get; }

        /// <summary>
        /// �|���P���� Repository
        /// </summary>
        IMemberSalesProfileRepository MemberSalesProfileRepository { get; }

        /// <summary>
        /// �Τ�P���T Repository
        /// </summary>
        IUserSalesInformationRepository UserSalesInformationRepository { get; }

        /// <summary>
        /// �Τ���] Repository
        /// </summary>
        IUserWalletRepository UserWalletRepository { get; }

        /// <summary>
        /// �Τ��v�� Repository
        /// </summary>
        IUserRightsRepository UserRightsRepository { get; }

        /// <summary>
        /// ���a�����q�� Repository
        /// </summary>
        IPlayerMarketOrderRepository PlayerMarketOrderRepository { get; }

        /// <summary>
        /// ������� Repository
        /// </summary>
        IMarketTransactionRepository MarketTransactionRepository { get; }

        /// <summary>
        /// �������� Repository
        /// </summary>
        IMarketReviewRepository MarketReviewRepository { get; }

        /// <summary>
        /// �޲z�̨����v�� Repository
        /// </summary>
        IManagerRolePermissionRepository ManagerRolePermissionRepository { get; }

        /// <summary>
        /// ���~ Repository
        /// </summary>
        IProductRepository ProductRepository { get; }

        /// <summary>
        /// �q�� Repository
        /// </summary>
        IOrderRepository OrderRepository { get; }

        /// <summary>
        /// �ʪ��� Repository
        /// </summary>
        ICartRepository CartRepository { get; }

        /// <summary>
        /// �C���]�w Repository
        /// </summary>
        IGameSettingsRepository GameSettingsRepository { get; }

        /// <summary>
        /// �޲z�̸�� Repository
        /// </summary>
        IManagerDataRepository ManagerDataRepository { get; }

        /// <summary>
        /// �����ӫ~ Repository
        /// </summary>
        IMarketItemRepository MarketItemRepository { get; }
        #endregion

        /// <summary>
        /// �x�s�Ҧ��ܧ�
        /// </summary>
        /// <returns>���v�T���O����</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// �}�l��Ʈw���
        /// </summary>
        /// <returns>�������</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// ������
        /// </summary>
        /// <returns>Task</returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// �^�u���
        /// </summary>
        /// <returns>Task</returns>
        Task RollbackTransactionAsync();
    }
}
