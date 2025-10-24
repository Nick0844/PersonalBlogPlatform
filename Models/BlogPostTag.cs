using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalBlogPlatform.Models
{
    public class BlogPostTag
    {
        public int BlogPostId { get; set; }
        
        public int TagId { get; set; }
        
        // Navigation Properties
        [ForeignKey("BlogPostId")]
        public virtual BlogPost BlogPost { get; set; }
        
        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
}
