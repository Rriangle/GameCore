using GameCore.Core.Entities;
using GameCore.Core.Services.Enhanced;

namespace GameCore.Core.Services.Enhanced
{
    public interface IAdvancedAuthService : IAuthService
    {
        // Enhanced Authentication with Security Features
        Task<AuthenticationResult> AuthenticateWithSecurityAsync(string identifier, string password, AuthenticationContext context);
        Task<bool> ValidatePasswordStrengthAsync(string password);
        Task<bool> CheckPasswordLeakAsync(string password);
        Task<TokenRefreshResult> RefreshTokenAsync(string refreshToken, string userAgent);
        
        // Account Security & Lockout Management
        Task<LockResult> LockAccountAsync(int userId, LockReason reason, TimeSpan? duration = null);
        Task<bool> UnlockAccountAsync(int userId, string unlockReason);
        Task<bool> IsAccountLockedAsync(int userId);
        Task<IEnumerable<LoginAttempt>> GetRecentLoginAttemptsAsync(int userId, TimeSpan timeWindow);
        
        // Audit & Logging
        Task LogAuthenticationEventAsync(AuthenticationEvent authEvent);
        Task<IEnumerable<AuthenticationEvent>> GetAuthenticationHistoryAsync(int userId, TimeSpan period);
        Task<SecurityReport> GenerateSecurityReportAsync(int userId);
        
        // Rate Limiting & Abuse Prevention
        Task<RateLimitResult> CheckRateLimitAsync(string identifier, string action);
        Task<bool> ResetRateLimitAsync(string identifier, string action);
        Task<IEnumerable<string>> GetBlockedIPsAsync();
        Task<bool> BlockIPAsync(string ipAddress, TimeSpan duration);
        
        // Multi-Factor Authentication (MFA) Preparation
        Task<MfaSetupResult> SetupMfaAsync(int userId, MfaMethod method);
        Task<bool> ValidateMfaTokenAsync(int userId, string token);
        Task<IEnumerable<MfaMethod>> GetAvailableMfaMethodsAsync(int userId);
        Task<bool> DisableMfaAsync(int userId, string confirmationCode);
    }

    // Enhanced Authentication Models
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public User? User { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool RequiresMfa { get; set; }
        public SecurityWarning? SecurityWarning { get; set; }
        public AuthenticationMetadata Metadata { get; set; } = new();
    }

    public class AuthenticationContext
    {
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string? DeviceFingerprint { get; set; }
        public bool IsTrustedDevice { get; set; }
        public string? GeolocationInfo { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Dictionary<string, string> AdditionalContext { get; set; } = new();
    }

    public class AuthenticationMetadata
    {
        public bool IsNewDevice { get; set; }
        public bool IsNewLocation { get; set; }
        public TimeSpan TimeSinceLastLogin { get; set; }
        public int RecentFailedAttempts { get; set; }
        public SecurityLevel SecurityLevel { get; set; }
    }

    public class SecurityWarning
    {
        public WarningType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public SecurityLevel Severity { get; set; }
        public string RecommendedAction { get; set; } = string.Empty;
    }

    public class TokenRefreshResult
    {
        public bool Success { get; set; }
        public string? NewAccessToken { get; set; }
        public string? NewRefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class LockResult
    {
        public bool Success { get; set; }
        public LockReason Reason { get; set; }
        public DateTime LockedAt { get; set; }
        public DateTime? UnlockAt { get; set; }
        public string? Message { get; set; }
    }

    public class LoginAttempt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string? FailureReason { get; set; }
        public DateTime AttemptedAt { get; set; }
        public string? GeolocationInfo { get; set; }
    }

    public enum SecurityLevel
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public enum LockReason
    {
        TooManyFailedAttempts = 1,
        SuspiciousActivity = 2,
        SecurityViolation = 3,
        AdminAction = 4,
        CompromisedAccount = 5
    }
} 