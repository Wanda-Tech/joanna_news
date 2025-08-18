using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
public class simpleNews
{
    public int NewsId { get; set; }
    public string Title { get; set; }

    public string Content { get; set; }
    public string? Summary { get; set; }

    public DateTime CreatedDate { get; set; }
    public string CreatedDateString { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }


    public NewsStatus NewsStatus { get; set; }

    public DateTime? PublishedDate { get; set; }
    public bool IsPublished { get; set; }

    public string? ImageUrl { get; set; }

    public int NewsCategoryId { get; set; }
    public string NewsCategory { get; set; }
    public string CategoryName { get; set; }
    public int TotalLikes { get; set; }

}