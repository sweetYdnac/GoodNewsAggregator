using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PreferenceService(
            IUnitOfWork unitOfWork, 
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(PreferenceDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentException("UserPreferenceDTO is null", nameof(dto));
            }

            var entity = _mapper.Map<T_Preference>(dto);

            if (entity is null)
            {
                throw new ArgumentException("Incorrect mapping from UserPreferenceDTO to T_UserPreference", nameof(entity));
            }

            var existedPreference = await _unitOfWork.UsersPreferences
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(up => up.UserId.Equals(entity.UserId));

            if (existedPreference is not null)
            {
                throw new ArgumentException($"Preference with userId = {entity.UserId} is exist", nameof(existedPreference));
            }

            var categories = new List<T_Category>();
            foreach (var id in dto.CategoriesId)
            {
                var category = await _unitOfWork.Categories
                    .Get()
                    .FirstOrDefaultAsync(c => c.Id.Equals(id));

                if (category is null)
                {
                    throw new ArgumentException("dto.CategoriesId contains incorrect id", nameof(dto.CategoriesId));
                }

                categories.Add(category);
            }

            entity.Categories = categories;

            await _unitOfWork.UsersPreferences.AddAsync(entity);
            return await _unitOfWork.Commit();
        }

        public async Task CreateDefaultPreferenceAsync(Guid userId)
        {
            var entity = new T_Preference()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PositivityRatingId = await _unitOfWork.PositivityRatings
                                            .Get()
                                            .OrderBy(r => r.Value)
                                            .Select(r => r.Id)
                                            .FirstAsync(),
                Categories = await _unitOfWork.Categories
                                            .Get()
                                            .ToListAsync(),
            };

            await _unitOfWork.UsersPreferences.AddAsync(entity);
        }

        public async Task<int> UpdateAsync(Guid id, Guid ratingId, IEnumerable<Guid> categoriesId)
        {
            if (categoriesId is null || !categoriesId.Any())
            {
                throw new ArgumentException(nameof(categoriesId));
            }

            var entity = await _unitOfWork.UsersPreferences
                .FindBy(up => up.Id.Equals(id), up => up.Categories)
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new ArgumentException($"User preference with id = {id} isn't exist" , nameof(id));
            }

            var patchList = new List<PatchModel>();

            if (!ratingId.Equals(entity.PositivityRatingId))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(entity.PositivityRatingId),
                    PropertyValue = ratingId,
                });
            }          

            var allCategories = await _unitOfWork.Categories.GetAllAsync();
            var newCategories = allCategories.Where(c => categoriesId.Contains(c.Id)).ToList();

            var oldCategoriesId = entity.Categories.Select(c => c.Id).ToList();
            if (!newCategories.Count().Equals(oldCategoriesId.Count()) || !newCategories.All(c => oldCategoriesId.Contains(c.Id)))
            {
                entity.Categories = newCategories;
            }

            await _unitOfWork.UsersPreferences.PatchAsync(id, patchList);
            return await _unitOfWork.Commit();
        }
    }
}
