using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.Data.Abstractions;

namespace by.Reba.Business.ServicesImplementations
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SourceDTO>> GetAllAsync()
        {
            var sources = await _unitOfWork.Sources.GetAllAsync();
            return sources.Select(source => _mapper.Map<SourceDTO>(source));
        }
    }
}
