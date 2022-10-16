using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
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

        public async Task<int> CreateAsync(CreateArticleDTO dto)
        {
            var entity = _mapper.Map<T_Article>(dto);

            if (entity is not null)
            {
                await _unitOfWork.Articles.AddAsync(entity);
                var result = await _unitOfWork.Commit();
                return result;
            }
            else
            {
                throw new ArgumentException(nameof(dto));
            }
        }

        public async Task<ArticleDTO> GetByIdAsync(Guid id)
        {
            var article = await _unitOfWork.Articles
                .Get()
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Rating)
                .Include(a => a.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(id));

            return _mapper.Map<ArticleDTO>(article);
        }

        public async Task<ArticleDTO> GetWithCommentsByIdAsync(Guid id)
        {
            var article = await _unitOfWork.Articles
                .Get()
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Rating)
                .Include(a => a.Comments.Where(c => c.ParentCommentId == null))
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(id));

            for (int i = 0; i < article.Comments.Count; i++)
            {
                article.Comments[i] = await _unitOfWork.Comments.GetWithInnerTreeByIdAsync(article.Comments[i].Id);
            }

            return _mapper.Map<ArticleDTO>(article);
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

        public async Task<IEnumerable<ArticlePreviewDTO>> GetFilteredAndOrderedByPageAsync(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortType, string searchString)
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
                ArticleSort.Positivity => articles.OrderByDescending(a => a.Rating.Value),
                ArticleSort.PublicationDate => articles.OrderByDescending(a => a.PublicationDate),
                ArticleSort.Comments => articles.OrderByDescending(a => a.Comments.Count),
                ArticleSort.Likes => articles.OrderByDescending(a => a.Assessment),
                _ => articles.OrderBy(a => a.PublicationDate)
            };

            var res = await ordered.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<ArticlePreviewDTO>(art))
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
