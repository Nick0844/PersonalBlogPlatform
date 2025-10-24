using Microsoft.AspNetCore.Identity;
using PersonalBlogPlatform.Models;

namespace PersonalBlogPlatform.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
            
            // Seed Roles
            await SeedRolesAsync(roleManager);
            
            // Seed Admin User
            await SeedAdminUserAsync(userManager);
            
            // Seed Categories
            await SeedCategoriesAsync(context);
            
            // Seed Tags
            await SeedTagsAsync(context);
        }
        
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Author", "User" };
            
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }
            }
        }
        
        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@blog.com";
            string adminPassword = "Admin@123";
            
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User",
                    Bio = "Platform Administrator",
                    CreatedAt = DateTime.UtcNow
                };
                
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.AddToRoleAsync(adminUser, "Author");
                }
            }
        }
        
        private static async Task SeedCategoriesAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Technology", Description = "Tech related posts", Slug = "technology" },
                    new Category { Name = "Lifestyle", Description = "Lifestyle and personal posts", Slug = "lifestyle" },
                    new Category { Name = "Travel", Description = "Travel experiences and tips", Slug = "travel" },
                    new Category { Name = "Food", Description = "Food and recipes", Slug = "food" },
                    new Category { Name = "Business", Description = "Business and entrepreneurship", Slug = "business" }
                };
                
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }
        
        private static async Task SeedTagsAsync(ApplicationDbContext context)
        {
            if (!context.Tags.Any())
            {
                var tags = new List<Tag>
                {
                    new Tag { Name = "C#", Slug = "csharp" },
                    new Tag { Name = "ASP.NET Core", Slug = "aspnet-core" },
                    new Tag { Name = "Entity Framework", Slug = "entity-framework" },
                    new Tag { Name = "Web Development", Slug = "web-development" },
                    new Tag { Name = "Tutorial", Slug = "tutorial" }
                };
                
                await context.Tags.AddRangeAsync(tags);
                await context.SaveChangesAsync();
            }
        }
    }
}
