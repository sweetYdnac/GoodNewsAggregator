using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticlePreviewDTO>> GetByPage(int page, int countOnPage);
        Task<IEnumerable<ArticleDTO>> GetArticleDTOsByPage(int page, int pageSize);

        Task<IEnumerable<ArticleDTO>> GetFilteredAndOrderedByPage(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortType, string searchString);
    }
}
