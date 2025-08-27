using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace GameCore.Core.Services.Enhanced
{
    // AuthResult 已移至 IAuthService.cs

    /// <summary>
    /// MFA 配置結果
    /// </summary>
    public class MFAConfigResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? MFASecret { get; set; }
        public string? QRCodeUrl { get; set; }
    }

    /// <summary>
    /// 詐騙風險評估
    /// </summary>
    public class FraudRiskAssessment
    {
        public FraudRiskLevel RiskLevel { get; set; }
        public int RiskScore { get; set; }
        public List<string> RiskFactors { get; set; } = new List<string>();
        public DateTime AssessmentTime { get; set; }
    }





    /// <summary>
    /// 安全審計日誌
    /// </summary>
    public class SecurityAuditLog
    {
        public int UserId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string IPAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }

    /// <summary>
    /// 高級認證服務實作
    /// 提供多因素認證、詐騙防護、安全審計等企業級功能
    /// </summary>
    public class AdvancedAuthService : IAdvancedAuthService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AdvancedAuthService> _logger;
        private readonly Dictionary<string, int> _loginAttempts;
        private readonly Dictionary<string, DateTime> _lockoutTimes;
        private readonly Dictionary<string, string> _mfaCodes;

        public AdvancedAuthService(
            IUserRepository userRepository,
            ILogger<AdvancedAuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
            _loginAttempts = new Dictionary<string, int>();
            _lockoutTimes = new Dictionary<string, DateTime>();
            _mfaCodes = new Dictionary<string, string>();
        }

        /// <summary>
        /// 高級登入驗證
        /// </summary>
        public async Task<AuthResult> AdvancedLoginAsync(string username, string password, string mfaCode = null)
        {
            try
            {
                // 檢查帳號鎖定狀態
                if (IsAccountLocked(username))
                {
                    _logger.LogWarning("帳號 {Username} 已被鎖定", username);
                    return new AuthResult
                    {
                        Success = false,
                        Message = "帳號已被鎖定，請稍後再試",
                        RequiresMFA = false,
                        LockoutEndTime = GetLockoutEndTime(username)
                    };
                }

                // 驗證基本登入
                var user = await _userRepository.GetByUsernameAsync(username);
                if (user == null || !VerifyPassword(password, user.Password))
                {
                    IncrementLoginAttempts(username);
                    _logger.LogWarning("登入失敗: 使用者 {Username}", username);
                    return new AuthResult
                    {
                        Success = false,
                        Message = "使用者名稱或密碼錯誤",
                        RequiresMFA = false
                    };
                }

                // 檢查是否需要 MFA
                if (user.RequiresMFA && string.IsNullOrEmpty(mfaCode))
                {
                    var mfaCodeGenerated = GenerateMFACode(username);
                    _logger.LogInformation("為使用者 {Username} 生成 MFA 代碼", username);
                    return new AuthResult
                    {
                        Success = false,
                        Message = "需要多因素認證",
                        RequiresMFA = true,
                        UserId = user.UserId
                    };
                }

                // 驗證 MFA
                if (user.RequiresMFA && !VerifyMFACode(username, mfaCode))
                {
                    IncrementLoginAttempts(username);
                    _logger.LogWarning("MFA 驗證失敗: 使用者 {Username}", username);
                    return new AuthResult
                    {
                        Success = false,
                        Message = "多因素認證代碼錯誤",
                        RequiresMFA = true,
                        UserId = user.UserId
                    };
                }

                // 檢查詐騙風險
                var fraudRisk = await AssessFraudRiskAsync(user);
                if (fraudRisk.RiskLevel >= FraudRiskLevel.High)
                {
                    _logger.LogWarning("檢測到高風險登入: 使用者 {Username}, 風險等級: {RiskLevel}", username, fraudRisk.RiskLevel);
                    return new AuthResult
                    {
                        Success = false,
                        Message = "登入被阻擋，請聯繫客服",
                        RequiresMFA = false,
                        FraudRisk = fraudRisk
                    };
                }

                // 登入成功
                ResetLoginAttempts(username);
                await LogSecurityEventAsync(user.UserId, "LOGIN_SUCCESS", "登入成功");
                
                _logger.LogInformation("使用者 {Username} 登入成功", username);
                return new AuthResult
                {
                    Success = true,
                    Message = "登入成功",
                    UserId = user.UserId,
                    User = user,
                    FraudRisk = fraudRisk
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "高級登入過程中發生錯誤: {Username}", username);
                return new AuthResult
                {
                    Success = false,
                    Message = "登入過程中發生錯誤"
                };
            }
        }

        /// <summary>
        /// 基本登入實作 (IAuthService)
        /// </summary>
        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            return await AdvancedLoginAsync(username, password);
        }

        /// <summary>
        /// 基本註冊實作 (IAuthService)
        /// </summary>
        public async Task<AuthResult> RegisterAsync(string username, string email, string password)
        {
            try
            {
                // 檢查使用者是否已存在
                var existingUser = await _userRepository.GetByUsernameAsync(username);
                if (existingUser != null)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "使用者名稱已存在"
                    };
                }

                // 檢查郵箱是否已存在
                var existingEmail = await _userRepository.GetByEmailAsync(email);
                if (existingEmail != null)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "電子郵件已被使用"
                    };
                }

                // 創建新使用者
                var newUser = new User
                {
                    Username = username,
                    Email = email,
                    Password = HashPassword(password),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdUser = await _userRepository.CreateAsync(newUser);

                return new AuthResult
                {
                    Success = true,
                    Message = "註冊成功",
                    UserId = createdUser.UserId,
                    User = createdUser
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊過程中發生錯誤");
                return new AuthResult
                {
                    Success = false,
                    Message = "註冊失敗，請稍後再試"
                };
            }
        }

        /// <summary>
        /// 基本登出實作 (IAuthService)
        /// </summary>
        public async Task<bool> LogoutAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                // 清除登入嘗試記錄
                ResetLoginAttempts(user.Username);

                // 記錄登出事件
                await LogAuthenticationEventAsync(new AuthenticationEvent
                {
                    UserId = userId,
                    EventType = "Logout",
                    Success = true,
                    Timestamp = DateTime.UtcNow
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登出過程中發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 基本使用者驗證實作 (IAuthService)
        /// </summary>
        public async Task<bool> ValidateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                return user != null && user.IsActive;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "驗證使用者時發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 基本密碼重設實作 (IAuthService)
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null) return false;

                // 生成重設密碼的 token
                var resetToken = GenerateResetToken();
                
                // 這裡應該發送重設密碼的郵件
                _logger.LogInformation("為使用者 {Email} 生成密碼重設 token: {Token}", email, resetToken);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重設密碼時發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 基本密碼變更實作 (IAuthService)
        /// </summary>
        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                // 驗證舊密碼
                if (!VerifyPassword(oldPassword, user.Password))
                {
                    return false;
                }

                // 更新密碼
                user.Password = HashPassword(newPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                // 記錄密碼變更事件
                await LogAuthenticationEventAsync(new AuthenticationEvent
                {
                    UserId = userId,
                    EventType = "PasswordChanged",
                    Success = true,
                    Timestamp = DateTime.UtcNow
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "變更密碼時發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 啟用多因素認證
        /// </summary>
        public async Task<MFAConfigResult> EnableMFAAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new MFAConfigResult
                    {
                        Success = false,
                        Message = "使用者不存在"
                    };
                }

                if (user.RequiresMFA)
                {
                    return new MFAConfigResult
                    {
                        Success = false,
                        Message = "多因素認證已啟用"
                    };
                }

                // 生成 MFA 密鑰
                var mfaSecret = GenerateMFASecret();
                user.MFASecret = mfaSecret;
                user.RequiresMFA = true;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);
                await LogSecurityEventAsync(userId, "MFA_ENABLED", "啟用多因素認證");

                _logger.LogInformation("使用者 {UserId} 啟用多因素認證", userId);
                return new MFAConfigResult
                {
                    Success = true,
                    Message = "多因素認證已啟用",
                    MFASecret = mfaSecret,
                    QRCodeUrl = GenerateQRCodeUrl(user.Username, mfaSecret)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "啟用多因素認證時發生錯誤: {UserId}", userId);
                return new MFAConfigResult
                {
                    Success = false,
                    Message = "啟用多因素認證失敗"
                };
            }
        }

        /// <summary>
        /// 停用多因素認證
        /// </summary>
        public async Task<MFAConfigResult> DisableMFAAsync(int userId, string currentPassword)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new MFAConfigResult
                    {
                        Success = false,
                        Message = "使用者不存在"
                    };
                }

                if (!VerifyPassword(currentPassword, user.Password))
                {
                    return new MFAConfigResult
                    {
                        Success = false,
                        Message = "密碼錯誤"
                    };
                }

                user.RequiresMFA = false;
                user.MFASecret = null;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);
                await LogSecurityEventAsync(userId, "MFA_DISABLED", "停用多因素認證");

                _logger.LogInformation("使用者 {UserId} 停用多因素認證", userId);
                return new MFAConfigResult
                {
                    Success = true,
                    Message = "多因素認證已停用"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停用多因素認證時發生錯誤: {UserId}", userId);
                return new MFAConfigResult
                {
                    Success = false,
                    Message = "停用多因素認證失敗"
                };
            }
        }

        /// <summary>
        /// 評估詐騙風險
        /// </summary>
        public async Task<FraudRiskAssessment> AssessFraudRiskAsync(User user)
        {
            var riskFactors = new List<string>();
            var riskScore = 0;

            // 檢查登入時間模式
            var currentHour = DateTime.UtcNow.Hour;
            if (currentHour < 6 || currentHour > 23)
            {
                riskFactors.Add("異常登入時間");
                riskScore += 20;
            }

            // 檢查登入頻率
            var recentLogins = await GetRecentLoginAttemptsAsync(user.UserId);
            if (recentLogins.Count > 5)
            {
                riskFactors.Add("頻繁登入嘗試");
                riskScore += 30;
            }

            // 檢查 IP 地址異常
            // 這裡可以整合 IP 地理位置檢查
            riskFactors.Add("IP 地址檢查待實作");

            // 檢查帳號年齡
            var accountAge = DateTime.UtcNow - user.CreatedAt;
            if (accountAge.TotalDays < 1)
            {
                riskFactors.Add("新註冊帳號");
                riskScore += 15;
            }

            // 檢查帳號狀態
            if (!user.IsActive)
            {
                riskFactors.Add("非活躍帳號");
                riskScore += 25;
            }

            var riskLevel = riskScore switch
            {
                < 30 => FraudRiskLevel.Low,
                < 60 => FraudRiskLevel.Medium,
                _ => FraudRiskLevel.High
            };

            return new FraudRiskAssessment
            {
                RiskLevel = riskLevel,
                RiskScore = riskScore,
                RiskFactors = riskFactors,
                AssessmentTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 記錄安全事件
        /// </summary>
        public async Task LogSecurityEventAsync(int userId, string eventType, string description)
        {
            try
            {
                // 這裡應該實作實際的安全事件記錄
                // 可以寫入資料庫或發送到安全監控系統
                _logger.LogInformation("安全事件: 使用者 {UserId}, 事件類型: {EventType}, 描述: {Description}", 
                    userId, eventType, description);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "記錄安全事件時發生錯誤: {UserId}, {EventType}", userId, eventType);
            }
        }

        /// <summary>
        /// 獲取安全審計日誌
        /// </summary>
        public async Task<List<SecurityAuditLog>> GetSecurityAuditLogsAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // 這裡應該實作實際的審計日誌查詢
                // 目前返回模擬資料
                var logs = new List<SecurityAuditLog>
                {
                    new SecurityAuditLog
                    {
                        UserId = userId,
                        EventType = "LOGIN_SUCCESS",
                        Description = "登入成功",
                        Timestamp = DateTime.UtcNow.AddHours(-1),
                        IPAddress = "192.168.1.100",
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"
                    },
                    new SecurityAuditLog
                    {
                        UserId = userId,
                        EventType = "MFA_ENABLED",
                        Description = "啟用多因素認證",
                        Timestamp = DateTime.UtcNow.AddDays(-1),
                        IPAddress = "192.168.1.100",
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"
                    }
                };

                if (startDate.HasValue)
                    logs = logs.Where(l => l.Timestamp >= startDate.Value).ToList();

                if (endDate.HasValue)
                    logs = logs.Where(l => l.Timestamp <= endDate.Value).ToList();

                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取安全審計日誌時發生錯誤: {UserId}", userId);
                return new List<SecurityAuditLog>();
            }
        }

        /// <summary>
        /// 高級安全認證
        /// </summary>
        public async Task<AuthenticationResult> AuthenticateWithSecurityAsync(string identifier, string password, AuthenticationContext context)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(identifier);
                if (user == null || !VerifyPassword(password, user.Password))
                {
                    await LogAuthenticationEventAsync(new AuthenticationEvent
                    {
                        UserId = user?.UserId ?? 0,
                        IpAddress = context.IpAddress,
                        UserAgent = context.UserAgent,
                        Success = false,
                        EventType = "LoginFailed",
                        Timestamp = DateTime.UtcNow
                    });

                    return new AuthenticationResult
                    {
                        Success = false,
                        ErrorMessage = "使用者名稱或密碼錯誤"
                    };
                }

                return new AuthenticationResult
                {
                    Success = true,
                    User = user,
                    AccessToken = GenerateAccessToken(user),
                    RefreshToken = GenerateRefreshToken(user),
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    RequiresMfa = user.RequiresMFA
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "認證過程中發生錯誤");
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "認證服務暫時不可用"
                };
            }
        }

        /// <summary>
        /// 驗證密碼強度
        /// </summary>
        public async Task<bool> ValidatePasswordStrengthAsync(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            var hasUpperCase = password.Any(char.IsUpper);
            var hasLowerCase = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }

        /// <summary>
        /// 檢查密碼是否洩露
        /// </summary>
        public async Task<bool> CheckPasswordLeakAsync(string password)
        {
            // 這裡應該調用外部 API 檢查密碼是否在已知洩露列表中
            // 目前返回 false 表示未洩露
            return false;
        }

        /// <summary>
        /// 刷新 Token
        /// </summary>
        public async Task<TokenRefreshResult> RefreshTokenAsync(string refreshToken, string userAgent)
        {
            try
            {
                // 驗證 refresh token 並生成新的 access token
                var user = await ValidateRefreshTokenAsync(refreshToken);
                if (user == null)
                {
                    return new TokenRefreshResult
                    {
                        Success = false,
                        ErrorMessage = "無效的刷新令牌"
                    };
                }

                return new TokenRefreshResult
                {
                    Success = true,
                    NewAccessToken = GenerateAccessToken(user),
                    NewRefreshToken = GenerateRefreshToken(user),
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新令牌時發生錯誤");
                return new TokenRefreshResult
                {
                    Success = false,
                    ErrorMessage = "刷新令牌失敗"
                };
            }
        }

        /// <summary>
        /// 鎖定帳號
        /// </summary>
        public async Task<LockResult> LockAccountAsync(int userId, LockReason reason, TimeSpan? duration = null)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new LockResult
                    {
                        Success = false,
                        Message = "使用者不存在"
                    };
                }

                var lockoutTime = DateTime.UtcNow.Add(duration ?? TimeSpan.FromMinutes(30));
                _lockoutTimes[user.Username] = lockoutTime;

                return new LockResult
                {
                    Success = true,
                    Reason = reason,
                    LockedAt = DateTime.UtcNow,
                    UnlockAt = lockoutTime,
                    Message = $"帳號已鎖定，原因: {reason}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "鎖定帳號時發生錯誤");
                return new LockResult
                {
                    Success = false,
                    Message = "鎖定帳號失敗"
                };
            }
        }

        /// <summary>
        /// 解鎖帳號
        /// </summary>
        public async Task<bool> UnlockAccountAsync(int userId, string unlockReason)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                _lockoutTimes.Remove(user.Username);
                _loginAttempts.Remove(user.Username);

                _logger.LogInformation("帳號 {UserId} 已解鎖，原因: {Reason}", userId, unlockReason);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解鎖帳號時發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 檢查帳號是否被鎖定
        /// </summary>
        public async Task<bool> IsAccountLockedAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                return IsAccountLocked(user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查帳號鎖定狀態時發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 獲取最近的登入嘗試
        /// </summary>
        public async Task<IEnumerable<LoginAttempt>> GetRecentLoginAttemptsAsync(int userId, TimeSpan timeWindow)
        {
            try
            {
                // 這裡應該從資料庫獲取登入嘗試記錄
                // 目前返回空列表
                return new List<LoginAttempt>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取登入嘗試記錄時發生錯誤");
                return new List<LoginAttempt>();
            }
        }

        /// <summary>
        /// 記錄認證事件
        /// </summary>
        public async Task LogAuthenticationEventAsync(AuthenticationEvent authEvent)
        {
            try
            {
                _logger.LogInformation("認證事件: 使用者 {UserId}, 類型: {EventType}, 成功: {Success}", 
                    authEvent.UserId, authEvent.EventType, authEvent.Success);
                
                // 這裡應該將事件保存到資料庫
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "記錄認證事件時發生錯誤");
            }
        }

        /// <summary>
        /// 獲取認證歷史
        /// </summary>
        public async Task<IEnumerable<AuthenticationEvent>> GetAuthenticationHistoryAsync(int userId, TimeSpan period)
        {
            try
            {
                // 這裡應該從資料庫獲取認證歷史
                // 目前返回空列表
                return new List<AuthenticationEvent>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取認證歷史時發生錯誤");
                return new List<AuthenticationEvent>();
            }
        }

        /// <summary>
        /// 生成安全報告
        /// </summary>
        public async Task<SecurityReport> GenerateSecurityReportAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new SecurityReport
                    {
                        Success = false,
                        Message = "使用者不存在"
                    };
                }

                return new SecurityReport
                {
                    Success = true,
                    UserId = userId,
                    LastLoginAt = user.LastLoginAt,
                    FailedLoginAttempts = _loginAttempts.GetValueOrDefault(user.Username, 0),
                    IsAccountLocked = IsAccountLocked(user.Username),
                    RequiresMfa = user.RequiresMFA,
                    SecurityScore = CalculateSecurityScore(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成安全報告時發生錯誤");
                return new SecurityReport
                {
                    Success = false,
                    Message = "生成安全報告失敗"
                };
            }
        }

        /// <summary>
        /// 檢查速率限制
        /// </summary>
        public async Task<RateLimitResult> CheckRateLimitAsync(string identifier, string action)
        {
            try
            {
                var key = $"{identifier}:{action}";
                var attempts = _loginAttempts.GetValueOrDefault(key, 0);
                var isLimited = attempts >= 5; // 5次嘗試後限制

                return new RateLimitResult
                {
                    IsLimited = isLimited,
                    RemainingAttempts = Math.Max(0, 5 - attempts),
                    ResetTime = DateTime.UtcNow.AddMinutes(15)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查速率限制時發生錯誤");
                return new RateLimitResult
                {
                    IsLimited = true,
                    RemainingAttempts = 0,
                    ResetTime = DateTime.UtcNow.AddMinutes(15)
                };
            }
        }

        /// <summary>
        /// 重置速率限制
        /// </summary>
        public async Task<bool> ResetRateLimitAsync(string identifier, string action)
        {
            try
            {
                var key = $"{identifier}:{action}";
                _loginAttempts.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置速率限制時發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 獲取被封鎖的 IP
        /// </summary>
        public async Task<IEnumerable<string>> GetBlockedIPsAsync()
        {
            try
            {
                // 這裡應該從資料庫獲取被封鎖的 IP 列表
                // 目前返回空列表
                return new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取被封鎖 IP 時發生錯誤");
                return new List<string>();
            }
        }

        /// <summary>
        /// 封鎖 IP
        /// </summary>
        public async Task<bool> BlockIPAsync(string ipAddress, TimeSpan duration)
        {
            try
            {
                _logger.LogWarning("IP {IpAddress} 已被封鎖 {Duration} 分鐘", ipAddress, duration.TotalMinutes);
                
                // 這裡應該將 IP 封鎖記錄保存到資料庫
                await Task.CompletedTask;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "封鎖 IP 時發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 設置 MFA
        /// </summary>
        public async Task<MfaSetupResult> SetupMfaAsync(int userId, MfaMethod method)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new MfaSetupResult
                    {
                        Success = false,
                        Message = "使用者不存在"
                    };
                }

                return new MfaSetupResult
                {
                    Success = true,
                    SetupCode = GenerateMFACode(user.Username),
                    BackupCodes = GenerateBackupCodes(),
                    Message = "MFA 設置成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "設置 MFA 時發生錯誤");
                return new MfaSetupResult
                {
                    Success = false,
                    Message = "設置 MFA 失敗"
                };
            }
        }

        /// <summary>
        /// 驗證 MFA Token
        /// </summary>
        public async Task<bool> ValidateMfaTokenAsync(int userId, string token)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                return VerifyMFACode(user.Username, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "驗證 MFA Token 時發生錯誤");
                return false;
            }
        }

        /// <summary>
        /// 獲取可用的 MFA 方法
        /// </summary>
        public async Task<IEnumerable<MfaMethod>> GetAvailableMfaMethodsAsync(int userId)
        {
            try
            {
                return new List<MfaMethod> { MfaMethod.Totp, MfaMethod.Email, MfaMethod.Sms };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取 MFA 方法時發生錯誤");
                return new List<MfaMethod>();
            }
        }

        /// <summary>
        /// 停用 MFA
        /// </summary>
        public async Task<bool> DisableMfaAsync(int userId, string confirmationCode)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                // 驗證確認碼
                if (!VerifyMFACode(user.Username, confirmationCode))
                    return false;

                // 停用 MFA
                user.RequiresMFA = false;
                await _userRepository.UpdateAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停用 MFA 時發生錯誤");
                return false;
            }
        }

        // 私有方法

        private bool IsAccountLocked(string username)
        {
            if (!_lockoutTimes.ContainsKey(username))
                return false;

            var lockoutEnd = _lockoutTimes[username];
            if (DateTime.UtcNow < lockoutEnd)
                return true;

            // 鎖定時間已過，清除鎖定狀態
            _lockoutTimes.Remove(username);
            _loginAttempts.Remove(username);
            return false;
        }

        private DateTime? GetLockoutEndTime(string username)
        {
            return _lockoutTimes.ContainsKey(username) ? _lockoutTimes[username] : null;
        }

        private void IncrementLoginAttempts(string username)
        {
            if (!_loginAttempts.ContainsKey(username))
                _loginAttempts[username] = 0;

            _loginAttempts[username]++;

            // 超過 5 次失敗嘗試，鎖定帳號 30 分鐘
            if (_loginAttempts[username] >= 5)
            {
                _lockoutTimes[username] = DateTime.UtcNow.AddMinutes(30);
                _logger.LogWarning("帳號 {Username} 因多次登入失敗被鎖定", username);
            }
        }

        private void ResetLoginAttempts(string username)
        {
            _loginAttempts.Remove(username);
            _lockoutTimes.Remove(username);
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            // 這裡應該使用 BCrypt 或其他安全的密碼驗證方法
            // 目前使用簡單的雜湊比較作為示例
            var hashedInput = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(inputPassword)));
            return hashedInput == hashedPassword;
        }

        private string GenerateMFACode(string username)
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            _mfaCodes[username] = code;
            
            // 5 分鐘後自動清除
            Task.Delay(TimeSpan.FromMinutes(5)).ContinueWith(_ => _mfaCodes.Remove(username));
            
            return code;
        }

        private bool VerifyMFACode(string username, string code)
        {
            return _mfaCodes.ContainsKey(username) && _mfaCodes[username] == code;
        }

        private string GenerateMFASecret()
        {
            var random = new Random();
            var bytes = new byte[32];
            random.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string GenerateQRCodeUrl(string username, string secret)
        {
            // 生成 Google Authenticator 格式的 QR Code URL
            var issuer = "GameCore";
            var label = username;
            return $"otpauth://totp/{issuer}:{label}?secret={secret}&issuer={issuer}";
        }

        private async Task<List<LoginAttempt>> GetRecentLoginAttemptsAsync(int userId)
        {
            // 這裡應該實作實際的登入嘗試記錄查詢
            // 目前返回模擬資料
            return new List<LoginAttempt>
            {
                new LoginAttempt
                {
                    UserId = userId,
                    Timestamp = DateTime.UtcNow.AddMinutes(-5),
                    Success = false,
                    IPAddress = "192.168.1.100"
                }
            };
        }

        // 私有輔助方法
        private string GenerateAccessToken(User user)
        {
            // 這裡應該使用 JWT 或其他 token 生成方法
            return $"access_token_{user.UserId}_{DateTime.UtcNow.Ticks}";
        }

        private string GenerateRefreshToken(User user)
        {
            // 這裡應該生成安全的刷新令牌
            return $"refresh_token_{user.UserId}_{DateTime.UtcNow.Ticks}";
        }

        private async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {
            // 這裡應該驗證刷新令牌並返回對應的使用者
            // 目前返回 null
            return null;
        }

        private int CalculateSecurityScore(User user)
        {
            var score = 100;
            if (!user.RequiresMFA) score -= 20;
            if (string.IsNullOrEmpty(user.Email)) score -= 10;
            if (string.IsNullOrEmpty(user.Phone)) score -= 10;
            return Math.Max(0, score);
        }

        private List<string> GenerateBackupCodes()
        {
            var codes = new List<string>();
            var random = new Random();
            for (int i = 0; i < 5; i++)
            {
                codes.Add(random.Next(100000, 999999).ToString());
            }
            return codes;
        }

        // 私有輔助方法
        private string HashPassword(string password)
        {
            // 這裡應該使用 BCrypt 或其他安全的密碼雜湊方法
            // 目前使用簡單的 SHA256 作為示例
            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private string GenerateResetToken()
        {
            var random = new Random();
            var bytes = new byte[32];
            random.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
} 