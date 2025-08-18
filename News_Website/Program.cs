using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using News_Website.Models;
using News_Website.Services;
using News_Website.Mapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = ".NewsWebsite";
        options.LoginPath = "/Account/SignIn";
        options.LogoutPath = "/Account/SignOut";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddDbContext<NewsWebsiteContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IAccountService, AccountService>();
// auto-mapper
builder.Services.AddAutoMapper(o => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpContextAccessor();
var app = builder.Build();



app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ Run migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NewsWebsiteContext>();
    DbInitializer.Seed(context);
}


app.Run();
