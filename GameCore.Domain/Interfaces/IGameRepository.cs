using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �C���ܮw����
    /// </summary>
    public interface IGameRepository : IRepository<Game>
    {
        /// <summary>
        /// ���o�Ҧ��C��
        /// </summary>
        /// <returns>�C���C��</returns>
        Task<IEnumerable<Game>> GetAllGamesAsync();

        /// <summary>
        /// �ھڤ������o�C��
        /// </summary>
        /// <param name="category">�C������</param>
        /// <returns>�C���C��</returns>
        Task<IEnumerable<Game>> GetGamesByCategoryAsync(string category);

        /// <summary>
        /// �j�M�C��
        /// </summary>
        /// <param name="keyword">����r</param>
        /// <returns>�j�M���G</returns>
        Task<IEnumerable<Game>> SearchGamesAsync(string keyword);

        /// <summary>
        /// ���o�����C��
        /// </summary>
        /// <param name="limit">�ƶq����</param>
        /// <returns>�����C���C��</returns>
        Task<IEnumerable<Game>> GetPopularGamesAsync(int limit = 10);

        /// <summary>
        /// ���o�̷s�C��
        /// </summary>
        /// <param name="limit">�ƶq����</param>
        /// <returns>�̷s�C���C��</returns>
        Task<IEnumerable<Game>> GetLatestGamesAsync(int limit = 10);
    }
} 
