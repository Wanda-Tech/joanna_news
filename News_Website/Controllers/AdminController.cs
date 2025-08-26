using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News_Website.Models;
using News_Website.Services;

namespace News_Website.Controllers;

//[Authorize(Roles = $"{Constants.Roles.Admin}")]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly INewsService _newsService;
    private readonly IAccountService _accountService;

    public AdminController(ILogger<AdminController> logger, INewsService newsService, IAccountService accountService)
    {
        _logger = logger;
        _newsService = newsService;
        _accountService = accountService;
    }

    public async Task<IActionResult> Index()
    {
        AdminDashbord adminDashbord = new AdminDashbord();
        adminDashbord.RecentNews = await _newsService.GetRecentNews();

        adminDashbord.RecentUsers = await _accountService.GetRecentUsers();

        return View(adminDashbord);
    }


    public async Task<IActionResult> News(NewsSearchRequest request)
    {
        ViewBag.Search = request.Search;
        ViewBag.Filter = (int?)request.Status;

        PaginatedResponse<simpleNews> newsList = await _newsService.GetAllNewsAsync(request);

        return View(newsList);
    }

 [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateNews(NewsRequest request)
    {
        try
        {
            if (ModelState.IsValid)
            {
                News news = await _newsService.CreateOrUpdateAsync(request);

                string message = $"News {news.NewsId} was created successfully";

                if (request.IsUpdating)
                {
                    message = "News was just updated";
                }

                SetTempMessage(message);

                return RedirectToAction(nameof(News));
            }
        }
        catch (System.Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        request.NewsCategorySelectList = await _newsService.GetNewsCategorySelectListAsync(request.NewsCategoryId);

        return View(request);
    }
    [HttpGet]
    public async Task<IActionResult> CreateNews()
    {
        // Prepare the request model for the view
        var request = new NewsRequest
        {
            // Load the list of categories for the dropdown
            NewsCategorySelectList = await _newsService.GetNewsCategorySelectListAsync(null)
        };

        return View(request); // This will render Views/Admin/CreateNews.cshtml
    }


    public async Task<IActionResult> EditNews(int id)
    {
        simpleNews news = await _newsService.GetNewsByIdAsync(id);

        NewsRequest request = new NewsRequest(news);

        request.NewsCategorySelectList = await _newsService.GetNewsCategorySelectListAsync(request.NewsCategoryId);

        return View("CreateNews", request);
    }

    private void SetTempMessage(string message)
    {
        TempData["Tmp.Message"] = message;
    }
}