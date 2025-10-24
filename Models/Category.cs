using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogPlatform.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public string? Slug { get; set; }
        
        // Navigation Properties
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
