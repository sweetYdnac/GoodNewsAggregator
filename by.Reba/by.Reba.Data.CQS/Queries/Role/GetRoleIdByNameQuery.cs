using MediatR;

namespace by.Reba.Data.CQS.Queries.Role
{
    public class GetRoleIdByNameQuery : IRequest<Guid?>
    {
        public string Name { get; set; }
    }
}
