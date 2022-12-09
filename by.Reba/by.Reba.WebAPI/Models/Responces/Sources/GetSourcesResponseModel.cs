using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.WebAPI.Models.Responces.Sources
{
    public class GetSourcesResponseModel
    {
        public IEnumerable<SourceDTO> Sources { get; set; }
    }
}
