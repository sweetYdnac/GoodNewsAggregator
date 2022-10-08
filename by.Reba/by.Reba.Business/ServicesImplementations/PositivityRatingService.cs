using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Data.Abstractions;

namespace by.Reba.Business.ServicesImplementations
{
    public class PositivityRatingService : IPositivityRatingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PositivityRatingService(
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PositivityRatingDTO>> GetAll()
        {
            var ratings = await _unitOfWork.PositivityRatings.GetAllAsync();
            return ratings.Select(r => _mapper.Map<PositivityRatingDTO>(r));
        }
    }
}
