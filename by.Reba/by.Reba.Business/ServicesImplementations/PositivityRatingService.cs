using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<PositivityRatingDTO>> GetAllOrderedAsync()
        {
            var ratings = await _unitOfWork.PositivityRatings
                .Get()
                .AsNoTracking()
                .OrderBy(r => r.Value)
                .ToListAsync();

            return ratings.Select(r => _mapper.Map<PositivityRatingDTO>(r));
        }
    }
}
