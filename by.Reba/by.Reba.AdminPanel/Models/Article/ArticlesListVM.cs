using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.AdminPanel.Models.Article
{
    public class ArticlesListVM
    {
        public IEnumerable<ArticleDTO> Articles { get; set; } = Enumerable.Empty<ArticleDTO>();
        public IEnumerable<CategoryDTO> Categories { get; set; } = Enumerable.Empty<CategoryDTO>();
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<PositivityRatingDTO> PositivityRatings { get; set; } = Enumerable.Empty<PositivityRatingDTO>();
        public IEnumerable<SourceDTO> Sources { get; set; }

    }
}
