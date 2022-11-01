using AutoMapper;
using by.Reba.Business.Helpers;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.SortTypes;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using static by.Reba.Core.TreeExtensions;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper) =>
            (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<int> CreateAsync(CreateOrEditArticleDTO dto)
        {
            var entity = _mapper.Map<T_Article>(dto);

            if (entity is null)
            {
                throw new ArgumentException(nameof(dto));
            }

            await _unitOfWork.Articles.AddAsync(entity);
            var result = await _unitOfWork.Commit();
            return result;
        }

        public async Task<ArticleDTO> GetByIdAsync(Guid id)
        {
            var article = await _unitOfWork.Articles
                .Get()
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Rating)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(id));

            return article is null
                ? throw new ArgumentException($"Article with id = {id} is not exist.", nameof(id))
                : _mapper.Map<ArticleDTO>(article);
        }

        public async Task<ArticleDTO> GetWithCommentsByIdAsync(Guid id)
        {
            var article = await _unitOfWork.Articles
                .Get()
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Rating)
                .Include(a => a.UsersWithPositiveAssessment)
                .Include(a => a.UsersWithNegativeAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.ParentComment)
                .Include(a => a.Comments).ThenInclude(c => c.UsersWithPositiveAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.UsersWithNegativeAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.Author)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(a => a.Id.Equals(id));

            if (article is null)
            {
                throw new ArgumentException($"Article with id = {id} is not exist.", nameof(id));
            }

            var articleDTO = _mapper.Map<ArticleDTO>(article);

            var comments = article.Comments.Select(c => _mapper.Map<CommentDTO>(c)).ToList();
            var tree = comments?.ToTree((parent, child) => child.ParentCommentId == parent.Id);
            articleDTO.CommentTrees = tree?.OrderByDescending(c => c.Data.CreationTime).Children;

            return articleDTO;
        }

        public async Task<IEnumerable<ArticlePreviewDTO>> GetPreviewsByPageAsync(int page, int pageSize, ArticleFilterDTO filter, ArticleSort sortType, string searchString)
        {
            var articles = await GetAllByFilter(filter);
            FindBySearchString(articles, searchString);
            SortBy(articles, sortType);

            return await articles.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<ArticlePreviewDTO>(art))
                .ToListAsync();
        }

        public async Task<int> GetTotalCount(ArticleFilterDTO filter, string searchString)
        {
            var articles = await GetByFilterAndSearchString(filter, searchString);
            return await articles.CountAsync();
        }

        private async Task<IQueryable<T_Article>> GetByFilterAndSearchString(ArticleFilterDTO filter, string searchString)
        {
            var articles = await GetAllByFilter(filter);
            return FindBySearchString(articles, searchString);
        }

        private async Task<IQueryable<T_Article>> GetAllByFilter(ArticleFilterDTO filter)
        {
            var rating = await _unitOfWork.PositivityRatings.GetByIdAsync(filter.MinPositivityRating);

            var articles = _unitOfWork.Articles
                .FindBy(a => filter.Categories.Contains(a.Category.Id), a => a.Category, a => a.Rating, a => a.Source,
                        a => a.UsersWithPositiveAssessment, a => a.UsersWithNegativeAssessment)
                .AsNoTracking()
                .Where(a => filter.Sources.Contains(a.Source.Id))
                .Where(a => a.PublicationDate >= filter.From && a.PublicationDate <= filter.To)
                .Where(a => rating != null && a.Rating.Value >= rating.Value);

            return articles;
        }

        private static IQueryable<T_Article> FindBySearchString(IQueryable<T_Article> articles, string searchString)
        {
            return string.IsNullOrEmpty(searchString) 
                    ? articles 
                    : (articles = articles.Where(a => a.Title.Contains(searchString)));
        }

        private static IQueryable<T_Article> SortBy(IQueryable<T_Article> articles, ArticleSort sortType)
        {
            return sortType switch
            {
                ArticleSort.Positivity => articles.OrderByDescending(a => a.Rating.Value),
                ArticleSort.PublicationDate => articles.OrderByDescending(a => a.PublicationDate),
                ArticleSort.Comments => articles.OrderByDescending(a => a.Comments.Count),
                ArticleSort.Likes => articles.OrderByDescending(a => a.UsersWithPositiveAssessment.Count() - a.UsersWithNegativeAssessment.Count()),
                _ => articles.OrderBy(a => a.PublicationDate)
            };
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
        public async Task<int> RateAsync(RateEntityDTO dto)
        {
            var article = await _unitOfWork.Articles
                .FindBy(a => a.Id.Equals(dto.Id), a => a.UsersWithPositiveAssessment, a => a.UsersWithNegativeAssessment)
                .FirstOrDefaultAsync();

            if (article is null)
            {
                throw new ArgumentException(nameof(dto));
            }

            var user = await _unitOfWork.Users.GetByIdAsync(dto.AuthorId);

            if (user is null)
            {
                throw new ArgumentException(nameof(dto.AuthorId));
            }

            var patchList = article.CreateRatePatchList(dto, user);

            await _unitOfWork.Articles.PatchAsync(dto.Id, patchList);
            return await _unitOfWork.Commit();
        }

        public async Task<int> UpdateAsync(CreateOrEditArticleDTO dto)
        {
            var entity = _mapper.Map<T_Article>(dto);

            var patchList = new List<PatchModel>();

            if (dto is null)
            {
                throw new ArgumentException(nameof(dto));
            }

            if (!dto.Title.Equals(entity.Title))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Title),
                    PropertyValue = dto.Title,
                });
            }

            if (!dto.Text.Equals(entity.Text))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Text),
                    PropertyValue = dto.Text,
                });
            }

            if (!dto.PosterUrl.Equals(entity.PosterUrl))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.PosterUrl),
                    PropertyValue = dto.PosterUrl,
                });
            }

            if (!dto.CategoryId.Equals(entity.CategoryId))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.CategoryId),
                    PropertyValue = dto.CategoryId,
                });
            }

            if (!dto.RatingId.Equals(entity.RatingId))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.RatingId),
                    PropertyValue = dto.RatingId,
                });
            }

            if (!dto.SourceId.Equals(entity.SourceId))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.SourceId),
                    PropertyValue = dto.SourceId,
                });
            }

            await _unitOfWork.Articles.PatchAsync(dto.Id, patchList);
            return await _unitOfWork.Commit();
        }

        public async Task<CreateOrEditArticleDTO> GetEditArticleDTOByIdAsync(Guid id)
        {
            var article = await _unitOfWork.Articles
                .Get()
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Rating)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(id));

            return article is null
                ? throw new ArgumentException($"Article with id = {id} is not exist.", nameof(id))
                : _mapper.Map<CreateOrEditArticleDTO>(article);
        }
    }
}
