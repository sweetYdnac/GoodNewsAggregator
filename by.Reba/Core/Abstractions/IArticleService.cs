using by.Reba.Core.DataTransferObjects;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<List<ArticlePreviewDTO>> GetByPage(IQueryable<ArticlePreviewDTO> filteredArticles);
        IQueryable<ArticlePreviewDTO> GetByFilter(ArticleFilterDTO filter);
        IQueryable<ArticlePreviewDTO> FilterByPublicationDate(IQueryable<ArticlePreviewDTO> filteredArticles);
        IQueryable<ArticlePreviewDTO> FilterByPositivityRating(IQueryable<ArticlePreviewDTO> filteredArticles);
        IQueryable<ArticlePreviewDTO> FilterByAssessment(IQueryable<ArticlePreviewDTO> filteredArticles);
        IQueryable<ArticlePreviewDTO> FilterByCommentsCount(IQueryable<ArticlePreviewDTO> filteredArticles);
        IQueryable<ArticlePreviewDTO> GetUserPrefered(IQueryable<ArticlePreviewDTO> filteredArticles);
    }
}
