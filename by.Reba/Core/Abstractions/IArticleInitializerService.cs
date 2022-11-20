namespace by.Reba.Core.Abstractions
{
    public interface IArticleInitializerService
    {
        Task<int> CreateArticlesFromExternalSourcesAsync();
        Task AddTextToArticlesAsync();

        Task AddRatingAsync();
    }
}
