using by.Reba.Core.DataTransferObjects.UserPreference;

namespace by.Reba.Core.Abstractions
{
    public interface IPreferenceService
    {
        Task<int> CreateDefaultPreferenceAsync(Guid userId);
        Task<int> UpdateAsync(Guid id, PreferenceDTO dto);
        Task<PreferenceDTO> GetPreferenceByEmailAsync(string email);
    }
}
