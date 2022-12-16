using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.SortTypes;
using by.Reba.Core.Tree;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using by.Reba.DataBase.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static by.Reba.Core.Tree.TreeExtensions;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ArticleService(IMapper mapper, IMediator mediator) => 
            (_mapper, _mediator) = (mapper, mediator);

        public async Task CreateAsync(CreateOrEditArticleDTO dto)
        {
            var entity = _mapper.Map<T_Article>(dto);

            if (entity is null)
            {
                throw new ArgumentException("Cannot map CreateOrEditArticleDTO to T_Article", nameof(dto));
            }

            await _mediator.Send(new AddArticleCommand() { Article = entity });
        }

        public async Task<ArticleDTO> GetByIdAsync(Guid id)
        {
            var article = await _mediator.Send(new GetArticleDetailsByIdQuery() { Id = id });

            return article is null
                ? throw new ArgumentException($"Article with id = {id} is not exist.", nameof(id))
                : _mapper.Map<ArticleDTO>(article);
        }

        public async Task<ArticleDTO> GetWithCommentsByIdAsync(Guid id)
        {
            var article = await _mediator.Send(new GetArticleDetailsByIdWithCommentsQuery() { Id = id });

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

        public async Task<int> GetTotalCountAsync(ArticleFilterDTO filter, string searchString)
        {
            var articles = await GetAllByFilter(filter);
            FindBySearchString(ref articles, searchString);
            return await articles.CountAsync();
        }

        private async Task<IQueryable<T_Article>> GetAllByFilter(ArticleFilterDTO filter)
        {
            return await _mediator.Send(_mapper.Map<GetArticlesQueryByFilterQuery>(filter));
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

        public async Task RateAsync(RateEntityDTO dto)
        {
            var article = await _mediator.Send(new GetTrackedArticleByIdWithAssessmentQuery() { Id = dto.Id });

            if (article is null)
            {
                throw new ArgumentException($"Article with id = {dto.Id} isn't exist", nameof(dto));
            }

            var user = await _mediator.Send(new GetNoTrackedUserByIdQuery() { Id = dto.Id });

            if (user is null)
            {
                throw new ArgumentException($"User with id = {dto.AuthorId} isn't exist", nameof(dto));
            }

            var patchList = article.CreateRatePatchList(dto, user);

            await _mediator.Send(new PatchArticleCommand()
            {
                Id = dto.Id,
                PatchData = patchList
            });
        }

        public async Task UpdateAsync(Guid id, CreateOrEditArticleDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto), $"CreateOrEditArticleDTO is null");
            }

            var entity = await _mediator.Send(new GetNoTrackedArticleByIdQuery() { Id = id });

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

            if (dto.HtmlContent is null || !dto.HtmlContent.Equals(entity.HtmlContent))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.HtmlContent),
                    PropertyValue = dto.HtmlContent,
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

            if (!dto.PositivityId.Equals(entity.PositivityId))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.PositivityId),
                    PropertyValue = dto.PositivityId,
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

            await _mediator.Send(new PatchArticleCommand()
            {
                Id = id,
                PatchData = patchList
            });
        }

        public async Task<CreateOrEditArticleDTO> GetEditArticleDTOByIdAsync(Guid id)
        {
            var article = await _mediator.Send(new GetArticleEditByIdQuery() { Id = id });

            return article is null
                ? throw new ArgumentException($"Article with id = {id} is not exist.", nameof(id))
                : _mapper.Map<CreateOrEditArticleDTO>(article);
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await _mediator.Send(new GetNoTrackedArticleByIdQuery() { Id = id });

            if (entity is null)
            {
                throw new ArgumentException($"Article with id = {id} isn't exist", nameof(id));
            }

            await _mediator.Send(new RemoveArticleCommand() { Article = entity });
        } 
    }
}
