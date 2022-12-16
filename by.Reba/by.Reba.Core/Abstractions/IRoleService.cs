namespace by.Reba.Core.Abstractions
{
    public interface IRoleService
    {
        Task<Guid?> GetRoleIdByNameAsync(string name);
        Task<bool> IsAdminAsync(string? email);
    }
}
