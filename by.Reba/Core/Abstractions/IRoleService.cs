namespace by.Reba.Core.Abstractions
{
    public interface IRoleService
    {
        Task<Guid?> GetRoleIdByNameAsync(string name);
        Task<string> GetRoleNameByIdAsync(Guid id);
        Task<bool> IsAdminAsync(string email);
    }
}
