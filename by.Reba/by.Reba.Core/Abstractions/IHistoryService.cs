namespace by.Reba.Core.Abstractions
{
    public interface IHistoryService
    {
        Task<int> AddOrUpdateArticleInHistoryAsync(Guid articleId, string userEmail);
    }
}
