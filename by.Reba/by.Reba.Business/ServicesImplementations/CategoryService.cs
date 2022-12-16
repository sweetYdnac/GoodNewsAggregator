using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Data.Abstractions;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Business.ServicesImplementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator) => 
            (_mapper, _mediator) = (mapper, mediator);

        public async Task CreateAsync(CategoryDTO dto)
        {
            var entity = _mapper.Map<T_Category>(dto);

            if (entity is null)
            {
                throw new ArgumentException($"Cannot map CategoryDTO to T_Category", nameof(dto));
            }

            await _mediator.Send(new AddCategoryCommand() { Category = entity });
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllOrderedAsync()
        {
            var categories = await _mediator.Send(new GetNoTrackedOrderedCategoriesQuery());
            return categories.Select(c => _mapper.Map<CategoryDTO>(c));
        }
    }
}
