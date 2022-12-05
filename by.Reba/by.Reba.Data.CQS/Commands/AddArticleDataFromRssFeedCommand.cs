using by.Reba.Core.DataTransferObjects.Article;
using MediatR;

namespace by.Reba.Data.CQS.Commands
{
    public class AddArticleDataFromRssFeedCommand : IRequest
    {
        public IEnumerable<ArticleDTO>? Articles;
    }
}
