using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetArticlesQueryByFilterQuery : IRequest<IQueryable<T_Article>>
    {
        public IList<Guid> CategoriesId { get; set; } = new List<Guid>();
        public DateTime From { get; set; }
        public DateTime To { get; set; } = DateTime.Now;
        public Guid MinPositivity { get; set; }
        public IList<Guid> SourcesId { get; set; } = new List<Guid>();
    }
}
