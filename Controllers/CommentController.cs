using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogPlatform.Data;
using PersonalBlogPlatform.Models;
using PersonalBlogPlatform.ViewModels;

namespace PersonalBlogPlatform.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public CommentController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                
                if (user == null)
                {
                    TempData["Error"] = "You must be logged in to comment.";
                    return RedirectToAction("Details", "BlogPost", new { id = model.BlogPostId });
                }
                
                var comment = new Comment
                {
                    Content = model.Content,
                    BlogPostId = model.BlogPostId,
                    UserId = user.Id,
                    ParentCommentId = model.ParentCommentId,
                    CreatedAt = DateTime.UtcNow
                };
                
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Comment posted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to post comment. Please try again.";
            }
            
            return RedirectToAction("Details", "BlogPost", new { id = model.BlogPostId });
        }
        
        // POST: Comment/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int blogPostId)
        {
            var comment = await _context.Comments.FindAsync(id);
            
            if (comment != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (comment.UserId != user.Id && !User.IsInRole("Admin"))
                {
                    TempData["Error"] = "You can only delete your own comments.";
                    return RedirectToAction("Details", "BlogPost", new { id = blogPostId });
                }
                
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Comment deleted successfully!";
            }
            
            return RedirectToAction("Details", "BlogPost", new { id = blogPostId });
        }
    }
}
