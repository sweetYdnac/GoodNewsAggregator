using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class AddPreferenceCommand : IRequest
    {
        public T_Preference Preference { get; set; }
    }
}
