using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.Data.Abstractions;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PreferenceService(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator) => 
            (_unitOfWork, _mapper, _mediator) = (unitOfWork, mapper, mediator);

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

        public async Task<PreferenceDTO> GetPreferenceByIdAsync(Guid id)
        {
            var preference = await _unitOfWork.Preferences
                .Get()
                .Include(p => p.Categories)
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            return preference is null
                ? throw new ArgumentException($"Preference with id = {id} doesn't exist", nameof(id))
                : _mapper.Map<PreferenceDTO>(preference);
        }

        public async Task<PreferenceDTO> GetPreferenceByUserEmailAsync(string email)
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
        public async Task SetDefaultFilterAsync(ArticleFilterDTO filter)
        {
            await SetDefaultDatesAndSources(filter);

            if (filter.CategoriesId.Count == 0)
            {
                filter.CategoriesId = await _mediator.Send(new GetCategoriesIdQuery());
            }

            if (filter.MinPositivity.Equals(default))
            {
                filter.MinPositivity = await _mediator.Send(new GetMinPositivityIdQuery());
            }
        }


        public async Task SetPreferenceInFilterAsync(Guid userId, ArticleFilterDTO filter)
        {
            var userPreference = await _unitOfWork.Preferences
                .Get()
                .Include(up => up.Categories)
                .AsNoTracking()
                .FirstOrDefaultAsync(up => up.UserId.Equals(userId));

            if (userPreference is null)
            {
                throw new ArgumentException($"User with id = {userId} doesn't have T_Preference", nameof(userId));
            }

            await SetDefaultDatesAndSources(filter);

            filter.CategoriesId = userPreference.Categories.Select(c => c.Id).ToList();
            filter.MinPositivity = userPreference.MinPositivityId;
        }

        private async Task SetDefaultDatesAndSources(ArticleFilterDTO filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter), "ArticleFilterDTO is null");
            }

            if (filter.From.Equals(default))
            {
                filter.From = DateTime.Now - TimeSpan.FromDays(100);
            }

            if (filter.To.Equals(default))
            {
                filter.To = DateTime.Now;
            }

            if (filter.SourcesId.Count == 0)
            {
                filter.SourcesId = await _unitOfWork.Sources
                    .Get()
                    .AsNoTracking()
                    .Select(s => s.Id)
                    .ToArrayAsync();
            }
        }
    }
}
