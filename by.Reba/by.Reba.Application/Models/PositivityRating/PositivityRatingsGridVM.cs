using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.Application.Models.PositivityRating
{
    public class PositivityRatingsGridVM
    {
        public IEnumerable<PositivityRatingDTO> Ratings { get; set; } = Enumerable.Empty<PositivityRatingDTO>();
    }
}
