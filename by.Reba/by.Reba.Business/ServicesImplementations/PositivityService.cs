using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Business.ServicesImplementations
{
    public class PositivityService : IPositivityService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PositivityService(IMapper mapper, IMediator mediator) => (_mapper, _mediator) = (mapper, mediator);

        public async Task CreateAsync(PositivityDTO dto)
        {
            var entity = _mapper.Map<T_Positivity>(dto);

            if (entity is null)
            {
                throw new ArgumentException($"Cannot map PositivityDTO to T_Positivity", nameof(dto));
            }

            await _mediator.Send(new AddPositivityCommand() { Positivity = entity });
        }

        public async Task<IEnumerable<PositivityDTO>> GetAllOrderedAsync()
        {
            var ratings = await _mediator.Send(new GetOrderedPositivitiesQuery());
            return ratings.Select(r => _mapper.Map<PositivityDTO>(r));
        }

        public async Task<PositivityDTO> GetByIdAsync(Guid id)
        {
            var entity = await _mediator.Send(new GetPositivityByIdQuery() { Id = id });

            return entity is null 
                ? throw new ArgumentException($"Rating with id = {id} isn't exist", nameof(id)) 
                : _mapper.Map<PositivityDTO>(entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await _mediator.Send(new GetPositivityByIdQuery() { Id = id });

            if (entity is null)
            {
                throw new ArgumentException($"Positivity with id = {id} isn't exist", nameof(id));
            }

            await _mediator.Send(new RemovePositivityCommand() { Positivity = entity });
        }

        public async Task UpdateAsync(Guid id, PositivityDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto), "PositivityDTO is null");
            }

            var entity = await _mediator.Send(new GetPositivityByIdQuery() { Id = id });

            if (entity is null)
            {
                throw new ArgumentException($"Positivity with id = {id} isn't exist", nameof(id));
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

            await _mediator.Send(new PatchPositivityCommand()
            {
                Id = id,
                PatchData = patchList
            });
        }
    }
}
