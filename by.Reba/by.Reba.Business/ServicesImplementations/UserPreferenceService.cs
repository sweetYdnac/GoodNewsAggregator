using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserPreferenceService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateDefaultUserPreferenceAsync(Guid userId)
        {
            var entity = new T_UserPreference()
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
            if (ratingId.Equals(default))
            {
                throw new ArgumentException(nameof(ratingId));
            }

            if (categoriesId is null || !categoriesId.Any())
            {
                throw new ArgumentException(nameof(categoriesId));
            }

            var entity = await _unitOfWork.UsersPreferences
                .FindBy(up => up.Id.Equals(id), up => up.MinPositivityRating, up => up.Categories)
                .FirstOrDefaultAsync();

            var patchList = new List<PatchModel>();

            if (!ratingId.Equals(entity.MinPositivityRating))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(entity.MinPositivityRating),
                    PropertyValue = ratingId,
                });
            }

            var newCategories = new List<T_Category>();

            foreach (var categoryId in categoriesId)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);

                if (category is null)
                {
                    throw new ArgumentException(nameof(categoriesId));
                }

                newCategories.Add(category);
            }

            entity.Categories = newCategories;

            await _unitOfWork.UsersPreferences.PatchAsync(id, patchList);
            return await _unitOfWork.Commit();
        }
    }
}
