using System.ComponentModel.DataAnnotations;
using PersonalBlogPlatform.Models;

namespace PersonalBlogPlatform.ViewModels
{
    public class BlogPostViewModel
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Summary is required")]
        [StringLength(500, ErrorMessage = "Summary cannot exceed 500 characters")]
        public string Summary { get; set; }
        
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        
        [Display(Name = "Featured Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? FeaturedImageUrl { get; set; }
        
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        
        [Display(Name = "Tags (comma separated)")]
        public string? TagsInput { get; set; }
        
        [Display(Name = "Publish")]
        public bool IsPublished { get; set; }
        
        // For display purposes
        public string? AuthorName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ViewCount { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
