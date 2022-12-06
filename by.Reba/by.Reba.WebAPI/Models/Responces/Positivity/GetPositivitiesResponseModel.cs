using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.WebAPI.Models.Responces.Positivity
{
    public class GetPositivitiesResponseModel
    {
        public IEnumerable<PositivityDTO> Positivities { get; set; }
    }
}
