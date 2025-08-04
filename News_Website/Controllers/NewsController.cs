using Microsoft.AspNetCore.Mvc;
using News_Website.Services;
using News_Website.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace News_Website.Controllers;

public class NewsController : Controller
{
    private readonly ILogger<NewsController> _logger;
    private readonly INewsService _newsService;

    public NewsController(ILogger<NewsController> logger, INewsService newsService)
    {
        _logger = logger;
        _newsService = newsService;
    }

    public async Task<IActionResult> Index()
    {
        List<simpleNews> newsList = await _newsService.GetAllNewsAsync();

        return View(newsList);
    }

    // GET: News/Detail/5
    public async Task<IActionResult> Detail(int id)
    {
        simpleNews? news = await _newsService.GetNewsByIdAsync(id);

        if (news == null)
        {
            return NotFound();
        }

        NewsDetailResponse response = new NewsDetailResponse
        {
            News = news,
            ReadNextNews = await _newsService.GetRandomNewsListAsync(3),
        };

        return View(response);
    }

}
