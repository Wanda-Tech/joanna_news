namespace News_Website.Models;

public class AdminDashbord
{
    public List<simpleNews> RecentNews { get; set; }
    public List<User> RecentUsers { get; set; }
    public Dictionary<int, int> NewsVisitsByDay { get; set; }
}