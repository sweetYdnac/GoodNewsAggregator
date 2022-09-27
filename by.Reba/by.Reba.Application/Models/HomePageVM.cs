using by.Reba.Core.DataTransferObjects;
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
