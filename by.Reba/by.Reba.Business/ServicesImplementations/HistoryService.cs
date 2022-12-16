using by.Reba.Core.Abstractions;
using by.Reba.Data.CQS.Commands;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries.History;
using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Business.ServicesImplementations
{
    public class HistoryService : IHistoryService
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;
        public HistoryService(IMediator mediator, IUserService userService) => 
            (_userService, _mediator) = (userService, mediator);

        public async Task AddOrUpdateArticleInHistoryAsync(Guid articleId, string userEmail)
        {
            var lastVisitedArticleHistory = await _mediator.Send(new GetLastVisitedArticleHistoryQuery()
            {
                ArticleId = articleId,
                UserEmail = userEmail
            });

            if (lastVisitedArticleHistory is null)
            {
                var userId = await _userService.GetIdByEmailAsync(userEmail);

                lastVisitedArticleHistory = new T_History()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ArticleId = articleId,
                    LastVisitTime = DateTime.Now,
                };

                await _mediator.Send(new AddHistoryCommand() { History = lastVisitedArticleHistory });
            }
            else
            {
                lastVisitedArticleHistory.LastVisitTime = DateTime.Now;
            }

            await _mediator.Send(new SaveChangesCommand());
        }
    }
}
