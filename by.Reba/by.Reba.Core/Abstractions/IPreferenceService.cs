using by.Reba.Core.DataTransferObjects.UserPreference;

namespace by.Reba.Core.Abstractions
{
    public interface IPreferenceService
    {
        Task<int> CreateAsync(PreferenceDTO dto);
        Task<int> CreateDefaultPreferenceAsync(Guid userId);
        Task<int> UpdateAsync(Guid id, PreferenceDTO dto);
        Task<PreferenceDTO> GetPreferenceByUserEmailAsync(string email);
        Task<PreferenceDTO> GetPreferenceByIdAsync(Guid id);
    }
}
