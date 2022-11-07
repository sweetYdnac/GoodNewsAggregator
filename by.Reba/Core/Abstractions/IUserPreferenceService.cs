namespace by.Reba.Core.Abstractions
{
    public interface IUserPreferenceService
    {
        Task<int> CreateDefaultUserPreferenceAsync(Guid userId);
    }
}
