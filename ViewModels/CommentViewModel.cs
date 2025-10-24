using System.ComponentModel.DataAnnotations;

namespace PersonalBlogPlatform.ViewModels
{
    public class CommentViewModel
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Comment content is required")]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string Content { get; set; }
        
        [Required]
        public int BlogPostId { get; set; }
        
        public int? ParentCommentId { get; set; }
        
        // For display
        public string? UserName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
