# Personal Blogging Platform

A fully functional blogging platform built with **ASP.NET Core MVC**, **Entity Framework Core**, and **ASP.NET Core Identity** for user authentication and authorization.

## 🎯 Features

- ✅ **User Authentication & Registration** - Secure login/register with ASP.NET Core Identity
- ✅ **Role-Based Authorization** - Admin, Author, and User roles
- ✅ **Blog Post Management** - Create, Read, Update, Delete (CRUD) operations
- ✅ **Categories & Tags** - Organize posts with categories and tags
- ✅ **Commenting System** - Nested comments with replies
- ✅ **Search & Filter** - Search posts by title/content, filter by category/tags
- ✅ **Responsive UI** - Bootstrap 5 responsive design
- ✅ **Draft & Publish** - Save drafts or publish immediately
- ✅ **View Tracking** - Track post view counts
- ✅ **User Dashboard** - "My Posts" page to manage your blogs

## 🛠️ Technologies Used

- **ASP.NET Core 8.0 MVC**
- **Entity Framework Core 8.0**
- **SQL Server LocalDB**
- **ASP.NET Core Identity**
- **Bootstrap 5**
- **Razor Views**
- **C#**

## 📋 Prerequisites

Before running this project, make sure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (included with Visual Studio)
- A code editor (Visual Studio 2022, VS Code, or JetBrains Rider)
- Git (for cloning the repository)

## 🚀 Installation & Setup

### 1. Clone the repository

git clone https://github.com/Nick0844/PersonalBlogPlatform.git
cd PersonalBlogPlatform

### 2. Install EF Core tools (if not already installed)

    dotnet tool install --global dotnet-ef

## To verify installation:

    dotnet ef


### 3. Restore NuGet packages

    dotnet restore


### 4. Update the connection string (Optional)

Edit `appsettings.json` if you want to change the database connection:

