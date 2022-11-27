using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork) => (_mapper, _unitOfWork) = (mapper, unitOfWork);

        public async Task<int> CreateAsync(CategoryDTO dto)
        {
            var entity = _mapper.Map<T_Category>(dto);

            if (entity is null)
            {
                throw new ArgumentException($"Cannot map CategoryDTO to T_Category", nameof(dto));
            }

            await _unitOfWork.Categories.AddAsync(entity);
            return await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllOrderedAsync()
        {
            var categories = await _unitOfWork.Categories
                .Get()
                .AsNoTracking()
                .OrderBy(c => c.Title)
                .ToArrayAsync();

            return categories.Select(c => _mapper.Map<CategoryDTO>(c));
        }
    }
}
