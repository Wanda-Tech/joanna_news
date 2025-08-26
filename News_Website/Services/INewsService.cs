
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using News_Website.Models;

namespace News_Website.Services;

public interface INewsService
{
    public Task<PaginatedResponse<simpleNews>> GetAllNewsAsync(NewsSearchRequest request);
    public Task<simpleNews> GetNewsByIdAsync(int id);
    public Task<List<simpleNews>> GetRandomNewsListAsync(int limit = 5);
    public Task<int> UpdateLikesAsync(int newsId);
    public Task<List<simpleNews>> GetRecentNews(int limit = 10);
    public Task<News> CreateOrUpdateAsync(NewsRequest request);
    public Task<SelectList> GetNewsCategorySelectListAsync(int? selectedId = null);
}