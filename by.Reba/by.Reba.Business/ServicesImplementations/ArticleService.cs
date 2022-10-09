using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;
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

        public async Task<IEnumerable<ArticlePreviewDTO>> GetByPageAsync(int page, int pageSize)
        {
            return await _unitOfWork.Articles.Get()
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<ArticlePreviewDTO>(art))
                .ToListAsync();
        }

        //public async Task<IEnumerable<ArticleDTO>> GetArticleDTOsByPage(int page, int pageSize)
        //{
        //    return await _unitOfWork.Articles
        //        .Get()
        //        .Include(a => a.Category)
        //        .Include(a => a.Source)
        //        .Include(a => a.Rating)
        //        .AsNoTracking()
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(art => _mapper.Map<ArticleDTO>(art))
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<ArticleDTO>> GetFilteredAndOrderedByPageAsync(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortType, string searchString)
        {
            var rating = await _unitOfWork.PositivityRatings.GetByIdAsync(filter.MinPositivityRating);

            var articles = _unitOfWork.Articles
                .FindBy(a => filter.Categories.Contains(a.Category.Id), a => a.Category, a => a.Rating, a => a.Source)
                .AsNoTracking()
                .Where(a => filter.Sources.Contains(a.Source.Id))
                .Where(a => a.PublicationDate >= filter.From && a.PublicationDate <= filter.To)
                .Where(a => a.Rating.Value >= rating.Value);
                

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

        public async Task<ArticleFilterDTO> SetDefaultFilterAsync(ArticleFilterDTO filter)
        {
            if (filter.Categories.Count() == 0)
            {
                filter.Categories = await _unitOfWork.Categories
                    .Get()
                    .AsNoTracking()
                    .Select(c => c.Id)
                    .ToListAsync();
            }

            if (filter.From.Equals(default))
            {
                filter.From = DateTime.Now - TimeSpan.FromDays(100);
            }

            if (filter.To.Equals(default))
            {
                filter.To = DateTime.Now;
            }

            if (filter.MinPositivityRating.Equals(default))
            {
                filter.MinPositivityRating = await _unitOfWork.PositivityRatings
                    .Get()
                    .AsNoTracking()
                    .OrderBy(r => r.Value)
                    .Select(r => r.Id)
                    .FirstAsync();
            }

            if (filter.Sources.Count() == 0)
            {
                filter.Sources = await _unitOfWork.Sources
                    .Get()
                    .AsNoTracking()
                    .Select(s => s.Id)
                    .ToListAsync();
            }

            return filter;
        }
    }
}
