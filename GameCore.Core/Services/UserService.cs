using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace GameCore.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.UserName == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.UserAccount == email);
        }

        public async Task<User> CreateUserAsync(string username, string email, string password)
        {
            // 檢查用戶名和郵箱是否已存在
            var existingUser = await GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("用戶名已存在");
            }

            var existingEmail = await GetUserByEmailAsync(email);
            if (existingEmail != null)
            {
                throw new InvalidOperationException("郵箱已存在");
            }

            var user = new User
            {
                UserName = username,
                UserAccount = email,
                UserPassword = HashPassword(password)
            };

            var result = await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdateUserAsync(int userId, string? username = null, string? email = null, string? password = null)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(username))
                user.UserName = username;

            if (!string.IsNullOrEmpty(email))
                user.UserAccount = email;

            if (!string.IsNullOrEmpty(password))
                user.UserPassword = HashPassword(password);

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            await _unitOfWork.UserRepository.DeleteByIdAsync(userId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null) return false;

            return VerifyPassword(password, user.UserPassword);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
}