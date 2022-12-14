using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Article
{
    public class RateArticleRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool IsLike { get; set; }
    }
}
