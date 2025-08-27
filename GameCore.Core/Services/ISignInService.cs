using GameCore.Core.Entities;
using GameCore.Core.Services;
using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    public interface ISignInService
    {
        Task<SignInResult> SignInAsync(int userId);
        Task<SignInStatusResult> GetSignInStatusAsync(int userId);
        Task<IEnumerable<SignInRecordDto>> GetSignInHistoryAsync(int userId, int page = 1, int pageSize = 30);
        Task<SignInStatisticsDto> GetSignInStatisticsAsync(int userId);
        Task<IEnumerable<SignInCalendarDto>> GetMonthlySignInCalendarAsync(int userId, int year, int month);
        Task<bool> IsSignedInTodayAsync(int userId);
        Task<int> GetConsecutiveDaysAsync(int userId);
        Task<bool> IsMonthlyPerfectAsync(int userId, int year, int month);
    }
}