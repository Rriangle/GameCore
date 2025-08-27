using System;
using System.Collections.Generic;
using GameCore.Core.Entities;

namespace GameCore.Core.Services.Enhanced
{
    /// <summary>
    /// 認證事件
    /// </summary>
    public class AuthenticationEvent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool Success { get; set; }
        public string? FailureReason { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    /// <summary>
    /// 安全報告
    /// </summary>
    public class SecurityReport
    {
        public int UserId { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<SecurityIncident> Incidents { get; set; } = new();
        public SecurityScore OverallScore { get; set; }
        public List<SecurityRecommendation> Recommendations { get; set; } = new();
        public Dictionary<string, object> Statistics { get; set; } = new();
    }

    /// <summary>
    /// 安全事件
    /// </summary>
    public class SecurityIncident
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SecurityLevel Severity { get; set; }
        public DateTime OccurredAt { get; set; }
        public bool Resolved { get; set; }
        public string? Resolution { get; set; }
    }

    /// <summary>
    /// 安全評分
    /// </summary>
    public class SecurityScore
    {
        public int OverallScore { get; set; }
        public int PasswordScore { get; set; }
        public int MfaScore { get; set; }
        public int LoginPatternScore { get; set; }
        public int DeviceTrustScore { get; set; }
        public string Grade { get; set; } = string.Empty;
    }

    /// <summary>
    /// 安全建議
    /// </summary>
    public class SecurityRecommendation
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SecurityLevel Priority { get; set; }
        public string Action { get; set; } = string.Empty;
        public bool Implemented { get; set; }
    }

    /// <summary>
    /// 速率限制結果
    /// </summary>
    public class RateLimitResult
    {
        public bool IsLimited { get; set; }
        public int RemainingAttempts { get; set; }
        public DateTime ResetTime { get; set; }
        public TimeSpan RetryAfter { get; set; }
        public string LimitType { get; set; } = string.Empty;
    }

    /// <summary>
    /// MFA 設定結果
    /// </summary>
    public class MfaSetupResult
    {
        public bool Success { get; set; }
        public string? QrCodeUrl { get; set; }
        public string? SecretKey { get; set; }
        public string? BackupCodes { get; set; }
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// MFA 方法
    /// </summary>
    public enum MfaMethod
    {
        AuthenticatorApp,
        SMS,
        Email,
        HardwareToken,
        Biometric
    }

    /// <summary>
    /// 警告類型
    /// </summary>
    public enum WarningType
    {
        NewDevice,
        NewLocation,
        UnusualTime,
        MultipleFailedAttempts,
        SuspiciousActivity
    }
} 