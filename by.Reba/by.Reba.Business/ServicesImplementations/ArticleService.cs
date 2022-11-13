using AutoMapper;
using by.Reba.Business.Helpers;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.SortTypes;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using static by.Reba.Core.TreeExtensions;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ArticleService
            (IUnitOfWork unitOfWork,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateOrEditArticleDTO dto)
        {
            var entity = _mapper.Map<T_Article>(dto);

            // TODO: ТОЛЬКО ДЛЯ ТЕСТОВ. НЕ ЗАБЫТЬ УБРАТЬ!!!!!!!!
            entity.RatingId = new Guid("736A0895-E7F1-40DE-AF35-5E1A2A359ED9");

            if (entity is null)
            {
                throw new ArgumentException(nameof(dto));
            }

            await _unitOfWork.Articles.AddAsync(entity);
            return await _unitOfWork.Commit();
        }

        public async Task<ArticleDTO> GetByIdAsync(Guid id)
        {
            var article = await _unitOfWork.Articles
                .Get()
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Rating)
                .Where(a => !string.IsNullOrEmpty(a.Text) && !a.RatingId.Equals(null))
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
                .Where(a => !string.IsNullOrEmpty(a.Text) && !a.RatingId.Equals(null))
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
            FindBySearchString(ref articles, searchString);
            SortBy(ref articles, sortType);

            return await articles.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<ArticlePreviewDTO>(art))
                .ToListAsync();
        }

        public async Task<int> GetTotalCount(ArticleFilterDTO filter, string searchString)
        {
            var articles = await GetAllByFilter(filter);
            articles = FindBySearchString(ref articles, searchString);
            return await articles.CountAsync();
        }

        private async Task<IQueryable<T_Article>> GetAllByFilter(ArticleFilterDTO filter)
        {
            var rating = await _unitOfWork.PositivityRatings.GetByIdAsync(filter.MinPositivityRating);

            var articles = _unitOfWork.Articles
                .FindBy(a => filter.Categories.Contains(a.Category.Id) && !string.IsNullOrEmpty(a.Text),
                        a => a.Category, a => a.Rating, a => a.Source, a => a.UsersWithPositiveAssessment, a => a.UsersWithNegativeAssessment)
                .AsNoTracking()
                .Where(a => filter.Sources.Contains(a.Source.Id))
                .Where(a => a.PublicationDate >= filter.From && a.PublicationDate <= filter.To)
                .Where(a => rating != null && a.Rating != null && a.Rating.Value >= rating.Value);

            var test = await articles.ToListAsync();

            return articles;
        }

        private static IQueryable<T_Article> FindBySearchString(ref IQueryable<T_Article> articles, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(a => a.Title.Contains(searchString));
            }

            return articles;
        }

        private static IQueryable<T_Article> SortBy(ref IQueryable<T_Article> articles, ArticleSort sortType)
        {
            return sortType switch
            {
                ArticleSort.Positivity => articles = articles.OrderByDescending(a => a.Rating.Value).ThenByDescending(a => a.PublicationDate),
                ArticleSort.PublicationDate => articles = articles.OrderByDescending(a => a.PublicationDate),
                ArticleSort.Comments => articles = articles.OrderByDescending(a => a.Comments.Count).ThenByDescending(a => a.PublicationDate),
                ArticleSort.Likes => articles = articles.OrderByDescending(a => a.UsersWithPositiveAssessment.Count() - a.UsersWithNegativeAssessment.Count())
                                                        .ThenByDescending(a => a.PublicationDate),
                _ => articles = articles.OrderByDescending(a => a.PublicationDate)
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

        public async Task<int> UpdateAsync(Guid id, CreateOrEditArticleDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentException(nameof(dto));
            }

            var entity = await _unitOfWork.Articles.GetByIdAsync(id);

            var patchList = new List<PatchModel>();

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

            if (!dto.SourceId.Equals(entity.SourceId))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.SourceId),
                    PropertyValue = dto.SourceId,
                });
            }

            await _unitOfWork.Articles.PatchAsync(id, patchList);
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

        public async Task RemoveAsync(Guid id)
        {
            var entity = await _unitOfWork.Articles.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentException(nameof(id));
            }

            _unitOfWork.Articles.Remove(entity);
            await _unitOfWork.Commit();
        }
    }
}
