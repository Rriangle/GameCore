using GameCore.Core.DTOs;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �P��A�Ȥ���
    /// </summary>
    public interface ISalesService
    {
        /// <summary>
        /// ���o�Τ�P��έp
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�P��έp��T</returns>
        Task<SalesStatisticsResponse> GetSalesStatisticsAsync(int userId);

        /// <summary>
        /// ���o�P���v���ӽЦC��]�޲z���\��^
        /// </summary>
        /// <param name="request">�d�߱���</param>
        /// <returns>�ӽЦC��</returns>
        Task<SalesPermissionListResponse> GetSalesPermissionApplicationsAsync(SalesPermissionListRequest request);

        /// <summary>
        /// �f�־P���v���ӽС]�޲z���\��^
        /// </summary>
        /// <param name="request">�f�ֽШD</param>
        /// <returns>�f�ֵ��G</returns>
        Task<SalesPermissionReviewResponse> ReviewSalesPermissionAsync(SalesPermissionReviewRequest request);

        /// <summary>
        /// ���o�P��Ʀ�]
        /// </summary>
        /// <param name="period">���������]daily/weekly/monthly�^</param>
        /// <param name="limit">��ܼƶq</param>
        /// <returns>�P��Ʀ�]</returns>
        Task<List<SalesRankingItem>> GetSalesRankingAsync(string period, int limit = 10);

        /// <summary>
        /// ���o�P�����
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <param name="startDate">�}�l���</param>
        /// <param name="endDate">�������</param>
        /// <returns>�P�����</returns>
        Task<SalesReportResponse> GetSalesReportAsync(int userId, DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// �P��έp�^��
    /// </summary>
    public class SalesStatisticsResponse
    {
        /// <summary>
        /// �Τ�ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// ����P���B
        /// </summary>
        public decimal MonthlySales { get; set; }

        /// <summary>
        /// ����q���
        /// </summary>
        public int MonthlyOrders { get; set; }

        /// <summary>
        /// �֭p�P���B
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// �֭p�q���
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// �����q����B
        /// </summary>
        public decimal AverageOrderValue { get; set; }

        /// <summary>
        /// �P���v�����A
        /// </summary>
        public string PermissionStatus { get; set; } = string.Empty;
    }

    /// <summary>
    /// �P���v���ӽЦC��ШD
    /// </summary>
    public class SalesPermissionListRequest
    {
        /// <summary>
        /// ���A�z��
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// �}�l���
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// ���X
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// �C������
        /// </summary>
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// �P���v���ӽЦC��^��
    /// </summary>
    public class SalesPermissionListResponse
    {
        /// <summary>
        /// �ӽЦC��
        /// </summary>
        public List<SalesPermissionApplicationItem> Applications { get; set; } = new();

        /// <summary>
        /// �`����
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// �`����
        /// </summary>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// �P���v���ӽж���
    /// </summary>
    public class SalesPermissionApplicationItem
    {
        /// <summary>
        /// �ӽ�ID
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// �Τ�ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// �Τ�W��
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// �ӽЮɶ�
        /// </summary>
        public DateTime ApplicationTime { get; set; }

        /// <summary>
        /// �ӽЪ��A
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// �Ȧ�N��
        /// </summary>
        public int BankCode { get; set; }

        /// <summary>
        /// �Ȧ�b���]�������á^
        /// </summary>
        public string MaskedBankAccount { get; set; } = string.Empty;

        /// <summary>
        /// �ӽл���
        /// </summary>
        public string? ApplicationNote { get; set; }
    }

    /// <summary>
    /// �P���v���f�ֽШD
    /// </summary>
    public class SalesPermissionReviewRequest
    {
        /// <summary>
        /// �ӽ�ID
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// �f�ֵ��G�]approved/rejected�^
        /// </summary>
        public string ReviewResult { get; set; } = string.Empty;

        /// <summary>
        /// �f�ֻ���
        /// </summary>
        public string ReviewNote { get; set; } = string.Empty;

        /// <summary>
        /// �޲z��ID
        /// </summary>
        public int ManagerId { get; set; }
    }

    /// <summary>
    /// �P���v���f�֦^��
    /// </summary>
    public class SalesPermissionReviewResponse
    {
        /// <summary>
        /// �f�֬O�_���\
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// �f�֮ɶ�
        /// </summary>
        public DateTime ReviewTime { get; set; }

        /// <summary>
        /// �q��ID�]�Y���\�o�e�q���^
        /// </summary>
        public int? NotificationId { get; set; }
    }

    /// <summary>
    /// �P��Ʀ�]����
    /// </summary>
    public class SalesRankingItem
    {
        /// <summary>
        /// �ƦW
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// �Τ�ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// �Τ�W��
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// �P���B
        /// </summary>
        public decimal SalesAmount { get; set; }

        /// <summary>
        /// �q���
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// �P�W���ƦW�ܤ�
        /// </summary>
        public int RankChange { get; set; }
    }

    /// <summary>
    /// �P�����^��
    /// </summary>
    public class SalesReportResponse
    {
        /// <summary>
        /// �Τ�ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// �`�P���B
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// �`�q���
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// �����q����B
        /// </summary>
        public decimal AverageOrderValue { get; set; }

        /// <summary>
        /// �̰��浧�P��
        /// </summary>
        public decimal HighestOrderValue { get; set; }

        /// <summary>
        /// �̧C�浧�P��
        /// </summary>
        public decimal LowestOrderValue { get; set; }

        /// <summary>
        /// �C��P��ƾ�
        /// </summary>
        public List<DailySalesData> DailyData { get; set; } = new();
    }

    /// <summary>
    /// �C��P��ƾ�
    /// </summary>
    public class DailySalesData
    {
        /// <summary>
        /// ���
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// �P���B
        /// </summary>
        public decimal SalesAmount { get; set; }

        /// <summary>
        /// �q���
        /// </summary>
        public int OrderCount { get; set; }
    }
} 
