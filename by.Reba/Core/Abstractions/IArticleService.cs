using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticlePreviewDTO>> GetPreviewsByPageAsync(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortType, string searchString);
        Task<ArticleFilterDTO> SetDefaultFilterAsync(ArticleFilterDTO filter);
        Task<int> CreateAsync(CreateOrEditArticleDTO dto);
        Task<ArticleDTO> GetByIdAsync(Guid id);
        Task<ArticleDTO> GetWithCommentsByIdAsync(Guid id);
        Task<CreateOrEditArticleDTO> GetEditArticleDTOByIdAsync(Guid id);
        Task<int> GetTotalCount(ArticleFilterDTO filter, string searchString);
        Task<int> RateAsync(RateEntityDTO dto);
        Task<int> UpdateAsync(Guid id, CreateOrEditArticleDTO dto);
        Task RemoveAsync(Guid id);


        Task CreateArticlesFromAllSourcesRssAsync();
    }
}
