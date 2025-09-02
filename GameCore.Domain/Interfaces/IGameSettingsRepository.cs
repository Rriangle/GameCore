using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �C���]�w�ܮw����
    /// </summary>
    public interface IGameSettingsRepository : IRepository<GameSettings>
    {
        /// <summary>
        /// �ھڳ]�w�W�٨��o�]�w��
        /// </summary>
        /// <param name="settingName">�]�w�W��</param>
        /// <returns>�]�w��</returns>
        Task<string?> GetSettingValueAsync(string settingName);

        /// <summary>
        /// �]�w�]�w��
        /// </summary>
        /// <param name="settingName">�]�w�W��</param>
        /// <param name="settingValue">�]�w��</param>
        /// <returns>�]�w���G</returns>
        Task<bool> SetSettingValueAsync(string settingName, string settingValue);

        /// <summary>
        /// ���o�Ҧ��ҥΪ��]�w
        /// </summary>
        /// <returns>�]�w�C��</returns>
        Task<IEnumerable<GameSettings>> GetActiveSettingsAsync();

        /// <summary>
        /// �ˬd�]�w�O�_�s�b
        /// </summary>
        /// <param name="settingName">�]�w�W��</param>
        /// <returns>�O�_�s�b</returns>
        Task<bool> SettingExistsAsync(string settingName);

        /// <summary>
        /// �R���]�w
        /// </summary>
        /// <param name="settingName">�]�w�W��</param>
        /// <returns>�R�����G</returns>
        Task<bool> DeleteSettingAsync(string settingName);

        /// <summary>
        /// ���o�C���]�w
        /// </summary>
        Task<GameSettings?> GetGameSettingsAsync();

        /// <summary>
        /// ���o�Τ�C���O��
        /// </summary>
        Task<IEnumerable<MiniGameRecord>> GetUserGameRecordsAsync(int userId, int page, int pageSize);

        /// <summary>
        /// ���o�d���C���O��
        /// </summary>
        Task<IEnumerable<MiniGameRecord>> GetPetGameRecordsAsync(int petId, int page, int pageSize);

        /// <summary>
        /// �ˬd�O�_�F��C�魭��
        /// </summary>
        Task<bool> HasReachedDailyLimitAsync(int userId, int gameId);

        /// <summary>
        /// ���o����C������
        /// </summary>
        Task<int> GetTodayGameCountAsync(int userId, int gameId);
    }
} 
