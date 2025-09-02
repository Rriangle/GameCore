using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�天室�??��??��???
    /// </summary>
    public enum ChatMemberRole
    {
        /// <summary>
        /// 管�???
        /// </summary>
        Admin = 1,

        /// <summary>
        /// ?�員
        /// </summary>
        Member = 2
    }
} 
