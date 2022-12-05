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

        public PreferenceService(IUnitOfWork unitOfWork, IMapper mapper) => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<int> CreateAsync(PreferenceDTO dto)
        {
            var entity = _mapper.Map<T_Preference>(dto);

            if (entity is null)
            {
                throw new ArgumentException("Cannot map PreferenceDTO to T_Preference", nameof(dto));
            }

            await _unitOfWork.Preferences.AddAsync(entity);
            var result = await _unitOfWork.Commit();
            return result;
        }

        public async Task<int> CreateDefaultPreferenceAsync(Guid userId)
        {
            var entity = new T_Preference()
            {
                Id = Guid.NewGuid(),
                UserId = userId,

                MinPositivityId = await _unitOfWork.Positivities
                    .Get()
                    .OrderBy(r => r.Value)
                    .Select(r => r.Id)
                    .FirstAsync(),

                Categories = await _unitOfWork.Categories
                    .Get()
                    .ToListAsync(),
            };

            await _unitOfWork.Preferences.AddAsync(entity);
            return await _unitOfWork.Commit();
        }

        public async Task<PreferenceDTO> GetPreferenceByEmailAsync(string email)
        {
            var preference = await _unitOfWork.Preferences
                .Get()
                .Include(p => p.Categories)
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.User.Email.Equals(email));

            return preference is null
                ? throw new ArgumentException($"User with email {email} doesn't have preference", nameof(email))
                : _mapper.Map<PreferenceDTO>(preference);
        }

        public async Task<int> UpdateAsync(Guid id, PreferenceDTO dto)
        {
            if (dto.CategoriesId is null)
            {
                throw new ArgumentNullException(nameof(dto), "categoriesId is null");
            }

            if (!dto.CategoriesId.Any())
            {
                throw new ArgumentException("categoriesId is empty", nameof(dto));
            }

            var entity = await _unitOfWork.Preferences
                .FindBy(up => up.Id.Equals(id), up => up.Categories)
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new ArgumentException($"Preference with id = {id} isn't exist" , nameof(id));
            }

            var patchList = new List<PatchModel>();

            if (!dto.PositivityId.Equals(entity.MinPositivityId))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(entity.MinPositivityId),
                    PropertyValue = dto.PositivityId,
                });
            }          

            var allCategories = await _unitOfWork.Categories.GetAllAsync();
            var newCategories = allCategories.Where(c => dto.CategoriesId.Contains(c.Id)).ToList();

            var oldCategoriesId = entity.Categories.Select(c => c.Id).ToList();
            if (!newCategories.Count.Equals(oldCategoriesId.Count) || !newCategories.All(c => oldCategoriesId.Contains(c.Id)))
            {
                entity.Categories = newCategories;
            }

            await _unitOfWork.Preferences.PatchAsync(id, patchList);
            return await _unitOfWork.Commit();
        }
    }
}
