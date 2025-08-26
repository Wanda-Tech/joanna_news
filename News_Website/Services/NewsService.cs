using AutoMapper;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using News_Website.Extension;
using News_Website.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace News_Website.Services
{

    public class NewsService : INewsService
    {
        private readonly NewsWebsiteContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;


        public NewsService(NewsWebsiteContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountService accountService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
        }

        public async Task<PaginatedResponse<simpleNews>> GetAllNewsAsync(NewsSearchRequest request)
        {
            IQueryable<News> query = _context.News
               .Include(n => n.User)
                .Include(n => n.NewsCategory)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query
                    .Where(q => EF.Functions.Like(q.Title, $"%{request.Search}%") ||
                    q.User.Email == request.Search);
            }
            if (request.Status.HasValue)
            {
                query = query.Where(q => q.NewsStatus == request.Status);
            }

            int totalCount = await query.CountAsync();

            List<News> news = await query
                .OrderByDescending(q => q.CreatedDate)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                    .ToListAsync();

            List<simpleNews> simpleNews = _mapper.Map<List<simpleNews>>(news);

            var response = new PaginatedResponse<simpleNews>(simpleNews, request, totalCount);

            return response;
        }

        public async Task<List<simpleNews>> GetRecentNews(int limit = 10)
        {
            DateTime daysBefore = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30));

            List<News> news = await _context.News
                .Include(n => n.User)
                .Include(n => n.NewsCategory)
                .Where(q => q.CreatedDate >= daysBefore)
                .OrderBy(n => Guid.NewGuid()) // Random order
                .Take(limit)
                .ToListAsync();

            List<simpleNews> response = _mapper.Map<List<simpleNews>>(news);

            return response;
        }

        public async Task<simpleNews> GetNewsByIdAsync(int id)
        {
            News? news = await _context.News
                .Include(n => n.User)
                .Include(n => n.NewsCategory)
                .FirstOrDefaultAsync(n => n.NewsId == id);
            ArgumentNullException.ThrowIfNull(news, $"News not found for id {id}");

            simpleNews response = _mapper.Map<simpleNews>(news);

            return response;
        }
        public Task<List<simpleNews>> GetRandomNewsListAsync(int limit = 5)
        {
            return _context.News
                .Include(n => n.User)
                .Include(n => n.NewsCategory)
                .OrderBy(n => Guid.NewGuid()) // Random order
                .Take(limit)
                .Select(n => _mapper.Map<simpleNews>(n))
                .ToListAsync();
        }
        public async Task<int> UpdateLikesAsync(int newsId)
        {
            var news = await _context.News
                .Include(n => n.Likes)  // ensure Likes collection is loaded
                .FirstOrDefaultAsync(n => n.NewsId == newsId);

            if (news == null) return -1;

            var newsLike = new NewsLike
            {
                NewsId = newsId,
                CreatedDate = DateTime.UtcNow,
                UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "Unknown",
                IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
            };

            news.TotalLikes += 1;

            news.Likes.Add(newsLike);

            await _context.SaveChangesAsync();

            return news.TotalLikes;

        }

        public async Task<News> CreateOrUpdateAsync(NewsRequest request)
        {
            if (request.IsUpdating)
            {
                // Fetch existing news
                News? existingNews = await _context.News.FindAsync(request.NewsId);
                if (existingNews == null)
                    throw new Exception("Existing news does not exist");

                // Update properties
                existingNews.Title = request.Title;
                existingNews.Summary = request.Summary;
                existingNews.Content = request.Content;
                existingNews.NewsCategoryId = request.NewsCategoryId;
                existingNews.NewsStatus = request.NewsStatus;

                // ✅ Handle image upload for existing news
                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.ImageFile.CopyToAsync(stream);
                    }

                    existingNews.ImageUrl = "/uploads/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
                return existingNews;
            }
            else
            {
                // Map new news from request
                News newsToCreate = _mapper.Map<News>(request);

                // Assign current user
                newsToCreate.UserId = _accountService.GetCurrentUserId();

                // ✅ Handle image upload for new news
                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.ImageFile.CopyToAsync(stream);
                    }

                    newsToCreate.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _context.News.Add(newsToCreate);
                await _context.SaveChangesAsync();

                return newsToCreate;
            }
        }

        public async Task<SelectList> GetNewsCategorySelectListAsync(int? selectedId = null)
        {
            var categories = await _context.NewsCategories.ToDictionaryAsync(q => q.Id, q => q.Name);

            return new SelectList(categories, "Key", "Value", selectedId);
        }
    }
}

