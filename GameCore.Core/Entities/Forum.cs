using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.Entities
{
    public class Forum
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public int CreatedBy { get; set; }
        
        public int PostCount { get; set; } = 0;
        
        public int ReplyCount { get; set; } = 0;
        
        public int ViewCount { get; set; } = 0;
        
        public DateTime? LastPostDate { get; set; }
        
        // Navigation properties
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<ForumSubscription> Subscriptions { get; set; } = new List<ForumSubscription>();
    }
    
    public class ForumSubscription
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int UserId { get; set; }
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Forum Forum { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
} 