using by.Reba.Core;
using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.Application.Models.Source
{
    public class SourcesGridVM
    {
        public IEnumerable<SourceDTO> Sources { get; set; } = Enumerable.Empty<SourceDTO>();
        public string SearchString { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
