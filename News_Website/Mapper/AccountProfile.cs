using AutoMapper;
using News_Website.Models;
using News_Website.Extension;
using News_Website.Helpers;

namespace News_Website.Mapper;
public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<SignUpRequest, User>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordHasher.HashPassword(src.Password)));
    }
}