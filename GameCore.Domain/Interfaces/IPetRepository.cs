using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �d���ܮw����
    /// </summary>
    public interface IPetRepository : IRepository<Pet>
    {
        /// <summary>
        /// ���o�Τ᪺�d��
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�d��</returns>
        Task<Pet?> GetByUserIdAsync(int userId);

        /// <summary>
        /// �ˬd�Τ�O�_���d��
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�O�_���d��</returns>
        Task<bool> HasPetAsync(int userId);

        /// <summary>
        /// ��s�d���g���
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="experience">�g���</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateExperienceAsync(int petId, int experience);

        /// <summary>
        /// ��s�d������
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="level">����</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateLevelAsync(int petId, int level);

        /// <summary>
        /// ��s�d�����d��
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="health">���d��</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateHealthAsync(int petId, int health);

        /// <summary>
        /// ��s�d����O
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="energy">��O</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateEnergyAsync(int petId, int energy);

        /// <summary>
        /// ��s�d���ּ֫�
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="happiness">�ּ֫�</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateHappinessAsync(int petId, int happiness);

        /// <summary>
        /// ��s�d�����j��
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="hunger">���j��</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateHungerAsync(int petId, int hunger);

        /// <summary>
        /// ��s�d���߱�
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="mood">�߱�</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateMoodAsync(int petId, int mood);

        /// <summary>
        /// ��s�d����O
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="stamina">��O</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateStaminaAsync(int petId, int stamina);

        /// <summary>
        /// ��s�d���M���
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="cleanliness">�M���</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateCleanlinessAsync(int petId, int cleanliness);

        /// <summary>
        /// ��s�d���C��
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="skinColor">����</param>
        /// <param name="backgroundColor">�I����</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateColorAsync(int petId, string skinColor, string backgroundColor);

        /// <summary>
        /// ��s�̫᤬�ʮɶ�
        /// </summary>
        /// <param name="petId">�d��ID</param>
        /// <param name="interactionTime">���ʮɶ�</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateLastInteractionAsync(int petId, DateTime interactionTime);

        /// <summary>
        /// ��s�d��
        /// </summary>
        Task<Pet> Update(Pet pet);

        /// <summary>
        /// ���o�̫᤬�ʮɶ�
        /// </summary>
        Task<DateTime?> GetLastInteractionTimeAsync(int petId);
    }
} 
