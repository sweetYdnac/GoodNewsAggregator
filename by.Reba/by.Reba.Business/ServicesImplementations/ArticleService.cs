using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;
using by.Reba.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPositivityRatingService _positivityRatingService;

        public ArticleService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IPositivityRatingService positivityRatingService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _positivityRatingService = positivityRatingService;
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

        public async Task<IEnumerable<ArticleDTO>> GetArticleDTOsByPage(int page, int pageSize)
        {
            return await _unitOfWork.Articles
                .Get()
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Rating)
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<ArticleDTO>(art))
                .ToListAsync();
        }

        public async Task<IEnumerable<ArticleDTO>> GetFilteredAndOrderedByPage(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortType, string searchString)
        {
            //var rating = await _unitOfWork.PositivityRatings.GetByIdAsync(filter.MinPositivityRating);

            var articles = _unitOfWork.Articles
                .FindBy(a => filter.Categories.Contains(a.Category.Id), a => a.Category, a => a.Rating, a => a.Source)
                .AsNoTracking()
                .Where(a => filter.Sources.Contains(a.Source.Id))
                .Where(a => a.PublicationDate >= filter.From && a.PublicationDate <= filter.To);
                //.Where(a => a.Rating.Value >= rating.Value)
                

            if (!string.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(a => a.Title.Contains(searchString));
            }

            var ordered = sortType switch
            {
                ArticleSort.Positivity => articles.OrderBy(a => a.Rating),
                ArticleSort.PublicationDate => articles.OrderBy(a => a.PublicationDate),
                ArticleSort.Comments => articles.OrderBy(a => a.Comments),
                ArticleSort.Likes => articles.OrderBy(a => a.Assessment),
                _ => articles.OrderBy(a => a.PublicationDate)
            };

            var res = await ordered.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<ArticleDTO>(art))
                .ToListAsync();

            return res;
        }
    }
}
