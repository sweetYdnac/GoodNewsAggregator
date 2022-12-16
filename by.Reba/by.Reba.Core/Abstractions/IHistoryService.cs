namespace by.Reba.Core.Abstractions
{
    public interface IHistoryService
    {
        Task AddOrUpdateArticleInHistoryAsync(Guid articleId, string userEmail);
    }
}
