using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class RemovePositivityCommand : IRequest
    {
        public T_Positivity Positivity { get; set; }
    }
}
