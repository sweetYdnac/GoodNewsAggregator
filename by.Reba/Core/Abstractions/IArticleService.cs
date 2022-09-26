using by.Reba.Core.DataTransferObjects;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticlePreviewDTO>> GetByPage(int page, int countOnPage);
        Task<List<ArticlePreviewDTO>> GetByPage(int page, int countOnPage, ArticleFilterDTO filter);
        Task<IQueryable<ArticlePreviewDTO>> GetUserPrefered(Guid userId);

        Task<IEnumerable<CategoryDTO>> GetAllCategories();

        Task<IEnumerable<ArticlePreviewDTO>> GetByFilter(ArticleFilterDTO filter);
    }
}
