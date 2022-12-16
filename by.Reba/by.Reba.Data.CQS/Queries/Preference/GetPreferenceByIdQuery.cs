using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries.Preference
{
    public class GetPreferenceByIdQuery : IRequest<T_Preference?>
    {
        public Guid Id { get; set; }
    }
}
