using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
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

        public async Task<int> CreateAsync(PositivityRatingDTO dto)
        {
            var entity = _mapper.Map<T_PositivityRating>(dto);

            if (entity is null)
            {
                throw new ArgumentException($"Cannot map dto = {dto} to entity",nameof(dto));
            }

            await _unitOfWork.PositivityRatings.AddAsync(entity);
            return await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<PositivityRatingDTO>> GetAllOrderedAsync()
        {
            var ratings = await _unitOfWork.PositivityRatings
                .Get()
                .AsNoTracking()
                .OrderBy(r => r.Value)
                .ToArrayAsync();

            return ratings.Select(r => _mapper.Map<PositivityRatingDTO>(r));
        }

        public async Task<PositivityRatingDTO> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.PositivityRatings.GetByIdAsync(id);

            return entity is null 
                ? throw new ArgumentException($"Rating with id = {id} isn't exist", nameof(id)) 
                : _mapper.Map<PositivityRatingDTO>(entity);
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var entity = await _unitOfWork.PositivityRatings.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentNullException($"Positivity with id = {id} isn't exist", nameof(id));
            }

            _unitOfWork.PositivityRatings.Remove(entity);
            return await _unitOfWork.Commit();
        }

        public async Task<int> UpdateAsync(Guid id, PositivityRatingDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException("DTO is null", nameof(dto));
            }

            var entity = await _unitOfWork.PositivityRatings.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentException($"Positivity with id = {id} isn't exist", nameof(dto));
            }

            var patchList = new List<PatchModel>();

            if (!dto.Title.Equals(entity.Title))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Title),
                    PropertyValue = dto.Title,
                });
            }

            if (!dto.Value.Equals(entity.Value))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Value),
                    PropertyValue = dto.Value,
                });
            }

            await _unitOfWork.PositivityRatings.PatchAsync(id, patchList);
            return await _unitOfWork.Commit();
        }
    }
}
