using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class NewsRequest
{
    public int? NewsId { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; }

    public string Content { get; set; }
    public string? Summary { get; set; }

    public NewsStatus NewsStatus { get; set; }
    public string? ImageUrl { get; set; }

    [Display(Name = "Upload Image")]
    public IFormFile? ImageFile { get; set; }

    [Required(ErrorMessage = "Please choose a News Category")]
    public int NewsCategoryId { get; set; }

    public bool IsUpdating => NewsId.HasValue && NewsId > 0;

    public SelectList? NewsCategorySelectList { get; set; }

    public NewsRequest()
    {
    }


    // Constructor for News entity
    public NewsRequest(News news)
    {
        NewsId = news.NewsId;
        Content = news.Content;
        Title = news.Title;
        Summary = news.Summary;
        NewsCategoryId = news.NewsCategoryId;
        ImageUrl = news.ImageUrl;
    }

    // Constructor for simpleNews entity (if needed)
    public NewsRequest(simpleNews news)
    {
        NewsId = news.NewsId;
        Content = news.Content;
        Title = news.Title;
        Summary = news.Summary;
        NewsCategoryId = news.NewsCategoryId;
        ImageUrl = news.ImageUrl;
    }
}
