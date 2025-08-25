using GameCore.Core.Interfaces;
using GameCore.Core.Models;
using GameCore.Core.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 用戶服務實作 - 處理用戶管理相關業務邏輯
    /// </summary>
    public class UserService : IUserService
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;

        public UserService(GameCoreDbContext context, ILogger<UserService> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// 用戶註冊
        /// </summary>
        public async Task<ServiceResult<UserRegistrationResultDto>> RegisterAsync(UserRegistrationDto request)
        {
            try
            {
                _logger.LogInformation("用戶註冊請求: {Email}", request.Email);

                // 檢查郵箱是否已存在
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    _logger.LogWarning("郵箱 {Email} 已被註冊", request.Email);
                    return ServiceResult<UserRegistrationResultDto>.BusinessErrorResult("郵箱已被註冊");
                }

                // 檢查用戶名是否已存在
                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    _logger.LogWarning("用戶名 {Username} 已被使用", request.Username);
                    return ServiceResult<UserRegistrationResultDto>.BusinessErrorResult("用戶名已被使用");
                }

                // 密碼加密
                var passwordHash = HashPassword(request.Password);

                // 創建用戶
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Country = request.Country,
                    City = request.City,
                    Address = request.Address,
                    ProfilePicture = request.ProfilePicture,
                    IsActive = true,
                    EmailVerified = false,
                    PhoneVerified = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                // 創建用戶錢包
                var userWallet = new UserWallet
                {
                    UserId = user.Id,
                    TotalPoints = 0,
                    AvailablePoints = 0,
                    FrozenPoints = 0,
                    LastUpdated = DateTime.UtcNow
                };

                _context.UserWallets.Add(userWallet);

                // 創建用戶介紹
                var userIntroduce = new UserIntroduce
                {
                    UserId = user.Id,
                    Bio = request.Bio ?? "",
                    Interests = request.Interests ?? "",
                    Website = request.Website ?? "",
                    SocialMedia = request.SocialMedia ?? "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserIntroduces.Add(userIntroduce);

                // 創建用戶權限
                var userRights = new UserRights
                {
                    UserId = user.Id,
                    Role = "user",
                    Permissions = "basic",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserRights.Add(userRights);

                await _context.SaveChangesAsync();

                var result = new UserRegistrationResultDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Message = "註冊成功"
                };

                _logger.LogInformation("用戶 {Email} 註冊成功，用戶ID: {UserId}", request.Email, user.Id);
                return ServiceResult<UserRegistrationResultDto>.SuccessResult(result, "註冊成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶註冊時發生錯誤: {Email}", request.Email);
                return ServiceResult<UserRegistrationResultDto>.ServerErrorResult("註冊失敗");
            }
        }

        /// <summary>
        /// 用戶登入
        /// </summary>
        public async Task<ServiceResult<UserLoginResultDto>> LoginAsync(UserLoginDto request)
        {
            try
            {
                _logger.LogInformation("用戶登入請求: {Email}", request.Email);

                // 查找用戶
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email || u.Username == request.Email);

                if (user == null)
                {
                    _logger.LogWarning("登入失敗: 用戶不存在 {Email}", request.Email);
                    return ServiceResult<UserLoginResultDto>.UnauthorizedResult("用戶名或密碼錯誤");
                }

                // 驗證密碼
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("登入失敗: 密碼錯誤 {Email}", request.Email);
                    return ServiceResult<UserLoginResultDto>.UnauthorizedResult("用戶名或密碼錯誤");
                }

                // 檢查用戶狀態
                if (!user.IsActive)
                {
                    _logger.LogWarning("登入失敗: 用戶已被停用 {Email}", request.Email);
                    return ServiceResult<UserLoginResultDto>.ForbiddenResult("帳戶已被停用");
                }

                // 生成 JWT Token
                var token = GenerateJwtToken(user);

                // 更新最後登入時間
                user.LastLoginAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var result = new UserLoginResultDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    Message = "登入成功"
                };

                _logger.LogInformation("用戶 {Email} 登入成功", request.Email);
                return ServiceResult<UserLoginResultDto>.SuccessResult(result, "登入成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶登入時發生錯誤: {Email}", request.Email);
                return ServiceResult<UserLoginResultDto>.ServerErrorResult("登入失敗");
            }
        }

        /// <summary>
        /// 第三方登入
        /// </summary>
        public async Task<ServiceResult<UserLoginResultDto>> ThirdPartyLoginAsync(ThirdPartyLoginDto request)
        {
            try
            {
                _logger.LogInformation("第三方登入請求: {Provider} - {ProviderId}", request.Provider, request.ProviderId);

                // 查找現有用戶
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.ProviderId == request.ProviderId && u.Provider == request.Provider);

                if (existingUser != null)
                {
                    // 現有用戶，直接登入
                    var token = GenerateJwtToken(existingUser);
                    existingUser.LastLoginAt = DateTime.UtcNow;
                    existingUser.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    var result = new UserLoginResultDto
                    {
                        UserId = existingUser.Id,
                        Username = existingUser.Username,
                        Email = existingUser.Email,
                        Token = token,
                        ExpiresAt = DateTime.UtcNow.AddHours(24),
                        Message = "登入成功"
                    };

                    return ServiceResult<UserLoginResultDto>.SuccessResult(result, "登入成功");
                }

                // 新用戶，創建帳戶
                var user = new User
                {
                    Username = request.Username ?? $"user_{Guid.NewGuid().ToString("N")[..8]}",
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ProfilePicture = request.ProfilePicture,
                    Provider = request.Provider,
                    ProviderId = request.ProviderId,
                    IsActive = true,
                    EmailVerified = true, // 第三方登入通常已驗證
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                // 創建用戶錢包
                var userWallet = new UserWallet
                {
                    UserId = user.Id,
                    TotalPoints = 0,
                    AvailablePoints = 0,
                    FrozenPoints = 0,
                    LastUpdated = DateTime.UtcNow
                };

                _context.UserWallets.Add(userWallet);

                // 創建用戶介紹
                var userIntroduce = new UserIntroduce
                {
                    UserId = user.Id,
                    Bio = "",
                    Interests = "",
                    Website = "",
                    SocialMedia = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserIntroduces.Add(userIntroduce);

                // 創建用戶權限
                var userRights = new UserRights
                {
                    UserId = user.Id,
                    Role = "user",
                    Permissions = "basic",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserRights.Add(userRights);

                await _context.SaveChangesAsync();

                // 生成 JWT Token
                var token = GenerateJwtToken(user);

                var result = new UserLoginResultDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    Message = "註冊並登入成功"
                };

                _logger.LogInformation("第三方用戶 {Provider} - {ProviderId} 註冊並登入成功", request.Provider, request.ProviderId);
                return ServiceResult<UserLoginResultDto>.SuccessResult(result, "註冊並登入成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "第三方登入時發生錯誤: {Provider} - {ProviderId}", request.Provider, request.ProviderId);
                return ServiceResult<UserLoginResultDto>.ServerErrorResult("第三方登入失敗");
            }
        }

        /// <summary>
        /// 取得用戶資料
        /// </summary>
        public async Task<ServiceResult<UserProfileDto>> GetProfileAsync(int userId)
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的資料", userId);

                var user = await _context.Users
                    .Include(u => u.UserIntroduce)
                    .Include(u => u.UserRights)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("用戶 {UserId} 不存在", userId);
                    return ServiceResult<UserProfileDto>.NotFoundResult("用戶不存在");
                }

                var profile = new UserProfileDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    Country = user.Country,
                    City = user.City,
                    Address = user.Address,
                    ProfilePicture = user.ProfilePicture,
                    Bio = user.UserIntroduce?.Bio,
                    Interests = user.UserIntroduce?.Interests,
                    Website = user.UserIntroduce?.Website,
                    SocialMedia = user.UserIntroduce?.SocialMedia,
                    Role = user.UserRights?.Role,
                    IsActive = user.IsActive,
                    EmailVerified = user.EmailVerified,
                    PhoneVerified = user.PhoneVerified,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                };

                _logger.LogInformation("成功取得用戶 {UserId} 的資料", userId);
                return ServiceResult<UserProfileDto>.SuccessResult(profile, "成功取得用戶資料");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 資料時發生錯誤", userId);
                return ServiceResult<UserProfileDto>.ServerErrorResult("取得用戶資料失敗");
            }
        }

        /// <summary>
        /// 更新用戶資料
        /// </summary>
        public async Task<ServiceResult<UserProfileDto>> UpdateProfileAsync(int userId, UserProfileUpdateDto request)
        {
            try
            {
                _logger.LogInformation("更新用戶 {UserId} 的資料", userId);

                var user = await _context.Users
                    .Include(u => u.UserIntroduce)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("用戶 {UserId} 不存在", userId);
                    return ServiceResult<UserProfileDto>.NotFoundResult("用戶不存在");
                }

                // 更新基本資料
                if (!string.IsNullOrEmpty(request.FirstName))
                    user.FirstName = request.FirstName;
                if (!string.IsNullOrEmpty(request.LastName))
                    user.LastName = request.LastName;
                if (!string.IsNullOrEmpty(request.Phone))
                    user.Phone = request.Phone;
                if (request.DateOfBirth.HasValue)
                    user.DateOfBirth = request.DateOfBirth;
                if (!string.IsNullOrEmpty(request.Gender))
                    user.Gender = request.Gender;
                if (!string.IsNullOrEmpty(request.Country))
                    user.Country = request.Country;
                if (!string.IsNullOrEmpty(request.City))
                    user.City = request.City;
                if (!string.IsNullOrEmpty(request.Address))
                    user.Address = request.Address;
                if (!string.IsNullOrEmpty(request.ProfilePicture))
                    user.ProfilePicture = request.ProfilePicture;

                user.UpdatedAt = DateTime.UtcNow;

                // 更新或創建用戶介紹
                if (user.UserIntroduce == null)
                {
                    user.UserIntroduce = new UserIntroduce
                    {
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.UserIntroduces.Add(user.UserIntroduce);
                }

                if (!string.IsNullOrEmpty(request.Bio))
                    user.UserIntroduce.Bio = request.Bio;
                if (!string.IsNullOrEmpty(request.Interests))
                    user.UserIntroduce.Interests = request.Interests;
                if (!string.IsNullOrEmpty(request.Website))
                    user.UserIntroduce.Website = request.Website;
                if (!string.IsNullOrEmpty(request.SocialMedia))
                    user.UserIntroduce.SocialMedia = request.SocialMedia;

                user.UserIntroduce.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // 返回更新後的資料
                var profile = new UserProfileDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    Country = user.Country,
                    City = user.City,
                    Address = user.Address,
                    ProfilePicture = user.ProfilePicture,
                    Bio = user.UserIntroduce.Bio,
                    Interests = user.UserIntroduce.Interests,
                    Website = user.UserIntroduce.Website,
                    SocialMedia = user.UserIntroduce.SocialMedia,
                    Role = "user", // 預設角色
                    IsActive = user.IsActive,
                    EmailVerified = user.EmailVerified,
                    PhoneVerified = user.PhoneVerified,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                };

                _logger.LogInformation("成功更新用戶 {UserId} 的資料", userId);
                return ServiceResult<UserProfileDto>.SuccessResult(profile, "成功更新用戶資料");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用戶 {UserId} 資料時發生錯誤", userId);
                return ServiceResult<UserProfileDto>.ServerErrorResult("更新用戶資料失敗");
            }
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        public async Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto request)
        {
            try
            {
                _logger.LogInformation("用戶 {UserId} 變更密碼", userId);

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("用戶 {UserId} 不存在", userId);
                    return ServiceResult.NotFoundResult("用戶不存在");
                }

                // 驗證舊密碼
                if (!VerifyPassword(request.OldPassword, user.PasswordHash))
                {
                    _logger.LogWarning("用戶 {UserId} 變更密碼失敗: 舊密碼錯誤", userId);
                    return ServiceResult.BusinessErrorResult("舊密碼錯誤");
                }

                // 加密新密碼
                user.PasswordHash = HashPassword(request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("用戶 {UserId} 成功變更密碼", userId);
                return ServiceResult.SuccessResult("密碼變更成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶 {UserId} 變更密碼時發生錯誤", userId);
                return ServiceResult.ServerErrorResult("變更密碼失敗");
            }
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        public async Task<ServiceResult> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            try
            {
                _logger.LogInformation("用戶忘記密碼: {Email}", request.Email);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                {
                    // 為了安全，不透露用戶是否存在
                    _logger.LogInformation("忘記密碼請求已處理: {Email}", request.Email);
                    return ServiceResult.SuccessResult("如果郵箱存在，重置連結已發送");
                }

                // 生成重置 Token
                var resetToken = GenerateResetToken();
                var resetTokenExpiry = DateTime.UtcNow.AddHours(24);

                // 儲存重置 Token
                user.ResetToken = resetToken;
                user.ResetTokenExpiry = resetTokenExpiry;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // TODO: 發送重置郵件
                _logger.LogInformation("重置 Token 已生成: {Email} - {Token}", request.Email, resetToken);

                return ServiceResult.SuccessResult("如果郵箱存在，重置連結已發送");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "處理忘記密碼請求時發生錯誤: {Email}", request.Email);
                return ServiceResult.ServerErrorResult("處理忘記密碼請求失敗");
            }
        }

        /// <summary>
        /// 重置密碼
        /// </summary>
        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordDto request)
        {
            try
            {
                _logger.LogInformation("用戶重置密碼: {Token}", request.Token);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == request.Token);
                if (user == null)
                {
                    _logger.LogWarning("重置密碼失敗: 無效的 Token {Token}", request.Token);
                    return ServiceResult.BusinessErrorResult("無效的重置連結");
                }

                if (user.ResetTokenExpiry < DateTime.UtcNow)
                {
                    _logger.LogWarning("重置密碼失敗: Token 已過期 {Token}", request.Token);
                    return ServiceResult.BusinessErrorResult("重置連結已過期");
                }

                // 更新密碼
                user.PasswordHash = HashPassword(request.NewPassword);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("用戶 {UserId} 成功重置密碼", user.Id);
                return ServiceResult.SuccessResult("密碼重置成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置密碼時發生錯誤: {Token}", request.Token);
                return ServiceResult.ServerErrorResult("重置密碼失敗");
            }
        }

        /// <summary>
        /// 驗證 JWT Token
        /// </summary>
        public async Task<ServiceResult<TokenValidationResultDto>> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? "default_secret_key");

                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

                    // 檢查用戶是否存在且啟用
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null || !user.IsActive)
                    {
                        return ServiceResult<TokenValidationResultDto>.UnauthorizedResult("Token 無效");
                    }

                    var result = new TokenValidationResultDto
                    {
                        IsValid = true,
                        UserId = userId,
                        Username = user.Username,
                        Email = user.Email,
                        ExpiresAt = jwtToken.ValidTo
                    };

                    return ServiceResult<TokenValidationResultDto>.SuccessResult(result, "Token 驗證成功");
                }
                catch
                {
                    return ServiceResult<TokenValidationResultDto>.UnauthorizedResult("Token 無效");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "驗證 Token 時發生錯誤");
                return ServiceResult<TokenValidationResultDto>.ServerErrorResult("Token 驗證失敗");
            }
        }

        /// <summary>
        /// 刷新 JWT Token
        /// </summary>
        public async Task<ServiceResult<TokenRefreshResultDto>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                _logger.LogInformation("刷新 Token 請求");

                // TODO: 實作 Refresh Token 邏輯
                // 這裡需要實作 Refresh Token 的驗證和生成邏輯

                var result = new TokenRefreshResultDto
                {
                    NewToken = "new_jwt_token_here",
                    RefreshToken = "new_refresh_token_here",
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };

                return ServiceResult<TokenRefreshResultDto>.SuccessResult(result, "Token 刷新成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新 Token 時發生錯誤");
                return ServiceResult<TokenRefreshResultDto>.ServerErrorResult("Token 刷新失敗");
            }
        }

        // 私有方法

        /// <summary>
        /// 密碼加密
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// 驗證密碼
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }

        /// <summary>
        /// 生成 JWT Token
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? "default_secret_key");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Username", user.Username),
                    new Claim("Email", user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["Jwt:Issuer"] ?? "GameCore",
                Audience = _configuration["Jwt:Audience"] ?? "GameCoreUsers",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// 生成重置 Token
        /// </summary>
        private string GenerateResetToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Replace("=", "");
        }
    }
}