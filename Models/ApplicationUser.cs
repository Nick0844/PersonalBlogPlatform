using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogPlatform.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FirstName { get; set; }
        
        [StringLength(100)]
        public string? LastName { get; set; }
        
        [StringLength(500)]
        public string? Bio { get; set; }
        
        public string? ProfileImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
