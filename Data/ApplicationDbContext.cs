using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlogPlatform.Models;

namespace PersonalBlogPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostTag> BlogPostTags { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Configure composite key for BlogPostTag (many-to-many)
            builder.Entity<BlogPostTag>()
                .HasKey(bt => new { bt.BlogPostId, bt.TagId });
            
            // Configure relationships
            builder.Entity<BlogPostTag>()
                .HasOne(bt => bt.BlogPost)
                .WithMany(b => b.BlogPostTags)
                .HasForeignKey(bt => bt.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<BlogPostTag>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BlogPostTags)
                .HasForeignKey(bt => bt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure Comment self-referencing relationship
            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configure BlogPost - Comment relationship
            builder.Entity<Comment>()
                .HasOne(c => c.BlogPost)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure BlogPost - Category relationship
            builder.Entity<BlogPost>()
                .HasOne(b => b.Category)
                .WithMany(c => c.BlogPosts)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Configure BlogPost - Author relationship
            builder.Entity<BlogPost>()
                .HasOne(b => b.Author)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configure Comment - User relationship
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
