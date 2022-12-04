namespace by.Reba.Core.Abstractions
{
    public interface IArticleInitializerService
    {
        Task<int> CreateArticlesFromExternalSourcesAsync();
        Task AddTextToArticlesAsync(int articlesCount);
        Task AddPositivityToArticlesAsync(int articlesCount);
        Task<int> RemoveEmptyArticles();
    }
}
