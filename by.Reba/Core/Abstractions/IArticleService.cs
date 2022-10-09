using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticlePreviewDTO>> GetByPageAsync(int page, int countOnPage);

        Task<IEnumerable<ArticleDTO>> GetFilteredAndOrderedByPageAsync(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortType, string searchString);
        Task<ArticleFilterDTO> SetDefaultFilterAsync(ArticleFilterDTO filter);
    }
}
