using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.Core.Abstractions
{
    public interface IPositivityRatingService
    {
        Task<IEnumerable<PositivityRatingDTO>> GetAllOrderedAsync();
    }
}
