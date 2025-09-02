using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �p�C����Ʀs������
    /// </summary>
    public interface IMiniGameRepository
    {
        /// <summary>
        /// �ھ�ID���o�p�C���O��
        /// </summary>
        Task<MiniGame?> GetByIdAsync(int id);

        /// <summary>
        /// �ھڥΤ�ID���o�p�C���O���C��
        /// </summary>
        Task<IEnumerable<MiniGame>> GetByUserIdAsync(int userId);

        /// <summary>
        /// �s�W�p�C���O��
        /// </summary>
        Task<MiniGame> AddAsync(MiniGame miniGame);

        /// <summary>
        /// ��s�p�C���O��
        /// </summary>
        Task<MiniGame> UpdateAsync(MiniGame miniGame);

        /// <summary>
        /// �R���p�C���O��
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// �s�W�p�C���O��
        /// </summary>
        Task<MiniGame> Add(MiniGame miniGame);

        /// <summary>
        /// �ˬd�O�_�F��C�魭��
        /// </summary>
        Task<bool> HasReachedDailyLimitAsync(int userId);

        /// <summary>
        /// ���o����C������
        /// </summary>
        Task<int> GetTodayGameCountAsync(int userId);

        /// <summary>
        /// ���o�C���]�w
        /// </summary>
        Task<GameSettings?> GetGameSettingsAsync();

        /// <summary>
        /// ���o�Τ�C���O��
        /// </summary>
        Task<IEnumerable<MiniGame>> GetUserGameRecordsAsync(int userId);

        /// <summary>
        /// ���o�d���C���O��
        /// </summary>
        Task<IEnumerable<MiniGame>> GetPetGameRecordsAsync(int petId);
    }
}
