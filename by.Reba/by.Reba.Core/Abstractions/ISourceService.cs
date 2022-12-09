using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.Core.Abstractions
{
    public interface ISourceService
    {
        Task<IEnumerable<SourceDTO>> GetAllAsync();
        Task<IEnumerable<SourceDTO>> GetAllByFilterAsync(int page, int pageSize, string searchString);
        Task<int> GetTotalCount(string searchString);
        Task<int> CreateAsync(CreateOrEditSourceDTO dto);
        Task<CreateOrEditSourceDTO> GetCreateOrEditDTObyIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, CreateOrEditSourceDTO dto);
        Task<int> RemoveAsync(Guid id);
        Task<SourceDTO> GetByIdAsync(Guid id);
    }
}
