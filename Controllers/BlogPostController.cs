using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalBlogPlatform.Data;
using PersonalBlogPlatform.Models;
using PersonalBlogPlatform.ViewModels;

namespace PersonalBlogPlatform.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public BlogPostController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // GET: BlogPost
        public async Task<IActionResult> Index(int? categoryId, string? tag, string? search)
        {
            var query = _context.BlogPosts
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.BlogPostTags)
                    .ThenInclude(bt => bt.Tag)
                .Include(b => b.Comments)
                .Where(b => b.IsPublished)
                .AsQueryable();
            
            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId);
            }
            
            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(b => b.BlogPostTags.Any(bt => bt.Tag.Slug == tag));
            }
            
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => 
                    b.Title.Contains(search) || 
                    b.Content.Contains(search) ||
                    b.Summary.Contains(search));
            }
            
            var posts = await query
                .OrderByDescending(b => b.PublishedAt ?? b.CreatedAt)
                .ToListAsync();
            
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            
            return View(posts);
        }
        
        // GET: BlogPost/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var blogPost = await _context.BlogPosts
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Comments.Where(c => c.ParentCommentId == null))
                    .ThenInclude(c => c.User)
                .Include(b => b.Comments.Where(c => c.ParentCommentId == null))
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.User)
                .Include(b => b.BlogPostTags)
                    .ThenInclude(bt => bt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (blogPost == null)
            {
                return NotFound();
            }
            
            // Increment view count
            blogPost.ViewCount++;
            await _context.SaveChangesAsync();
            
            return View(blogPost);
        }
        
        // GET: BlogPost/Create - ANY AUTHENTICATED USER CAN CREATE
        [Authorize]  // Changed from [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View();
        }
        
        // POST: BlogPost/Create - ANY AUTHENTICATED USER CAN CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]  // Changed from [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Create(BlogPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                
                var blogPost = new BlogPost
                {
                    Title = model.Title,
                    Summary = model.Summary,
                    Content = model.Content,
                    FeaturedImageUrl = model.FeaturedImageUrl,
                    CategoryId = model.CategoryId,
                    AuthorId = user.Id,
                    IsPublished = model.IsPublished,
                    CreatedAt = DateTime.UtcNow,
                    PublishedAt = model.IsPublished ? DateTime.UtcNow : null
                };
                
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                
                // Handle tags
                if (!string.IsNullOrEmpty(model.TagsInput))
                {
                    await AddTagsToPost(blogPost.Id, model.TagsInput);
                }
                
                TempData["Success"] = "Blog post created successfully!";
                return RedirectToAction(nameof(Details), new { id = blogPost.Id });
            }
            
            ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
            return View(model);
        }
        
        // GET: BlogPost/Edit/5 - OWNER OR ADMIN CAN EDIT
        [Authorize]  // Changed from [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var blogPost = await _context.BlogPosts
                .Include(b => b.BlogPostTags)
                    .ThenInclude(bt => bt.Tag)
                .FirstOrDefaultAsync(b => b.Id == id);
            
            if (blogPost == null)
            {
                return NotFound();
            }
            
            var user = await _userManager.GetUserAsync(User);
            if (blogPost.AuthorId != user.Id && !User.IsInRole("Admin"))
            {
                TempData["Error"] = "You can only edit your own posts.";
                return RedirectToAction(nameof(Index));
            }
            
            var model = new BlogPostViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Summary = blogPost.Summary,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                CategoryId = blogPost.CategoryId,
                IsPublished = blogPost.IsPublished,
                TagsInput = string.Join(", ", blogPost.BlogPostTags.Select(bt => bt.Tag.Name))
            };
            
            ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
            return View(model);
        }
        
        // POST: BlogPost/Edit/5 - OWNER OR ADMIN CAN EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]  // Changed from [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Edit(int id, BlogPostViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var blogPost = await _context.BlogPosts
                        .Include(b => b.BlogPostTags)
                        .FirstOrDefaultAsync(b => b.Id == id);
                    
                    if (blogPost == null)
                    {
                        return NotFound();
                    }
                    
                    var user = await _userManager.GetUserAsync(User);
                    if (blogPost.AuthorId != user.Id && !User.IsInRole("Admin"))
                    {
                        TempData["Error"] = "You can only edit your own posts.";
                        return RedirectToAction(nameof(Index));
                    }
                    
                    blogPost.Title = model.Title;
                    blogPost.Summary = model.Summary;
                    blogPost.Content = model.Content;
                    blogPost.FeaturedImageUrl = model.FeaturedImageUrl;
                    blogPost.CategoryId = model.CategoryId;
                    blogPost.IsPublished = model.IsPublished;
                    blogPost.UpdatedAt = DateTime.UtcNow;
                    
                    if (model.IsPublished && blogPost.PublishedAt == null)
                    {
                        blogPost.PublishedAt = DateTime.UtcNow;
                    }
                    
                    // Update tags
                    _context.BlogPostTags.RemoveRange(blogPost.BlogPostTags);
                    await _context.SaveChangesAsync();
                    
                    if (!string.IsNullOrEmpty(model.TagsInput))
                    {
                        await AddTagsToPost(blogPost.Id, model.TagsInput);
                    }
                    
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Blog post updated successfully!";
                    return RedirectToAction(nameof(Details), new { id = blogPost.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(model.Id.Value))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            
            ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", model.CategoryId);
            return View(model);
        }
        
        // GET: BlogPost/Delete/5 - OWNER OR ADMIN CAN DELETE
        [Authorize]  // Changed from [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var blogPost = await _context.BlogPosts
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (blogPost == null)
            {
                return NotFound();
            }
            
            var user = await _userManager.GetUserAsync(User);
            if (blogPost.AuthorId != user.Id && !User.IsInRole("Admin"))
            {
                TempData["Error"] = "You can only delete your own posts.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(blogPost);
        }
        
        // POST: BlogPost/Delete/5 - OWNER OR ADMIN CAN DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]  // Changed from [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            
            if (blogPost != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (blogPost.AuthorId != user.Id && !User.IsInRole("Admin"))
                {
                    TempData["Error"] = "You can only delete your own posts.";
                    return RedirectToAction(nameof(Index));
                }
                
                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Blog post deleted successfully!";
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        // GET: My Posts - View user's own posts
        [Authorize]
        public async Task<IActionResult> MyPosts()
        {
            var user = await _userManager.GetUserAsync(User);
            
            var myPosts = await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.Comments)
                .Include(b => b.BlogPostTags)
                    .ThenInclude(bt => bt.Tag)
                .Where(b => b.AuthorId == user.Id)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            
            return View(myPosts);
        }
        
        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
        
        private async Task AddTagsToPost(int postId, string tagsInput)
        {
            var tagNames = tagsInput.Split(',')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct();
            
            foreach (var tagName in tagNames)
            {
                var tag = await _context.Tags
                    .FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
                
                if (tag == null)
                {
                    tag = new Tag
                    {
                        Name = tagName,
                        Slug = tagName.ToLower().Replace(" ", "-")
                    };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }
                
                var blogPostTag = new BlogPostTag
                {
                    BlogPostId = postId,
                    TagId = tag.Id
                };
                
                _context.BlogPostTags.Add(blogPostTag);
            }
            
            await _context.SaveChangesAsync();
        }
    }
}
