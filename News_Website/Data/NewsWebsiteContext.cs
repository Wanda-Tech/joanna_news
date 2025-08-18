using Microsoft.EntityFrameworkCore;
using System;

public class NewsWebsiteContext : DbContext
{
    public NewsWebsiteContext(DbContextOptions<NewsWebsiteContext> options) : base(options)
    {
    }


    public DbSet<News> News { get; set; }
    public DbSet<NewsCategory> NewsCategories { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; } 
    public DbSet<NewsLike> NewsLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleId = 1,
                Description = "Has full access to all resources.",
                Name = "Admin"
            }
        );
        modelBuilder.Entity<NewsLike>().HasData(
            new NewsLike { 
                Id = 1,
                NewsId = 1,
                CreatedDate = new DateTime(2025, 08, 07)
            }
        );
    }


}
