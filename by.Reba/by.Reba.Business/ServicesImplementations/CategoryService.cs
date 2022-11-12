using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;

namespace by.Reba.Business.ServicesImplementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(CategoryDTO dto)
        {
            var entity = _mapper.Map<T_Category>(dto);

            if (entity is null)
            {
                throw new ArgumentException(nameof(dto));
            }

            await _unitOfWork.Categories.AddAsync(entity);
            return await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => _mapper.Map<CategoryDTO>(c));
        }
    }
}
