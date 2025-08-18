
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using News_Website.Models;

namespace News_Website.Services;

public interface INewsService
{
    public Task<List<simpleNews>> GetAllNewsAsync(); 
    public Task<simpleNews> GetNewsByIdAsync(int id);
    public Task<List<simpleNews>> GetRandomNewsListAsync(int limit = 5);
    public Task<int> UpdateLikesAsync(int newsId);
}