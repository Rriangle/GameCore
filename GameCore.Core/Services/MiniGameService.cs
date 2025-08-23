using GameCore.Core.Entities;
using GameCore.Core.Interfaces;

namespace GameCore.Core.Services
{
    public class MiniGameService : IMiniGameService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MiniGameService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MiniGame> StartGameAsync(int userId, int petId, int level)
        {
            var game = new MiniGame
            {
                UserId = userId,
                PetId = petId,
                Level = level,
                MonsterCount = level * 2,
                SpeedMultiplier = 1.0m,
                Result = "Unknown",
                ExpGained = 0,
                PointsChanged = 0,
                HungerDelta = 0,
                MoodDelta = 0,
                StaminaDelta = 0
            };

            var result = await _unitOfWork.MiniGameRepository.AddAsync(game);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> EndGameAsync(int playId, string result, int expGained, int pointsChanged)
        {
            var game = await _unitOfWork.MiniGameRepository.GetByIdAsync(playId);
            if (game == null) return false;

            game.Result = result;
            game.ExpGained = expGained;
            game.ExpGainedTime = DateTime.UtcNow;
            game.PointsChanged = pointsChanged;
            game.PointsChangedTime = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MiniGame>> GetUserGameHistoryAsync(int userId, int page = 1, int pageSize = 20)
        {
            var games = await _unitOfWork.MiniGameRepository.GetAllAsync();
            return games.Where(g => g.UserId == userId)
                       .OrderByDescending(g => g.ExpGainedTime)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize);
        }

        public async Task<MiniGameStats> GetUserGameStatsAsync(int userId)
        {
            var games = await _unitOfWork.MiniGameRepository.GetAllAsync();
            var userGames = games.Where(g => g.UserId == userId).ToList();

            var stats = new MiniGameStats
            {
                TotalGames = userGames.Count,
                WinCount = userGames.Count(g => g.Result == "Win"),
                LoseCount = userGames.Count(g => g.Result == "Lose"),
                AbortCount = userGames.Count(g => g.Result == "Abort"),
                TotalExperienceGained = userGames.Sum(g => g.ExpGained),
                TotalPointsGained = userGames.Sum(g => g.PointsChanged),
                HighestLevel = userGames.Max(g => g.Level)
            };

            return stats;
        }

        public async Task<IEnumerable<MiniGame>> GetTopScoresAsync(int count = 10)
        {
            var games = await _unitOfWork.MiniGameRepository.GetAllAsync();
            return games.Where(g => g.Result == "Win")
                       .OrderByDescending(g => g.ExpGained)
                       .Take(count);
        }
    }
}