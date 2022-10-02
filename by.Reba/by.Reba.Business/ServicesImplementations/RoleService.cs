using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid?> GetRoleIdByNameAsync(string name)
        {
            var role = await _unitOfWork.Roles.FindBy(r => r.Name.Equals(name))
                .FirstOrDefaultAsync();

            return role?.Id;
        }

        public async Task<string> GetRoleNameByIdAsync(Guid id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);

            return role != null
                ? role.Name
                : string.Empty;
        }
    }
}
