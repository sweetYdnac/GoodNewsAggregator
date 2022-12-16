using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.UserPreference;

namespace by.Reba.Core.Abstractions
{
    public interface IPreferenceService
    {
        Task CreateAsync(PreferenceDTO dto);
        Task CreateDefaultPreferenceAsync(Guid userId);
        Task UpdateAsync(Guid id, PreferenceDTO dto);
        Task<PreferenceDTO> GetPreferenceByUserEmailAsync(string email);
        Task<PreferenceDTO> GetPreferenceByIdAsync(Guid id);
        Task SetDefaultFilterAsync(ArticleFilterDTO filter);
        Task SetPreferenceInFilterAsync(string userEmail, ArticleFilterDTO filter);
    }
}
