using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 寵物表 (對應 Pets)
    /// </summary>
    [Table("Pets")]
    public class Pet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "小可愛";

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = "Default";

        [Required]
        public int Level { get; set; } = 1;

        [Required]
        public int Experience { get; set; } = 0;

        [Required]
        public int Hunger { get; set; } = 100;

        [Required]
        public int Mood { get; set; } = 100;

        [Required]
        public int Stamina { get; set; } = 100;

        [Required]
        public int Cleanliness { get; set; } = 100;

        [Required]
        public int Health { get; set; } = 100;

        [Required]
        [StringLength(20)]
        public string Color { get; set; } = "Default";

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual User User { get; set; } = null!;
        public virtual ICollection<PetInteraction> PetInteractions { get; set; } = new List<PetInteraction>();
    }

    /// <summary>
    /// 寵物互動記錄表 (對應 PetInteractions)
    /// </summary>
    [Table("PetInteractions")]
    public class PetInteraction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string InteractionType { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Pet Pet { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}