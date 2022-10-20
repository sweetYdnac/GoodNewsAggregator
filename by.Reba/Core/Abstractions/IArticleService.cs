using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticlePreviewDTO>> GetPreviewsByPageAsync(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortType, string searchString);
        Task<ArticleFilterDTO> SetDefaultFilterAsync(ArticleFilterDTO filter);
        Task<int> CreateAsync(CreateArticleDTO dto);
        Task<ArticleDTO> GetByIdAsync(Guid id);
        Task<ArticleDTO> GetWithCommentsByIdAsync(Guid id);
        Task<int> GetTotalCount(ArticleFilterDTO filter, string searchString);
    }
}
