using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    public interface ISignInRepository : IRepository<SignInRecord>
    {
        Task<SignInRecord?> GetByUserIdAndDateAsync(int userId, DateTime date);
        Task<IEnumerable<SignInRecord>> GetByUserIdAsync(int userId, int page = 1, int pageSize = 30);
        Task<IEnumerable<SignInRecord>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<UserSignInStats?> GetSignInByDateAsync(int userId, DateTime date);
        Task<IEnumerable<UserSignInStats>> GetRecentSignInsAsync(int userId, int days);
        Task<IEnumerable<UserSignInStats>> GetSignInsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<UserSignInStats>> GetSignInHistoryAsync(int userId, int page, int pageSize);
        void Add(SignInRecord signInRecord);
        void AddStatistics(UserSignInStats statistics);
        void UpdateStatistics(UserSignInStats statistics);
    }
}