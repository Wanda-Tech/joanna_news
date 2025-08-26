using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using News_Website.Services;
using News_Website.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace News_Website.Controllers;

 [Authorize(Roles = $"{Constants.Roles.Admin},{Constants.Roles.Editor }")]

public class NewsController : Controller
{
    private readonly ILogger<NewsController> _logger;
    private readonly INewsService _NewsService;

    public NewsController(ILogger<NewsController> logger, INewsService NewsService)
    {
        _logger = logger;
        _NewsService = NewsService;
    }

    public async Task<IActionResult> Index(NewsSearchRequest request)
    {
        PaginatedResponse<simpleNews> newsList = await _NewsService.GetAllNewsAsync(request);

        return View(newsList);
    }

    // GET: News/Detail/5
    public async Task<IActionResult> Detail(int id)
    {
        simpleNews? news = await _NewsService.GetNewsByIdAsync(id);

        if (news == null)
        {
            return NotFound();
        }

        NewsDetailResponse response = new NewsDetailResponse
        {
            News = news,
            ReadNextNews = await _NewsService.GetRandomNewsListAsync(3),
        };

        return View(response);
    }
    [HttpPost]
    public async Task<IActionResult> Like(string newsId)
    {
        if (int.TryParse(newsId, out int id))
        {
            int newCounter = await _NewsService.UpdateLikesAsync(id);

            if (newCounter >= 0)
                return Ok(new { totalLikes = newCounter });

            return NotFound("News not found.");
        }

        return BadRequest("Invalid News ID.");
    }




}
