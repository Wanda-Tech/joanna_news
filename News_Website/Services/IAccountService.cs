using AutoMapper;
using Microsoft.EntityFrameworkCore;
using News_Website.Models;

namespace News_Website.Services;

public interface IAccountService
{
    public Task<User> AuthenticateAsync(SignInRequest request);
    public Task<User> RegisterNewUser(SignUpRequest request);

    public Task SignOutAsync();
}