using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.Core.Abstractions
{
    public interface IPositivityService
    {
        Task<IEnumerable<PositivityDTO>> GetAllOrderedAsync();
        Task<int> CreateAsync(PositivityDTO dto);
        Task<PositivityDTO> GetByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, PositivityDTO dto);
        Task<int> RemoveAsync(Guid id);
    }
}
