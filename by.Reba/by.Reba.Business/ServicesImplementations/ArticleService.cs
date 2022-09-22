using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ArticlePreviewDTO>> GetByPage(int page, int countOnPage)
        {
            return await _unitOfWork.ArticleRepository.Get()
                .AsNoTracking()
                .Skip((page - 1) * countOnPage)
                .Take(countOnPage)
                .Select(art => _mapper.Map<ArticlePreviewDTO>(art))
                .ToListAsync();
        }

        public Task<List<ArticlePreviewDTO>> GetByPage(int page, int countOnPage, ArticleFilterDTO filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<ArticlePreviewDTO>> GetUserPrefered(Guid userId)
        {
            var user = await _unitOfWork.UserRepository
                .Get()
                .AsNoTracking()
                .Include(u => u.Preference)
                .ThenInclude(p => p.MinPositivityRating)
                .Where(u => u.Id.Equals(userId))
                .FirstOrDefaultAsync();

            return _unitOfWork.ArticleRepository
                .Get()
                .AsNoTracking()
                .Where(a => user.Preference.Categories.Contains(a.Category) &&
                            user.Preference.MinPositivityRating.Value >= a.Rating.Value)
                .Select(a => _mapper.Map<ArticlePreviewDTO>(a));
        }
    }
}
