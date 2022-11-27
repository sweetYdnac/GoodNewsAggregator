using by.Reba.Core.Abstractions;
using by.Reba.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace by.Reba.Business.ServicesImplementations
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public RoleService(IUnitOfWork unitOfWork, IConfiguration configuration) => (_unitOfWork, _configuration) = (unitOfWork, configuration);

        public async Task<Guid?> GetRoleIdByNameAsync(string name)
        {
            var role = await _unitOfWork.Roles.FindBy(r => r.Name.Equals(name))
                .FirstOrDefaultAsync();

            return role?.Id;
        }

        public async Task<string> GetRoleNameByIdAsync(Guid id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);

            return role == null
                ? string.Empty
                : role.Name;
        }

        public async Task<bool> IsAdminAsync(string? email)
        {
            var res = await _unitOfWork.Users
                .FindBy(u => u.Email.Equals(email), u => u.Role)
                .FirstOrDefaultAsync();

            return res is not null && res.Role.Name.Equals(_configuration["Roles:Admin"]);
        }
    }
}
