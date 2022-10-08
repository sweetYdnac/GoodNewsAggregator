using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.Application.Models.Article
{
    public class ArticlesGridVM
    {
        public IEnumerable<ArticleDTO> Articles { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<PositivityRatingDTO> PositivityRatings { get; set; }       
        public IEnumerable<SourceDTO> Sources { get; set; } 
    }
}
