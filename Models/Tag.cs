using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogPlatform.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public string? Slug { get; set; }
        
        // Navigation Properties
        public virtual ICollection<BlogPostTag> BlogPostTags { get; set; } = new List<BlogPostTag>();
    }
}
