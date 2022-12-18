using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.Data.Abstractions;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.Data.CQS.Queries.Preference;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PreferenceService(IMediator mediator, IMapper mapper, IUnitOfWork unitOfWork) => 
            (_mediator, _mapper) = (mediator, mapper);

        public async Task CreateAsync(PreferenceDTO dto)
        {
            var entity = _mapper.Map<T_Preference>(dto);

            if (entity is null)
            {
                throw new ArgumentException("Cannot map PreferenceDTO to T_Preference", nameof(dto));
            }

            await _mediator.Send(new AddPreferenceCommand() { Preference = entity });
        }

        public async Task CreateDefaultPreferenceAsync(Guid userId)
        {
            var entity = new T_Preference()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MinPositivityId = await _mediator.Send(new GetMinPositivityIdQuery()),
                Categories = await _mediator.Send(new GetTrackedCategoriesQuery()),
            };

            await _mediator.Send(new AddPreferenceCommand() { Preference = entity });
        }

        public async Task<PreferenceDTO> GetPreferenceByIdAsync(Guid id)
        {
            var preference = await _mediator.Send(new GetPreferenceByIdQuery() { Id = id});

            return preference is null
                ? throw new ArgumentException($"Preference with id = {id} doesn't exist", nameof(id))
                : _mapper.Map<PreferenceDTO>(preference);
        }

        public async Task<PreferenceDTO> GetPreferenceByUserEmailAsync(string email)
        {
            var preference = await _mediator.Send(new GetPreferenceByUserEmailQuery() { Email = email });

            return preference is null
                ? throw new ArgumentException($"User with email {email} doesn't have preference", nameof(email))
                : _mapper.Map<PreferenceDTO>(preference);
        }

        public async Task UpdateAsync(Guid id, PreferenceDTO dto)
        {
            if (dto.CategoriesId is null)
            {
                throw new ArgumentNullException(nameof(dto), "categoriesId is null");
            }

            if (!dto.CategoriesId.Any())
            {
                throw new ArgumentException("categoriesId is empty", nameof(dto));
            }

            var entity = await _mediator.Send(new GetPreferenceByIdQuery() { Id = id });

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

            var allCategories = await _mediator.Send(new GetTrackedCategoriesQuery());
            var oldCategoriesId = entity.Categories.Select(c => c.Id).ToList();
            var newCategories = allCategories.Where(c => dto.CategoriesId.Contains(c.Id)).ToList();

            if (!newCategories.Count.Equals(oldCategoriesId.Count) || !newCategories.All(c => oldCategoriesId.Contains(c.Id)))
            {
                entity.Categories = newCategories;
            }

            await _mediator.Send(new PatchPreferenceCommand()
            {
                Id = id,
                PatchData = patchList
            });
        }

        public async Task SetPreferenceInFilterAsync(string userEmail, ArticleFilterDTO filter)
        {
            var userPreference = await _mediator.Send(new GetPreferenceByUserEmailQuery() { Email = userEmail });

            if (userPreference is null)
            {
                throw new ArgumentException($"User with email = {userEmail} haven't T_Preference", nameof(userEmail));
            }

            var setDefaultDatesAndSourcesTask = SetDefaultDatesAndSources(filter);

            filter.CategoriesId = userPreference.Categories.Select(c => c.Id).ToList();
            filter.MinPositivity = userPreference.MinPositivityId;

            await setDefaultDatesAndSourcesTask;
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
                filter.SourcesId = await _mediator.Send(new GetSourcesIdQuery());
            }
        }
    }
}
