using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class SourceService : ISourceService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SourceService(IMediator mediator, IMapper mapper) => (_mediator, _mapper) = (mediator, mapper);

        public async Task<IEnumerable<SourceDTO>> GetAllAsync()
        {
            var sources = await _mediator.Send(new GetSourcesQuery());
            return sources.Select(source => _mapper.Map<SourceDTO>(source));
        }

        public async Task<IEnumerable<SourceDTO>> GetAllByFilterAsync(int page, int pageSize, string searchString)
        {
            var sources = (await GetByFilter(searchString)).OrderBy(s => s.Name);

            return await sources.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<SourceDTO>(art))
                .ToArrayAsync();
        }

        public async Task<int> GetTotalCountAsync(string searchString)
        {
            var sources = await GetByFilter(searchString);
            return await sources.CountAsync();
        }

        private async Task<IQueryable<T_Source>> GetByFilter(string searchString)
        {
            var sources = await _mediator.Send(new GetQueriableSourcesQuery());

            if (!string.IsNullOrEmpty(searchString))
            {
                sources = sources.Where(s => s.Name.Contains(searchString));
            }

            return sources;
        }

        public async Task CreateAsync(CreateOrEditSourceDTO dto)
        {
            var entity = _mapper.Map<T_Source>(dto);

            if (entity is null)
            {
                throw new ArgumentException("Cannot map CreateOrEditSourceDTO to T_Source", nameof(dto));
            }

            await _mediator.Send(new AddSourceCommand() { Source = entity });
        }

        public async Task<CreateOrEditSourceDTO> GetCreateOrEditDTObyIdAsync(Guid id)
        {
            var entity = await _mediator.Send(new GetSourceByIdQuery() { Id = id });

            return entity is null 
                ? throw new ArgumentException($"Source with id = {id} isn't exist", nameof(id)) 
                : _mapper.Map<CreateOrEditSourceDTO>(entity);
        }

        public async Task UpdateAsync(Guid id, CreateOrEditSourceDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto), "CreateOrEditSourceDTO is null");
            }

            var entity = await _mediator.Send(new GetSourceByIdQuery() { Id = id });

            if (entity is null)
            {
                throw new ArgumentException($"Source with id = {id} isn't exist", nameof(id));
            }

            var patchList = new List<PatchModel>();

            if (!dto.Name.Equals(entity.Name))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Name),
                    PropertyValue = dto.Name,
                });
            }

            if (!dto.RssUrl.Equals(entity.RssUrl))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.RssUrl),
                    PropertyValue = dto.RssUrl,
                });
            }

            if (!dto.Type.Equals(entity.Type))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Type),
                    PropertyValue = dto.Type,
                });
            }

            await _mediator.Send(new PatchSourceCommand()
            {
                Id = id,
                PatchData = patchList
            });
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await _mediator.Send(new GetSourceByIdQuery() { Id = id });

            if (entity is null)
            {
                throw new ArgumentException($"Source with id = {id} isn't exist", nameof(id));
            }

            await _mediator.Send(new RemoveSourceCommand() { Source = entity });
        }

        public async Task<SourceDTO> GetByIdAsync(Guid id)
        {
            var entity = await _mediator.Send(new GetSourceByIdQuery() { Id = id });

            return entity is null
                ? throw new ArgumentException($"Source with id = {id} isn't exist", nameof(id))
                : _mapper.Map<SourceDTO>(entity);
        }
    }
}
