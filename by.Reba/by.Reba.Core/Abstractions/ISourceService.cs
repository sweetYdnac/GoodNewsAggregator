using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.Core.Abstractions
{
    public interface ISourceService
    {
        Task<IEnumerable<SourceDTO>> GetAllAsync();
        Task<IEnumerable<SourceDTO>> GetAllByFilterAsync(int page, int pageSize, string searchString);
        Task<int> GetTotalCountAsync(string searchString);
        Task CreateAsync(CreateOrEditSourceDTO dto);
        Task<CreateOrEditSourceDTO> GetCreateOrEditDTObyIdAsync(Guid id);
        Task UpdateAsync(Guid id, CreateOrEditSourceDTO dto);
        Task RemoveAsync(Guid id);
        Task<SourceDTO> GetByIdAsync(Guid id);
    }
}
