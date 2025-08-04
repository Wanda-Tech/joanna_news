using AutoMapper;

namespace News_Website.Mapper
{
    public class newsProfile : Profile
    {
        public newsProfile()
        {
            CreateMap<News, simpleNews>()
                .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.NewsStatus == NewsStatus.Published))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.NewsCategory.Name))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("MMMM dd, yyyy")));
        }
    }
} 