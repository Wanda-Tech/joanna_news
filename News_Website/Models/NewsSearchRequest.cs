
using News_Website.Models;
using System.ComponentModel.DataAnnotations;

namespace News_Website.Models;

public class NewsSearchRequest : SearchRequest
{
    public NewsStatus? Status { get; set; }
}