using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.Core.Abstractions
{
    public interface IPositivityRatingService
    {
        Task<IEnumerable<PositivityRatingDTO>> GetAllOrderedAsync();
        Task<int> CreateAsync(PositivityRatingDTO dto);
        Task<PositivityRatingDTO> GetByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, PositivityRatingDTO dto);
        Task<int> RemoveAsync(Guid id);
    }
}
