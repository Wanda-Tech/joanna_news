namespace News_Website.Models
{
    public class NewsDetailResponse
    {
        public simpleNews News { get; set; }
         public List<simpleNews> ReadNextNews { get; set; }
    }
}
