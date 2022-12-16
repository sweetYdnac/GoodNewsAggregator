using by.Reba.Core.Abstractions;
using by.Reba.Data.CQS.Queries;
using by.Reba.Data.CQS.Queries.Role;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace by.Reba.Business.ServicesImplementations
{
    public class RoleService : IRoleService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public RoleService(IMediator mediator, IConfiguration configuration) => (_mediator, _configuration) = (mediator, configuration);

        public async Task<Guid?> GetRoleIdByNameAsync(string name)
        {
            return await _mediator.Send(new GetRoleIdByNameQuery() { Name = name });
        }

        public async Task<bool> IsAdminAsync(string? email)
        {
            var res = await _mediator.Send(new GetNoTrackedUserWithRoleByEmailQuery() { Email = email });
            return res is not null && res.Role.Name.Equals(_configuration["Roles:Admin"]);
        }
    }
}
