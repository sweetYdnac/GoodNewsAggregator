using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries.Preference
{
    public class GetPreferenceByUserEmailQuery : IRequest<T_Preference?>
    {
        public string Email { get; set; }
    }
}
