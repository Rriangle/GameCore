using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    public class UserRegistrationDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UserProfileResult
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public string? Bio { get; set; }
    }

    public class UserUpdateDto
    {
        [StringLength(50)]
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Avatar { get; set; }
        public string? Bio { get; set; }
    }

    public class UserUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserProfileResult? User { get; set; }
    }

    public class PasswordChangeDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class PasswordChangeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }
} 
