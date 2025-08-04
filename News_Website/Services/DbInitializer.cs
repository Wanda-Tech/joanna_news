using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Xml;
using static System.Net.WebRequestMethods;

public static class DbInitializer
{
    public static void Seed(NewsWebsiteContext context)
    { 
        //seed NewsCategories
        if (!context.NewsCategories.Any())
        {
            context.NewsCategories.AddRange(
                new NewsCategory
                {
                    Name = "Articles",
                    Description = "All about articles",
                    NewsList = new List<News>()
                },
                new NewsCategory
                {
                    Name = "Reports",
                    Description = "In-depth reports",
                    NewsList = new List<News>()
                },
                new NewsCategory
                {
                    Name = "Breaking",
                    Description = "Latest breaking news",
                    NewsList = new List<News>()
                }
            );
            context.SaveChanges();
        }


        //seed Roles
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin", Description = "Administrator role" },
                new Role { Name = "Editor", Description = "Editor role" },
                new Role { Name = "Viewer", Description = "Viewer role" }
            );
            context.SaveChanges();
        }
        // Seed user
        User? userAdmin = context.Users.FirstOrDefault(u => u.Email == "admin@news.com");
        if (userAdmin == null)
        {
            userAdmin = new User
            {
                Phone = "1234567890",
                Email = "admin@news.com",
                Password = "admin@news.com",
                UserRoles = new List<UserRole>()
            };
            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (adminRole != null)
            {
                userAdmin.UserRoles.Add(new UserRole
                {
                    Role = adminRole
                });
            }
            context.Users.Add(userAdmin);
            context.SaveChanges(); // Save so UserId is generated
        }

        
        //seed news
        if (!context.News.Any())
        {
            var allNewsCategories = context.NewsCategories
                .Select(q => q.Id).ToList();
            var images = new[]
            {
                "https://www.reuters.com/resizer/v2/DA25HNIGKBJGXODOJJY3HV4SA4.jpg?auth=81baedfd13e7c51c9b1a8db0d35e10d2b0852f3e5017d3c2d2881bb24d1605a8&width=240&quality=80",
                "https://www.reuters.com/resizer/v2/U7HZ3YNZF5JYZLNAKBJI5C3VPA.jpg?auth=fb72b8482ddf551603788a8df1ec9776708eec9d416d44def0a509badcbcdb99&width=1200&quality=80",
                "https://www.reuters.com/resizer/v2/FEOHDPJXUNMZNNM6J2XRCTOXXA.jpg?auth=c2dadfbc67e5a472e0235f737fe27e299e7b321867deeafd018791a045b54c94&width=1200&quality=80",
                "https://www.reuters.com/resizer/v2/DPBY2SBFVBPLPEWH7VNBDEMGCY.jpg?auth=b0cd30a20c32ec79c6d649c48ddab565cb29c827f7648838369bfe75320da0da&width=1200&quality=80",
            };
            Random rnd = new Random();
            for (int i = 0; i < 20; i++) {
                context.News.Add(
                        new News
                        {

                            //NewsCategory = context.NewsCategories.FirstOrDefault(nc => nc.Name == "Articles"),
                            NewsCategoryId = allNewsCategories[rnd.Next() % allNewsCategories.Count],
                            User = userAdmin,
                            CreatedDate = DateTime.UtcNow,
                            Title = @"'Japanese first' party emerges as election force with tough immigration talk",
                            NewsStatus = NewsStatus.Draft,
                            Summary = @"<ul>
                            <li> Sanseito, birthed on YouTube, makes election gains </li>
                            <li> Party has also pledged tax cuts and welfare spending </li>
                            <li> Leader says he wants to expand lower house presence </li>
                            </ul>",
                            ImageUrl = images[rnd.Next() % images.Length],
                            Content = @"
                            <p>
                                TOKYO, July 21 (Reuters) - The fringe far-right Sanseito party emerged as one of the biggest winners in
                                Japan's upper house election on Sunday, gaining support with warnings of a ""silent invasion"" of
                                immigrants, and pledges for tax cuts and welfare spending.
                            </p>

                            <p>
                                Birthed on YouTube during the COVID-19 pandemic spreading conspiracy theories about vaccinations and a
                                cabal of global elites, the party broke into mainstream politics with its ""Japanese First"" campaign.
                            </p>

                            <p>
                                ""We were criticized as being xenophobic and discriminatory. The public came to understand that the media
                                was wrong and Sanseito was right,"" Kamiya said.
                            </p>

                            <p>
                                Kamiya's message grabbed voters frustrated with a weak economy and currency that has lured tourists in
                                record numbers in recent years, further driving up prices that Japanese can ill afford, political
                                analysts say.
                            </p>

                            <p>
                                Japan's fast-ageing society has also seen foreign-born residents hit a record of about 3.8 million last
                                year, though that is just 3% of the total population, a fraction of the corresponding proportion in the
                                United States and Europe.
                            </p>

                            <h4>INSPIRED BY TRUMP</h4>

                            <p>
                                Kamiya, a former supermarket manager and English teacher, told Reuters before the election that he had
                                drawn inspiration from U.S. President Donald Trump's ""bold political style"".
                            </p>

                            <p>
                                He has also drawn comparisons with Germany's AfD and Reform UK although right-wing populist policies
                                have yet to take root in Japan as they have in Europe and the United States.
                            </p>

                            <p>
                                Post-election, Kamiya said he plans to follow the example of Europe's emerging populist parties by
                                building alliances with other small parties rather than work with an LDP administration, which has ruled
                                for most of Japan's postwar history.
                            </p>",
                        }

                );
            };                          
            context.SaveChanges();          
        }
    } 
    
} 