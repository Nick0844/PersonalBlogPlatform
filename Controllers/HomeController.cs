using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalBlogPlatform.Data;
using System.Diagnostics;

namespace PersonalBlogPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<IActionResult> Index()
        {
            // Get recent posts with all related data
            var recentPosts = await _context.BlogPosts
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.BlogPostTags)
                    .ThenInclude(bt => bt.Tag)
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.PublishedAt)
                .Take(6)
                .ToListAsync();
            
            // Get categories for sidebar
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            
            // Return the list (even if empty)
            return View(recentPosts);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
