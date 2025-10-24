using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalBlogPlatform.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Summary { get; set; }
        
        [Required]
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        
        public string? FeaturedImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? PublishedAt { get; set; }
        
        public bool IsPublished { get; set; } = false;
        
        public int ViewCount { get; set; } = 0;
        
        // Foreign Keys
        [Required]
        public string AuthorId { get; set; }
        
        public int? CategoryId { get; set; }
        
        // Navigation Properties
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<BlogPostTag> BlogPostTags { get; set; } = new List<BlogPostTag>();
    }
}
