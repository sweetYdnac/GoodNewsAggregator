namespace by.Reba.Core.Abstractions
{
    public interface IArticleInitializerService
    {
        Task CreateArticlesFromExternalSourcesAsync();
        Task AddTextToArticlesAsync(int articlesCount);
        Task AddPositivityToArticlesAsync(int articlesCount);
        Task RemoveEmptyArticles();

        Task Test();
    }
}
