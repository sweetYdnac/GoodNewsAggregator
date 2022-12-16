using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class AddCategoryCommand : IRequest
    {
        public T_Category Category { get; set; }
    }
}
