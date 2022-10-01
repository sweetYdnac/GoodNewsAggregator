using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models
{
    public class HomePageVM
    {
        public IEnumerable<ArticlePreviewDTO> Articles { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<PositivityRatingDTO> PositivityRatings { get; set; }
    }
}
