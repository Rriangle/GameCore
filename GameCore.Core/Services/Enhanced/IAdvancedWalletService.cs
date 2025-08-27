using GameCore.Core.Entities;

namespace GameCore.Core.Services.Enhanced
{
    public interface IAdvancedWalletService : IWalletService
    {
        // Atomic Transaction Management
        Task<WalletTransactionResult> ExecuteAtomicTransactionAsync(AtomicTransactionRequest request);
        Task<ConcurrencyResult> HandleConcurrentTransactionAsync(ConcurrentTransactionRequest request);
        Task<bool> ValidateTransactionIntegrityAsync(int transactionId);
        Task<WalletBalance> GetRealTimeBalanceAsync(int userId);
        Task<bool> ReserveBalanceAsync(int userId, decimal amount, string reservationId);
        Task<bool> ReleaseReservationAsync(string reservationId);
        
        // Fraud Detection & Prevention
        Task<FraudAnalysisResult> AnalyzeTransactionAsync(TransactionAnalysisRequest request);
        Task<bool> ValidateTransactionPatternAsync(int userId, decimal amount);
        Task<IEnumerable<SuspiciousTransaction>> GetSuspiciousTransactionsAsync(int userId, TimeSpan period);
        Task<RiskScore> CalculateUserRiskScoreAsync(int userId);
        Task<bool> BlockSuspiciousTransactionAsync(int transactionId, string reason);
        
        // Escrow & Secure Transactions
        Task<EscrowResult> CreateEscrowAsync(EscrowRequest request);
        Task<bool> ReleaseEscrowAsync(int escrowId, EscrowReleaseReason reason);
        Task<bool> RefundEscrowAsync(int escrowId, string refundReason);
        Task<IEnumerable<EscrowTransaction>> GetActiveEscrowsAsync(int userId);
        Task<EscrowStatus> GetEscrowStatusAsync(int escrowId);
        
        // Advanced Balance Management
        Task<bool> ValidateBalanceConsistencyAsync(int userId);
        Task<BalanceReconciliationResult> ReconcileBalanceAsync(int userId);
        Task<WalletAuditTrail> GetTransactionAuditTrailAsync(int transactionId);
        Task<IEnumerable<BalanceSnapshot>> GetBalanceHistoryAsync(int userId, TimeSpan period);
        
        // Security & Monitoring
        Task<WalletSecurityReport> GenerateSecurityReportAsync(int userId);
        Task<bool> EnableWalletMonitoringAsync(int userId, MonitoringLevel level);
        Task<IEnumerable<SecurityAlert>> GetSecurityAlertsAsync(int userId);
        Task<bool> ResetWalletSecurityAsync(int userId, string adminConfirmation);
    }

    // Advanced Wallet Models
    public class AtomicTransactionRequest
    {
        public List<TransactionStep> Steps { get; set; } = new();
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();
        public int InitiatorUserId { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
        public bool AllowPartialSuccess { get; set; } = false;
    }

    public class TransactionStep
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
    }

    public class WalletTransactionResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public List<TransactionStepResult> StepResults { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public DateTime ProcessedAt { get; set; }
        public TimeSpan ProcessingTime { get; set; }
    }

    public class FraudAnalysisResult
    {
        public double RiskScore { get; set; } // 0-1
        public FraudRiskLevel RiskLevel { get; set; }
        public List<FraudIndicator> Indicators { get; set; } = new();
        public bool RequiresManualReview { get; set; }
        public string RecommendedAction { get; set; } = string.Empty;
    }

    public class EscrowRequest
    {
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public TimeSpan AutoReleaseTimeout { get; set; }
        public List<EscrowCondition> ReleaseConditions { get; set; } = new();
    }

    public class TransactionStepResult
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public decimal ProcessedAmount { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime ProcessedAt { get; set; }
    }

    public class FraudIndicator
    {
        public string IndicatorType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Severity { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
    }

    public enum FraudRiskLevel
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public enum MonitoringLevel
    {
        Basic = 1,
        Enhanced = 2,
        Maximum = 3
    }
} 