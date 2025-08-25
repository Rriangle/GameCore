using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.Services;
using GameCore.Core.DTOs;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace GameCore.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<UserRegistrationResult> RegisterAsync(UserRegistrationDto registrationDto)
        {
            try
            {
                // 檢查用戶名是否已存在
                if (await _userRepository.ExistsByUsernameAsync(registrationDto.Username))
                {
                    return new UserRegistrationResult
                    {
                        Success = false,
                        Message = "用戶名已存在"
                    };
                }

                // 檢查郵箱是否已存在
                if (await _userRepository.ExistsByEmailAsync(registrationDto.Email))
                {
                    return new UserRegistrationResult
                    {
                        Success = false,
                        Message = "郵箱已被註冊"
                    };
                }

                // 創建新用戶
                var user = new User
                {
                    Username = registrationDto.Username,
                    Email = registrationDto.Email,
                    PasswordHash = HashPassword(registrationDto.Password),
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow,
                    IsActive = true,
                    Role = UserRole.User,
                    Points = 100, // 初始點數
                    Experience = 0,
                    Level = 1
                };

                _userRepository.Add(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("新用戶註冊成功: {Username}", user.Username);

                return new UserRegistrationResult
                {
                    Success = true,
                    Message = "註冊成功",
                    UserId = user.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶註冊失敗: {Username}", registrationDto.Username);
                return new UserRegistrationResult
                {
                    Success = false,
                    Message = "註冊失敗，請稍後重試"
                };
            }
        }

        public async Task<UserLoginResult> LoginAsync(UserLoginDto loginDto)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
                if (user == null)
                {
                    return new UserLoginResult
                    {
                        Success = false,
                        Message = "用戶名或密碼錯誤"
                    };
                }

                if (!VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    return new UserLoginResult
                    {
                        Success = false,
                        Message = "用戶名或密碼錯誤"
                    };
                }

                if (!user.IsActive)
                {
                    return new UserLoginResult
                    {
                        Success = false,
                        Message = "帳戶已被停用"
                    };
                }

                // 更新最後登入時間
                user.LastLoginAt = DateTime.UtcNow;
                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶登入成功: {Username}", user.Username);

                return new UserLoginResult
                {
                    Success = true,
                    Message = "登入成功",
                    User = new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role,
                        Points = user.Points,
                        Experience = user.Experience,
                        Level = user.Level,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt,
                        LastLoginAt = user.LastLoginAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶登入失敗: {Username}", loginDto.Username);
                return new UserLoginResult
                {
                    Success = false,
                    Message = "登入失敗，請稍後重試"
                };
            }
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    user.LastLoginAt = DateTime.UtcNow;
                    _userRepository.Update(user);
                    await _unitOfWork.SaveChangesAsync();
                }

                _logger.LogInformation("用戶登出: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶登出失敗: {UserId}", userId);
                return false;
            }
        }

        public async Task<UserProfileResult> GetProfileAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new UserProfileResult
                    {
                        Success = false,
                        Message = "用戶不存在"
                    };
                }

                return new UserProfileResult
                {
                    Success = true,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role,
                        Points = user.Points,
                        Experience = user.Experience,
                        Level = user.Level,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt,
                        LastLoginAt = user.LastLoginAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶資料失敗: {UserId}", userId);
                return new UserProfileResult
                {
                    Success = false,
                    Message = "獲取資料失敗"
                };
            }
        }

        public async Task<UserUpdateResult> UpdateProfileAsync(int userId, UserUpdateDto updateDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new UserUpdateResult
                    {
                        Success = false,
                        Message = "用戶不存在"
                    };
                }

                // 檢查郵箱是否被其他用戶使用
                if (!string.IsNullOrEmpty(updateDto.Email) && updateDto.Email != user.Email)
                {
                    if (await _userRepository.ExistsByEmailAsync(updateDto.Email))
                    {
                        return new UserUpdateResult
                        {
                            Success = false,
                            Message = "郵箱已被其他用戶使用"
                        };
                    }
                    user.Email = updateDto.Email;
                }

                if (!string.IsNullOrEmpty(updateDto.Username) && updateDto.Username != user.Username)
                {
                    if (await _userRepository.ExistsByUsernameAsync(updateDto.Username))
                    {
                        return new UserUpdateResult
                        {
                            Success = false,
                            Message = "用戶名已被使用"
                        };
                    }
                    user.Username = updateDto.Username;
                }

                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶資料更新成功: {UserId}", userId);

                return new UserUpdateResult
                {
                    Success = true,
                    Message = "更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶資料更新失敗: {UserId}", userId);
                return new UserUpdateResult
                {
                    Success = false,
                    Message = "更新失敗"
                };
            }
        }

        public async Task<PasswordChangeResult> ChangePasswordAsync(int userId, PasswordChangeDto passwordDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new PasswordChangeResult
                    {
                        Success = false,
                        Message = "用戶不存在"
                    };
                }

                if (!VerifyPassword(passwordDto.CurrentPassword, user.PasswordHash))
                {
                    return new PasswordChangeResult
                    {
                        Success = false,
                        Message = "當前密碼錯誤"
                    };
                }

                user.PasswordHash = HashPassword(passwordDto.NewPassword);
                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶密碼修改成功: {UserId}", userId);

                return new PasswordChangeResult
                {
                    Success = true,
                    Message = "密碼修改成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶密碼修改失敗: {UserId}", userId);
                return new PasswordChangeResult
                {
                    Success = false,
                    Message = "密碼修改失敗"
                };
            }
        }

        public async Task<bool> ExistsAsync(int userId)
        {
            return await _userRepository.ExistsAsync(userId);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _userRepository.ExistsByUsernameAsync(username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _userRepository.ExistsByEmailAsync(email);
        }

        public async Task<UserDto> GetByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Points = user.Points,
                Experience = user.Experience,
                Level = user.Level,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Points = user.Points,
                Experience = user.Experience,
                Level = user.Level,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Points = user.Points,
                Experience = user.Experience,
                Level = user.Level,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        public async Task<bool> AddPointsAsync(int userId, int points)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                user.Points += points;
                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶 {UserId} 獲得 {Points} 點數", userId, points);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加用戶點數失敗: {UserId}, {Points}", userId, points);
                return false;
            }
        }

        public async Task<bool> AddExperienceAsync(int userId, int experience)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                user.Experience += experience;
                
                // 計算新等級
                var newLevel = CalculateLevel(user.Experience);
                if (newLevel > user.Level)
                {
                    user.Level = newLevel;
                    _logger.LogInformation("用戶 {UserId} 升級到 {Level} 級", userId, newLevel);
                }

                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶 {UserId} 獲得 {Experience} 經驗值", userId, experience);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加用戶經驗值失敗: {UserId}, {Experience}", userId, experience);
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }

        private int CalculateLevel(int experience)
        {
            // 簡單的等級計算公式：每100經驗值升1級
            return Math.Max(1, (experience / 100) + 1);
        }
    }
}