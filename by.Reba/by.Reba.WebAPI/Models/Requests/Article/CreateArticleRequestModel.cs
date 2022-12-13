using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Article
{
    public class CreateArticleRequestModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string PosterUrl { get; set; }

        public string? HtmlContent { get; set; }

        [Required]
        public string SourceUrl { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid SourceId { get; set; }

        [Required]
        public Guid PositivityId { get; set; }
    }
}
