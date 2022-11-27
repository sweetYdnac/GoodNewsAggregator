using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.Application.Models.PositivityRating
{
    public class PositivityGridVM
    {
        public IEnumerable<PositivityDTO> Ratings { get; set; } = Enumerable.Empty<PositivityDTO>();
    }
}