{
"ConnectionStrings": {
"DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=PersonalBlogPlatformDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
}


### 5. Run database migrations

    dotnet ef migrations add InitialCreate
    dotnet ef database update


This will:
- Create the database `PersonalBlogPlatformDb`
- Create all necessary tables (Users, BlogPosts, Comments, Categories, Tags, etc.)
- Seed initial data:
  - **Roles**: Admin, Author, User
  - **Admin user** with credentials below
  - **5 Categories**: Technology, Lifestyle, Travel, Food, Business
  - **5 Tags**: C#, ASP.NET Core, Entity Framework, Web Development, Tutorial

### 6. Build the project

    dotnet build


### 7. Run the application

    dotnet run


Or press **F5** in Visual Studio to run with debugging.

The application will start at:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`

Open your browser and navigate to one of the URLs above.

## 👤 Default Admin Credentials

After running migrations, you can login with:

- **Email**: `admin@blog.com`
- **Password**: `Admin@123`

**Note**: For security, change these credentials after first login in a production environment.

## 📁 Project Structure

PersonalBlogPlatform/
├── Controllers/ # MVC Controllers
│ ├── HomeController.cs # Home page and error handling
│ ├── AccountController.cs # User authentication (login/register)
│ ├── BlogPostController.cs # Blog CRUD operations
│ └── CommentController.cs # Comment management
├── Models/ # Database Entity Models
│ ├── ApplicationUser.cs # Extended Identity user
│ ├── BlogPost.cs # Blog post entity
│ ├── Comment.cs # Comment entity (supports nested replies)
│ ├── Category.cs # Category entity
│ ├── Tag.cs # Tag entity
│ └── BlogPostTag.cs # Many-to-many join table
├── ViewModels/ # View Models for data transfer
│ ├── LoginViewModel.cs
│ ├── RegisterViewModel.cs
│ ├── BlogPostViewModel.cs
│ └── CommentViewModel.cs
├── Views/ # Razor Views
│ ├── Home/ # Home page views
│ ├── Account/ # Login and register views
│ ├── BlogPost/ # Blog CRUD views
│ └── Shared/ # Shared layouts and partials
├── Data/ # Database Context & Seeding
│ ├── ApplicationDbContext.cs # EF Core DbContext
│ └── DbSeeder.cs # Database seeding logic
├── wwwroot/ # Static files
│ ├── css/ # Stylesheets
│ ├── js/ # JavaScript files
│ └── lib/ # Third-party libraries (Bootstrap, jQuery)
├── Migrations/ # EF Core migrations
├── Program.cs # Application entry point
├── appsettings.json # Configuration settings
└── PersonalBlogPlatform.csproj # Project file


## 🎨 Key Features Explained

### User Roles & Permissions

1. **Admin** 👑
   - Full access to all features
   - Can edit/delete any user's posts
   - Can manage all comments
   - Access to all blog posts (including drafts)

2. **Author** ✍️
   - Can create and publish blog posts
   - Can edit/delete their own posts
   - Can manage comments on their posts
   - Access to "My Posts" dashboard

3. **User** 👤
   - Can read all published posts
   - Can write their own blog posts
   - Can comment on posts
   - Can edit/delete their own comments

### Blog Post Lifecycle

1. **Draft** 📝
   - Visible only to the author in "My Posts" page
   - Can be edited before publishing
   - Shows "Draft" badge in My Posts

2. **Published** 🌐
   - Visible to everyone on Home and Blog pages
   - Appears in search results
   - Shows in category and tag filters
   - Tracks view counts

### Search & Filter Capabilities

- **Search**: Find posts by title, content, or summary
- **Filter by Category**: View all posts in a specific category
- **Filter by Tags**: View all posts with a specific tag
- **Sort**: Posts ordered by publication date (newest first)
- **Author Filter**: View posts by specific authors (in development)

## 🔐 Security Features

- ✅ **Password Requirements**: Minimum 6 characters, must include uppercase, lowercase, digit, and special character
- ✅ **Anti-Forgery Tokens**: CSRF protection on all forms
- ✅ **Role-Based Authorization**: `[Authorize]` attributes on protected actions
- ✅ **Ownership Validation**: Users can only edit/delete their own content
- ✅ **Admin Override**: Admins can moderate any content
- ✅ **Secure Password Hashing**: Identity framework uses secure hashing algorithms

## 🗄️ Database Schema

### Key Tables:

**AspNetUsers** (Extended Identity)
- Id, UserName, Email, PasswordHash
- FirstName, LastName, Bio, ProfileImageUrl
- CreatedAt

**BlogPosts**
- Id, Title, Summary, Content
- FeaturedImageUrl, CreatedAt, UpdatedAt, PublishedAt
- IsPublished, ViewCount
- AuthorId (FK), CategoryId (FK)

**Comments**
- Id, Content, CreatedAt, UpdatedAt
- BlogPostId (FK), UserId (FK)
- ParentCommentId (FK - for nested replies)

**Categories**
- Id, Name, Description, Slug

**Tags**
- Id, Name, Slug

**BlogPostTags** (Many-to-Many Join)
- BlogPostId (FK), TagId (FK)
- Composite Primary Key

### Relationships:
- User → BlogPosts (One-to-Many)
- User → Comments (One-to-Many)
- BlogPost → Comments (One-to-Many)
- BlogPost → Category (Many-to-One)
- BlogPost ↔ Tags (Many-to-Many via BlogPostTags)
- Comment → Comment (Self-referencing for replies)

## 🎯 Usage Guide

### For Blog Authors:

1. **Register/Login**: Create an account or login
2. **Create Post**: Click "New Post" in navigation
3. **Write Content**: Fill in title, summary, and content
4. **Add Category & Tags**: Organize your post
5. **Save as Draft**: Uncheck "Publish" to save as draft
6. **Publish**: Check "Publish immediately" to make it public
7. **Manage Posts**: View all your posts in "My Posts"

### For Readers:

1. **Browse Posts**: Visit Home or Blog page
2. **Search**: Use search bar to find posts
3. **Filter**: Click categories or tags to filter
4. **Read**: Click "Read More" to view full post
5. **Comment**: Login and leave comments
6. **Reply**: Reply to existing comments

## 🚧 Future Enhancements

- [ ] Rich text editor (TinyMCE or CKEditor) for blog content
- [ ] Image upload functionality (local storage or cloud)
- [ ] Email notifications for new comments
- [ ] Social media sharing buttons
- [ ] Post analytics dashboard (views, comments, likes)
- [ ] User profile pages with avatars
- [ ] RSS/Atom feed for blog posts
- [ ] Archive by date (year/month navigation)
- [ ] Post scheduling (publish at specific time)
- [ ] Markdown support for posts
- [ ] Dark mode theme
- [ ] API endpoints for mobile apps

## 🐛 Known Issues

- None currently reported

## 📝 Changelog

### Version 1.0.0 (October 2025)
- Initial release
- Basic blogging functionality
- User authentication and authorization
- CRUD operations for posts
- Commenting system
- Search and filter features

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### How to contribute:
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is open source and available under the [MIT License](LICENSE).

## 👨‍💻 Author

**Nikhil Singh**
- GitHub: [@Nick0844](https://github.com/Nick0844)
- Email: nikhilsingh4086@gmail.com

## 🙏 Acknowledgments

- Built as part of my .NET Core Project learning journey
- Thanks to the ASP.NET Core community for excellent documentation
- Bootstrap team for the responsive UI framework
- Microsoft for the comprehensive Entity Framework Core and Identity documentation

## 📚 Resources

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Bootstrap 5 Documentation](https://getbootstrap.com/docs/5.0/getting-started/introduction/)

## ⚠️ Disclaimer

This is an educational project created for learning purposes. While it includes security best practices, it should be thoroughly reviewed and tested before being used in a production environment.

---

**Built with ❤️ using ASP.NET Core MVC**

*Last Updated: October 24, 2025*

