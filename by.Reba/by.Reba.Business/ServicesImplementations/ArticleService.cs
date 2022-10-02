using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.Data.Abstractions;
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

        public async Task<IEnumerable<ArticlePreviewDTO>> GetByPage(int page, int pageSize)
        {
            return await _unitOfWork.Articles.Get()
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<ArticlePreviewDTO>(art))
                .ToListAsync();
        }

        public Task<List<ArticlePreviewDTO>> GetByPage(int page, int countOnPage, ArticleFilterDTO filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<ArticlePreviewDTO>> GetUserPrefered(Guid userId)
        {
            var user = await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .Include(u => u.Preference)
                .ThenInclude(p => p.MinPositivityRating)
                .Where(u => u.Id.Equals(userId))
                .FirstOrDefaultAsync();

            return _unitOfWork.Articles
                .Get()
                .AsNoTracking()
                .Where(a => user.Preference.Categories.Contains(a.Category) &&
                            user.Preference.MinPositivityRating.Value >= a.Rating.Value)
                .Select(a => _mapper.Map<ArticlePreviewDTO>(a));
        }

        public Task<IEnumerable<ArticlePreviewDTO>> GetByFilter(ArticleFilterDTO filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PositivityRatingDTO>> GetAllPositivityRatings()
        {
            var ratings = await _unitOfWork.PositivityRatings.GetAllAsync();
            return ratings.Select(r => _mapper.Map<PositivityRatingDTO>(r));
        }

        public async Task<IEnumerable<SourceDTO>> GetAllSources()
        {
            var sources = await _unitOfWork.Sources.GetAllAsync();
            return sources.Select(source => _mapper.Map<SourceDTO>(source));
        }

        public async Task<IEnumerable<ArticleDTO>> GetArticleDTOsByPage(int page, int pageSize)
        {
            return await _unitOfWork.Articles
                .Get()
                .AsNoTracking()
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Rating)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<ArticleDTO>(art))
                .ToListAsync();
        }
    }
}
