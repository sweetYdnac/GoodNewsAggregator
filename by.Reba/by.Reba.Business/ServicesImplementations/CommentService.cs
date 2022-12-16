using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.Tree;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using by.Reba.DataBase.Helpers;
using MediatR;

namespace by.Reba.Business.ServicesImplementations
{
    public class CommentService : ICommentService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CommentService(IMediator mediator, IMapper mapper) => (_mediator, _mapper) = (mediator, mapper);

        public async Task CreateAsync(CreateCommentDTO dto)
        {
            var entity = _mapper.Map<T_Comment>(dto);

            if (entity is null)
            {
                throw new ArgumentException("Cannot map CreateCommentDTO to T_Comment", nameof(dto));
            }

            await _mediator.Send(new AddCommentCommand() { Comment = entity });
        }

        public async Task<Guid?> GetAuthorIdAsync(Guid id) =>
            await _mediator.Send(new GetAuthorIdByCommentIdQuery() { Id = id });

        public async Task<CommentShortSummaryDTO> GetByIdAsync(Guid id)
        {
            var entity = await _mediator.Send(new GetShortSummaryCommentByIdQuery() { Id = id });

            return entity is null
                ? throw new ArgumentException($"Comment with id = {id} is not exist.", nameof(id))
                : _mapper.Map<CommentShortSummaryDTO>(entity);
        }

        public async Task<IEnumerable<ITree<CommentDTO>>> GetCommentsTreesByArticleIdAsync(Guid articleId)
        {
            var articleComments = await _mediator.Send(new GetCommentsByArticleIdQuery() { ArticleId = articleId });

            if (!articleComments.Any())
            {
                return Enumerable.Empty<ITree<CommentDTO>>();
            }

            var comments = articleComments.Select(c => _mapper.Map<CommentDTO>(c)).ToList();
            var tree = comments?.ToTree((parent, child) => child.ParentCommentId == parent.Id);

            return tree.OrderByDescending(c => c.Data.CreationTime).Children;
        }

        public async Task RateAsync(RateEntityDTO dto)
        {
            var comment = await _mediator.Send(new GetTrackedCommentByIdWithAssessmentQuery() { Id = dto.Id });

            if (comment is null)
            {
                throw new ArgumentException($"Comment with id = {dto.Id} is not exist", nameof(dto));
            }

            var user = await _mediator.Send(new GetNoTrackedUserByIdQuery() { Id = dto.AuthorId });

            if (user is null)
            {
                throw new ArgumentException($"User with id = {dto.AuthorId} is not exist", nameof(dto));
            }

            var patchList = comment.CreateRatePatchList(dto, user);

            await _mediator.Send(new PatchCommentCommand()
            {
                Id = dto.Id,
                PatchData = patchList
            });
        }

        public async Task UpdateAsync(Guid id, EditCommentDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto), "EditCommentDTO is null");
            }

            var entity = await _mediator.Send(new GetNoTrackedCommentByIdQuery() { Id = id });

            if (entity is null)
            {
                throw new ArgumentException($"Comment with id = {id} isn't exist", nameof(id));
            }

            var patchList = new List<PatchModel>();

            if (!dto.Content.Equals(entity.Content))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Content),
                    PropertyValue = dto.Content,
                });
            }

            await _mediator.Send(new PatchCommentCommand()
            {
                Id = dto.Id,
                PatchData = patchList
            });
        }
    }
}
