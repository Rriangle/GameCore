using System;
using System.ComponentModel.DataAnnotations;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 市場?��?實�?
    /// </summary>
    public partial class MarketItem
    {
        [Key]
        public int PProductId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string PProductType { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string PProductTitle { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string PProductName { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string PProductDescription { get; set; } = string.Empty;
        
        public int? ProductId { get; set; }
        
        [Required]
        public int SellerId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string PStatus { get; set; } = "Active";
        
        [Required]
        public decimal Price { get; set; }
        
        [MaxLength(100)]
        public string PProductImgId { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // 導航屬�?
        public virtual ProductInfo? Product { get; set; }
        public virtual User? Seller { get; set; }
    }
} 
