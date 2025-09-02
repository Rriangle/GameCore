using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.Types
{
    /// <summary>
    /// 錢包餘額
    /// </summary>
    public class WalletBalance
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 主錢包餘額
        /// </summary>
        public decimal MainBalance { get; set; }

        /// <summary>
        /// 銷售錢包餘額
        /// </summary>
        public decimal SalesBalance { get; set; }

        /// <summary>
        /// 凍結餘額
        /// </summary>
        public decimal FrozenBalance { get; set; }

        /// <summary>
        /// 可用餘額
        /// </summary>
        public decimal AvailableBalance => MainBalance - FrozenBalance;

        /// <summary>
        /// 總餘額
        /// </summary>
        public decimal TotalBalance => MainBalance + SalesBalance;

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 貨幣類型
        /// </summary>
        public string Currency { get; set; } = "TWD";
    }

    /// <summary>
    /// 錢包餘額回應
    /// </summary>
    public class WalletBalanceResponse
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "TWD";
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 點數交易記錄
    /// </summary>
    public class PointTransaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public decimal BalanceAfter { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
        public string Status { get; set; } = "Completed";
    }

    /// <summary>
    /// 點數交易查詢請求
    /// </summary>
    public class PointTransactionQueryRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TransactionType { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 點數交易查詢回應
    /// </summary>
    public class PointTransactionQueryResponse
    {
        public List<PointTransaction> Transactions { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 並發交易請求
    /// </summary>
    public class ConcurrentTransactionRequest
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        [Required]
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// 交易描述
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 請求時間戳
        /// </summary>
        public DateTime RequestTimestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// 並發結果
    /// </summary>
    public class ConcurrencyResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 結果訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 交易編號
        /// </summary>
        public int? TransactionId { get; set; }

        /// <summary>
        /// 衝突類型
        /// </summary>
        public string ConflictType { get; set; } = string.Empty;

        /// <summary>
        /// 建議重試時間
        /// </summary>
        public DateTime? RetryAfter { get; set; }
    }

    /// <summary>
    /// 交易分析請求
    /// </summary>
    public class TransactionAnalysisRequest
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 分析開始時間
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 分析結束時間
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 分析類型
        /// </summary>
        public string AnalysisType { get; set; } = "General";

        /// <summary>
        /// 最小金額閾值
        /// </summary>
        public decimal? MinAmount { get; set; }

        /// <summary>
        /// 最大金額閾值
        /// </summary>
        public decimal? MaxAmount { get; set; }
    }

    /// <summary>
    /// 可疑交易
    /// </summary>
    public class SuspiciousTransaction
    {
        /// <summary>
        /// 交易編號
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// 可疑原因
        /// </summary>
        public string SuspiciousReason { get; set; } = string.Empty;

        /// <summary>
        /// 風險分數
        /// </summary>
        public int RiskScore { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 是否已處理
        /// </summary>
        public bool IsProcessed { get; set; }
    }

    /// <summary>
    /// 風險分數
    /// </summary>
    public class RiskScore
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 風險分數
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 風險等級
        /// </summary>
        public string RiskLevel { get; set; } = string.Empty;

        /// <summary>
        /// 風險因素
        /// </summary>
        public List<string> RiskFactors { get; set; } = new List<string>();

        /// <summary>
        /// 計算時間
        /// </summary>
        public DateTime CalculatedAt { get; set; }

        /// <summary>
        /// 建議措施
        /// </summary>
        public List<string> Recommendations { get; set; } = new List<string>();
    }

    /// <summary>
    /// 託管請求
    /// </summary>
    public class EscrowRequest
    {
        /// <summary>
        /// 買家編號
        /// </summary>
        [Required]
        public int BuyerId { get; set; }

        /// <summary>
        /// 賣家編號
        /// </summary>
        [Required]
        public int SellerId { get; set; }

        /// <summary>
        /// 託管金額
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 託管條件
        /// </summary>
        [Required]
        public string Condition { get; set; } = string.Empty;

        /// <summary>
        /// 託管期限
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 託管描述
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// 託管結果
    /// </summary>
    public class EscrowResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 結果訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 託管編號
        /// </summary>
        public int? EscrowId { get; set; }

        /// <summary>
        /// 託管狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 託管金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 託管交易
    /// </summary>
    public class EscrowTransaction
    {
        /// <summary>
        /// 託管編號
        /// </summary>
        public int EscrowId { get; set; }

        /// <summary>
        /// 買家編號
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 賣家編號
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 託管金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 託管狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 託管條件
        /// </summary>
        public string Condition { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }

    /// <summary>
    /// 託管狀態
    /// </summary>
    public class EscrowStatus
    {
        /// <summary>
        /// 託管編號
        /// </summary>
        public int EscrowId { get; set; }

        /// <summary>
        /// 託管狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 狀態描述
        /// </summary>
        public string StatusDescription { get; set; } = string.Empty;

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 下一步動作
        /// </summary>
        public string NextAction { get; set; } = string.Empty;
    }

    /// <summary>
    /// 餘額對帳結果
    /// </summary>
    public class BalanceReconciliationResult
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 是否一致
        /// </summary>
        public bool IsConsistent { get; set; }

        /// <summary>
        /// 差異金額
        /// </summary>
        public decimal Discrepancy { get; set; }

        /// <summary>
        /// 差異原因
        /// </summary>
        public string DiscrepancyReason { get; set; } = string.Empty;

        /// <summary>
        /// 對帳時間
        /// </summary>
        public DateTime ReconciledAt { get; set; }

        /// <summary>
        /// 建議措施
        /// </summary>
        public List<string> Recommendations { get; set; } = new List<string>();
    }

    /// <summary>
    /// 錢包審計追蹤
    /// </summary>
    public class WalletAuditTrail
    {
        /// <summary>
        /// 審計編號
        /// </summary>
        public int AuditId { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 操作類型
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// 操作描述
        /// </summary>
        public string OperationDescription { get; set; } = string.Empty;

        /// <summary>
        /// 操作前餘額
        /// </summary>
        public decimal BalanceBefore { get; set; }

        /// <summary>
        /// 操作後餘額
        /// </summary>
        public decimal BalanceAfter { get; set; }

        /// <summary>
        /// 操作時間
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Operator { get; set; } = string.Empty;
    }

    /// <summary>
    /// 餘額快照
    /// </summary>
    public class BalanceSnapshot
    {
        /// <summary>
        /// 快照編號
        /// </summary>
        public int SnapshotId { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 可用餘額
        /// </summary>
        public decimal AvailableBalance { get; set; }

        /// <summary>
        /// 凍結餘額
        /// </summary>
        public decimal FrozenBalance { get; set; }

        /// <summary>
        /// 總餘額
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// 快照時間
        /// </summary>
        public DateTime SnapshotTime { get; set; }

        /// <summary>
        /// 快照原因
        /// </summary>
        public string SnapshotReason { get; set; } = string.Empty;
    }

    /// <summary>
    /// 錢包安全報告
    /// </summary>
    public class WalletSecurityReport
    {
        /// <summary>
        /// 報告編號
        /// </summary>
        public int ReportId { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 安全分數
        /// </summary>
        public int SecurityScore { get; set; }

        /// <summary>
        /// 安全等級
        /// </summary>
        public string SecurityLevel { get; set; } = string.Empty;

        /// <summary>
        /// 安全風險
        /// </summary>
        public List<string> SecurityRisks { get; set; } = new List<string>();

        /// <summary>
        /// 建議措施
        /// </summary>
        public List<string> Recommendations { get; set; } = new List<string>();

        /// <summary>
        /// 報告時間
        /// </summary>
        public DateTime GeneratedAt { get; set; }
    }

    /// <summary>
    /// 安全警報
    /// </summary>
    public class SecurityAlert
    {
        /// <summary>
        /// 警報編號
        /// </summary>
        public int AlertId { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 警報類型
        /// </summary>
        public string AlertType { get; set; } = string.Empty;

        /// <summary>
        /// 警報等級
        /// </summary>
        public string AlertLevel { get; set; } = string.Empty;

        /// <summary>
        /// 警報描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 是否已處理
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// 警報時間
        /// </summary>
        public DateTime AlertTime { get; set; }

        /// <summary>
        /// 處理時間
        /// </summary>
        public DateTime? ProcessedAt { get; set; }
    }

    /// <summary>
    /// 管理員點數調整請求
    /// </summary>
    public class AdminPointAdjustmentRequest
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 調整金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 調整類型
        /// </summary>
        public string AdjustmentType { get; set; } = string.Empty;

        /// <summary>
        /// 調整原因
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 管理員ID
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 是否通知用戶
        /// </summary>
        public bool NotifyUser { get; set; } = true;

        /// <summary>
        /// 備註
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 管理員點數調整回應
    /// </summary>
    public class AdminPointAdjustmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public decimal NewBalance { get; set; }
        public int TransactionId { get; set; }
        public DateTime AdjustmentDate { get; set; }
    }

    /// <summary>
    /// 銷售錢包資訊
    /// </summary>
    public class SalesWalletInfo
    {
        public int UserId { get; set; }
        public decimal SalesBalance { get; set; }
        public bool HasSalesPermission { get; set; }
        public DateTime? PermissionGrantedDate { get; set; }
        public string PermissionStatus { get; set; } = "Pending";
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
    }

    /// <summary>
    /// 銷售權限請求
    /// </summary>
    public class SalesPermissionRequest
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankCode { get; set; } = string.Empty;

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankAccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶持有人姓名
        /// </summary>
        public byte[]? AccountCoverPhoto { get; set; }

        /// <summary>
        /// 申請原因
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售權限回應
    /// </summary>
    public class SalesPermissionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int? ApprovedBy { get; set; }
    }

    /// <summary>
    /// 監控等級
    /// </summary>
    public enum MonitoringLevel
    {
        None = 0,
        Basic = 1,
        Enhanced = 2,
        Maximum = 3
    }
} 
