using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.Core.Abstractions
{
    public interface IPositivityService
    {
        Task<IEnumerable<PositivityDTO>> GetAllOrderedAsync();
        Task CreateAsync(PositivityDTO dto);
        Task<PositivityDTO> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, PositivityDTO dto);
        Task RemoveAsync(Guid id);
    }
}
