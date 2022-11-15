namespace by.Reba.Core.Abstractions
{
    public interface IArticleInitializerService
    {
        Task CreateArticlesFromExternalSourcesAsync();
        Task AddTextToArticlesAsync();
    }
}
