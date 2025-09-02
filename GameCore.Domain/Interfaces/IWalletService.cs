using GameCore.Core.DTOs;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ���]�A�Ȥ���
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// ���o�Τ���]�l�B
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>���]�l�B��T</returns>
        Task<WalletBalanceResponse> GetWalletBalanceAsync(int userId);

        /// <summary>
        /// �d���I�Ƭy���O��
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <param name="request">�d�߱���</param>
        /// <returns>�I�Ƭy���O��</returns>
        Task<PointTransactionQueryResponse> GetPointTransactionsAsync(int userId, PointTransactionQueryRequest request);

        /// <summary>
        /// �վ�Τ��I�ơ]�޲z���\��^
        /// </summary>
        /// <param name="request">�վ�ШD</param>
        /// <returns>�վ㵲�G</returns>
        Task<AdminPointAdjustmentResponse> AdjustUserPointsAsync(AdminPointAdjustmentRequest request);

        /// <summary>
        /// ���o�P����]��T
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�P����]��T</returns>
        Task<SalesWalletInfo> GetSalesWalletInfoAsync(int userId);

        /// <summary>
        /// �ӽоP���v��
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <param name="request">�ӽи�T</param>
        /// <returns>�ӽе��G</returns>
        Task<SalesPermissionResponse> ApplySalesPermissionAsync(int userId, SalesPermissionRequest request);

        /// <summary>
        /// ���o�P���v���ӽЪ��A
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�ӽЪ��A</returns>
        Task<SalesPermissionResponse?> GetSalesPermissionStatusAsync(int userId);

        /// <summary>
        /// �ˬd�Τ�O�_���P���v��
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�O�_���P���v��</returns>
        Task<bool> HasSalesAuthorityAsync(int userId);

        /// <summary>
        /// �ಾ�I�ƨ�P����]
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <param name="amount">�ಾ���B</param>
        /// <returns>�O�_���\</returns>
        Task<bool> TransferToSalesWalletAsync(int userId, int amount);

        /// <summary>
        /// �q�P����]�����I��
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <param name="amount">������B</param>
        /// <returns>�O�_���\</returns>
        Task<bool> WithdrawFromSalesWalletAsync(int userId, int amount);
    }
} 
