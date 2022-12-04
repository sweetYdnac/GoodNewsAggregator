using AutoMapper;
using by.Reba.Business.Helpers;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.SortTypes;
using by.Reba.Core.Tree;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using static by.Reba.Core.Tree.TreeExtensions;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork,IMapper mapper) => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<int> CreateAsync(CreateOrEditArticleDTO dto)
        {
            var entity = _mapper.Map<T_Article>(dto);

            if (entity is null)
            {
                throw new ArgumentException("Cannot map CreateOrEditArticleDTO to T_Article", nameof(dto));
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
                .Include(a => a.Positivity)
                .Where(a => !string.IsNullOrEmpty(a.HtmlContent) && !a.PositivityId.Equals(null))
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
                .Include(a => a.Positivity)
                .Include(a => a.UsersWithPositiveAssessment)
                .Include(a => a.UsersWithNegativeAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.ParentComment)
                .Include(a => a.Comments).ThenInclude(c => c.UsersWithPositiveAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.UsersWithNegativeAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.Author)
                .Where(a => !string.IsNullOrEmpty(a.HtmlContent) && !a.PositivityId.Equals(null))
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
                .ToArrayAsync();
        }

        public async Task<int> GetTotalCount(ArticleFilterDTO filter, string searchString)
        {
            var articles = await GetAllByFilter(filter);
            FindBySearchString(ref articles, searchString);
            return await articles.CountAsync();
        }

        private async Task<IQueryable<T_Article>> GetAllByFilter(ArticleFilterDTO filter)
        {
            var rating = await _unitOfWork.Positivities.GetByIdAsync(filter.MinPositivity);

            var articles = _unitOfWork.Articles
                .FindBy(a => filter.CategoriesId.Contains(a.Category.Id) && !string.IsNullOrEmpty(a.HtmlContent),
                        a => a.Category, a => a.Positivity, a => a.Source, a => a.UsersWithPositiveAssessment, a => a.UsersWithNegativeAssessment, a => a.Comments)
                .AsNoTracking()
                .Where(a => filter.SourcesId.Contains(a.Source.Id))
                .Where(a => a.PublicationDate >= filter.From && a.PublicationDate <= filter.To)
                .Where(a => rating != null && a.Positivity != null && a.Positivity.Value >= rating.Value);

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

        private static IQueryable<T_Article> SortBy(ref IQueryable<T_Article> articles, ArticleSort sortOrder)
        {
            return sortOrder switch
            {
                ArticleSort.Positivity => articles = articles.OrderByDescending(a => a.Positivity.Value).ThenByDescending(a => a.PublicationDate),
                ArticleSort.PublicationDate => articles = articles.OrderByDescending(a => a.PublicationDate),
                ArticleSort.Comments => articles = articles.OrderByDescending(a => a.Comments.Count).ThenByDescending(a => a.PublicationDate),
                ArticleSort.Likes => articles = articles.OrderByDescending(a => a.UsersWithPositiveAssessment.Count() - a.UsersWithNegativeAssessment.Count())
                                                        .ThenByDescending(a => a.PublicationDate),
                _ => articles = articles.OrderByDescending(a => a.PublicationDate)
            };
        }

        public async Task SetDefaultFilterAsync(ArticleFilterDTO filter)
        {
            await SetDefaultDatesAndSources(filter);

            if (filter.CategoriesId.Count == 0)
            {
                filter.CategoriesId = await _unitOfWork.Categories
                    .Get()
                    .AsNoTracking()
                    .Select(c => c.Id)
                    .ToArrayAsync();
            }

            if (filter.MinPositivity.Equals(default))
            {
                filter.MinPositivity = await _unitOfWork.Positivities
                    .Get()
                    .AsNoTracking()
                    .OrderBy(r => r.Value)
                    .Select(r => r.Id)
                    .FirstAsync();
            }
        }
        public async Task<int> RateAsync(RateEntityDTO dto)
        {
            var article = await _unitOfWork.Articles
                .FindBy(a => a.Id.Equals(dto.Id), a => a.UsersWithPositiveAssessment, a => a.UsersWithNegativeAssessment)
                .FirstOrDefaultAsync();

            if (article is null)
            {
                throw new ArgumentException($"Article with id = {dto.Id} isn't exist", nameof(dto));
            }

            var user = await _unitOfWork.Users.GetByIdAsync(dto.AuthorId);

            if (user is null)
            {
                throw new ArgumentException($"User with id = {dto.AuthorId} isn't exist", nameof(dto));
            }

            var patchList = article.CreateRatePatchList(dto, user);

            await _unitOfWork.Articles.PatchAsync(dto.Id, patchList);
            return await _unitOfWork.Commit();
        }

        public async Task<int> UpdateAsync(Guid id, CreateOrEditArticleDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto), $"CreateOrEditArticleDTO is null");
            }

            var entity = await _unitOfWork.Articles.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentException($"Article with id = {id} isn't exist", nameof(id));
            }

            var patchList = new List<PatchModel>();

            if (!dto.Title.Equals(entity.Title))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Title),
                    PropertyValue = dto.Title,
                });
            }

            if (dto.Text is null || !dto.Text.Equals(entity.HtmlContent))
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

            if (!dto.RatingId.Equals(entity.PositivityId))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.RatingId),
                    PropertyValue = dto.RatingId,
                });
            }

            if (!dto.SourceUrl.Equals(entity.SourceUrl))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.SourceUrl),
                    PropertyValue = dto.SourceUrl,
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
                .Include(a => a.Positivity)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(id));

            return article is null
                ? throw new ArgumentException($"Article with id = {id} is not exist.", nameof(id))
                : _mapper.Map<CreateOrEditArticleDTO>(article);
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var entity = await _unitOfWork.Articles.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentException($"Article with id = {id} isn't exist", nameof(id));
            }

            _unitOfWork.Articles.Remove(entity);
            return await _unitOfWork.Commit();
        }

        public async Task SetPreferenceInFilterAsync(Guid userId, ArticleFilterDTO filter)
        {
            var userPreference = await _unitOfWork.Preferences
                .Get()
                .Include(up => up.Categories)
                .AsNoTracking()
                .FirstOrDefaultAsync(up => up.UserId.Equals(userId));

            if (userPreference is null)
            {
                throw new ArgumentException($"User with id = {userId} doesn't have T_Preference", nameof(userId));
            }

            await SetDefaultDatesAndSources(filter);

            filter.CategoriesId = userPreference.Categories.Select(c => c.Id).ToList();
            filter.MinPositivity = userPreference.MinPositivityId;
        }

        private async Task SetDefaultDatesAndSources(ArticleFilterDTO filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter), "ArticleFilterDTO is null");
            }

            if (filter.From.Equals(default))
            {
                filter.From = DateTime.Now - TimeSpan.FromDays(100);
            }

            if (filter.To.Equals(default))
            {
                filter.To = DateTime.Now;
            }

            if (filter.SourcesId.Count == 0)
            {
                filter.SourcesId = await _unitOfWork.Sources
                    .Get()
                    .AsNoTracking()
                    .Select(s => s.Id)
                    .ToArrayAsync();
            }
        }
    }
}
