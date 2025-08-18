using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using News_Website.Helpers;
using System;
using System.ComponentModel.DataAnnotations;


public static class DbInitializer
{
    public static void Seed(NewsWebsiteContext context)
    {
      
        // Ensure the database is created

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

        User? userAdmin = context.Users.FirstOrDefault(u => u.Email == "admin@news.com");
        // Seed user
        if (!context.Users.Any())
        {
            userAdmin = new User 
            {
                Phone = "77787812",
                Email = "admin@news.com",
                UserRoles = new List<UserRole>(),
            };
            string password = PasswordHasher.HashPassword(userAdmin.Email);

            userAdmin.Password = password;

            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");


            userAdmin.UserRoles.Add(new UserRole
            {
                Role = adminRole
            });
            context.Users.Add(userAdmin);

            context.SaveChanges();
        }




        //seed news
        if (!context.News.Any())
        {
            var allNewsCategories = context.NewsCategories
               .Select(q => q.Id).ToList();
            var images = new[] {
            "https://www.reuters.com/resizer/v2/KALBIIWFZBNBVO3JFREMRRNUOM.jpg?auth=bdd8dbc1b6d1f5e97dc4befa011565e35423ffa58f35ce76fcd198d36c8c5c66&width=1200&quality=80",
            "https://www.reuters.com/resizer/v2/UQC7RFFQYVNIFOKQTRCKFYZNVI.jpg?auth=4d10655282043c200d3771553d5c58f92105caeb3ac615fea36557be5c2767a8&width=1200&quality=80",
            "https://www.reuters.com/resizer/v2/N7XHIIYNANIVLDUB53UV3FRIWM.jpg?auth=2297ef2e78c1e96344542adfa2380390d15b702bb66c95485b2c1f7d8dd0bd47&width=1200&quality=80",
            "https://www.reuters.com/resizer/v2/CO2INK6Y5ZKSPMQFLHKKA3NXME.jpg?auth=9df15600a71bd9f7544157119fb85437f3e2ffd28b66cd22fd53b775e1364ef1&width=1200&quality=80",
            "https://www.reuters.com/resizer/v2/HOGPC37FNNCKVPEAO5INJFPACY.jpg?auth=e328cb1533726343ed51ccdfbd0250cf71559afef8a8ee4f699c23a0e592bd02&width=1920&quality=80",
            };

            var titles = new[] {
            "Big Alcohol prepares to fight back as buzzy cannabis drinks steal sales",
            "'Japanese First' party emerges as election force with tough immigration talk",
            "Green hydrogen retreat poses threat to emissions targets",
            "Hyundai Motor warns of bigger hit from US tariffs after second-quarter profit fall",
            "American Nazis: The Aryan Freedom Network is riding high in Trump era",
            };
            Random rnd = new Random();
            for (int i = 0; i < 6; i++) {
                context.News.Add(
                        new News
                        {

                            //NewsCategory = context.NewsCategories.FirstOrDefault(nc => nc.Name == "Articles"),
                            NewsCategoryId = allNewsCategories[rnd.Next() % allNewsCategories.Count],
                            User = userAdmin,
                            Title = titles[rnd.Next() % titles.Length],
                            ImageUrl = images[rnd.Next() % images.Length],
                            NewsStatus = NewsStatus.Draft,
                            CreatedDate = DateTime.UtcNow,
                            Summary = @"<ul>
                            <li> Sanseito, birthed on YouTube, makes election gains </li>
                            <li> Party has also pledged tax cuts and welfare spending </li>
                            <li> Leader says he wants to expand lower house presence </li>
                            </ul>",
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

        //seed likes
        // After seeding Users and News and calling context.SaveChanges()
      
if (!context.NewsLikes.Any())
        {
            var user = context.Users.FirstOrDefault();
            var news = context.News.FirstOrDefault();
            if (user != null && news != null)
            {
                context.NewsLikes.Add(new NewsLike
                {
                    Id = 1,
                    NewsId = news.NewsId,
                    CreatedDate = DateTime.UtcNow
                });
                context.SaveChanges();
            }
        }
    }
    
} 