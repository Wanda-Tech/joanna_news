using AutoMapper;
using Microsoft.EntityFrameworkCore;
using News_Website.Models;

namespace News_Website.Services {

    public class NewsService
    {
        private readonly NewsWebsiteContext _context;
        private readonly IMapper _mapper;

        public NewsService(NewsWebsiteContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<simpleNews>> GetAllNewsAsync()
        {
            List<News> news = await _context.News
                .Include(n => n.User)
                .Include(n => n.NewsCategory)
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
    }
}

