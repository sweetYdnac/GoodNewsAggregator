namespace by.Reba.Core.Abstractions
{
    public interface IUserPreferenceService
    {
        Task CreateDefaultUserPreferenceAsync(Guid userId);
        Task<int> UpdateAsync(Guid id, Guid ratingId, IEnumerable<Guid> categoriesId);
    }
}
