using by.Reba.Core.DataTransferObjects.UserPreference;

namespace by.Reba.Core.Abstractions
{
    public interface IPreferenceService
    {
        Task CreateDefaultPreferenceAsync(Guid userId);
        Task<int> CreateAsync(PreferenceDTO dto);
        Task<int> UpdateAsync(Guid id, Guid ratingId, IEnumerable<Guid> categoriesId);
    }
}
