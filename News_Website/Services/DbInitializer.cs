public static class DbInitializer
{
    public static void Seed(NewsWebsiteContext context)
    {
        if (!context.NewsCategories.Any())
        {
            context.NewsCategories.AddRange(
                new NewsCategory
                {
                    Name = "Articles",
                    Description = "All about articles",
                    NewsList = new List<News>()
                },
                new NewsCategory
                {
                    Name = "Reports",
                    Description = "In-depth reports",
                    NewsList = new List<News>()
                },
                new NewsCategory
                {
                    Name = "Breaking",
                    Description = "Latest breaking news",
                    NewsList = new List<News>()
                }
            );
            context.SaveChanges();
        }
        
        
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin", Description = "Administrator role" },
                new Role { Name = "Editor", Description = "Editor role" },
                new Role { Name = "Viewer", Description = "Viewer role" }
            );
            context.SaveChanges();
        }
    }
}