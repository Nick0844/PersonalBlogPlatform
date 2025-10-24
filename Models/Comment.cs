using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalBlogPlatform.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign Keys
        [Required]
        public int BlogPostId { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        public int? ParentCommentId { get; set; }
        
        // Navigation Properties
        [ForeignKey("BlogPostId")]
        public virtual BlogPost BlogPost { get; set; }
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        
        [ForeignKey("ParentCommentId")]
        public virtual Comment? ParentComment { get; set; }
        
        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
