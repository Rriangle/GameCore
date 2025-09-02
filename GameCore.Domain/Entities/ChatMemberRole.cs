using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?Šå¤©å®¤æ??¡è??²æ???
    /// </summary>
    public enum ChatMemberRole
    {
        /// <summary>
        /// ç®¡ç???
        /// </summary>
        Admin = 1,

        /// <summary>
        /// ?å“¡
        /// </summary>
        Member = 2
    }
} 
