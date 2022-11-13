namespace by.Reba.Core.Abstractions
{
    public interface IArticleInitializerService
    {
        Task CreateArticlesFromAllSourcesRssAsync();
        Task AddTextToArticlesAsync();
    }
}
