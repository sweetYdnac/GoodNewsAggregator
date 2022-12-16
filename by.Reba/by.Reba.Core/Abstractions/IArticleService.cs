using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticlePreviewDTO>> GetPreviewsByPageAsync(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortOrder, string searchString);
        Task CreateAsync(CreateOrEditArticleDTO dto);
        Task<ArticleDTO> GetByIdAsync(Guid id);
        Task<ArticleDTO> GetWithCommentsByIdAsync(Guid id);
        Task<CreateOrEditArticleDTO> GetEditArticleDTOByIdAsync(Guid id);
        Task<int> GetTotalCountAsync(ArticleFilterDTO filter, string searchString);
        Task RateAsync(RateEntityDTO dto);
        Task UpdateAsync(Guid id, CreateOrEditArticleDTO dto);
        Task RemoveAsync(Guid id);
    }
}
