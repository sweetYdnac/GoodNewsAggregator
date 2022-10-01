using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticlePreviewDTO>> GetByPage(int page, int countOnPage);
        Task<IEnumerable<ArticleDTO>> GetArticleDTOsByPage(int page, int pageSize);
        Task<IQueryable<ArticlePreviewDTO>> GetUserPrefered(Guid userId);
        Task<IEnumerable<PositivityRatingDTO>> GetAllPositivityRatings();
        Task<IEnumerable<ArticlePreviewDTO>> GetByFilter(ArticleFilterDTO filter);
        Task<IEnumerable<SourceDTO>> GetAllSources();
    }
}
