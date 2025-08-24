using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    public interface ISignInService
    {
        Task<SignInResult> SignInAsync(int userId);
        Task<SignInStatusResult> GetSignInStatusAsync(int userId);
        Task<IEnumerable<SignInRecordDto>> GetSignInHistoryAsync(int userId, int page = 1, int pageSize = 30);
        Task<bool> IsSignedInTodayAsync(int userId);
        Task<int> GetConsecutiveDaysAsync(int userId);
    }

    public class SignInResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Points { get; set; }
        public int Experience { get; set; }
        public bool IsWeekend { get; set; }
        public int ConsecutiveDays { get; set; }
        public bool IsMonthlyPerfect { get; set; }
    }

    public class SignInStatusResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsSignedInToday { get; set; }
        public bool IsWeekend { get; set; }
        public int ConsecutiveDays { get; set; }
        public DateTime Today { get; set; }
    }

    public class SignInRecordDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SignInDate { get; set; }
        public int Points { get; set; }
        public int Experience { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsPerfect { get; set; }
        public DateTime CreateTime { get; set; }
    }
}