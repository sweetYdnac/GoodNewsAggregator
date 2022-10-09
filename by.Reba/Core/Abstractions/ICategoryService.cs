using by.Reba.Core.DataTransferObjects.Category;

namespace by.Reba.Core.Abstractions
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
    }
}
