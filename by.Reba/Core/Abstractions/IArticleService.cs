using by.Reba.Core.DataTransferObjects;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleService
    {
        Task<List<ArticlePreviewDTO>> GetByPage(int page, int countOnPage, ArticleFilterDTO filter);
        Task<IQueryable<ArticlePreviewDTO>> GetUserPrefered(Guid userId);
    }
}
