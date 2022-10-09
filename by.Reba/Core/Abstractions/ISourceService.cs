using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.Core.Abstractions
{
    public interface ISourceService
    {
        Task<IEnumerable<SourceDTO>> GetAllAsync();
    }
}
