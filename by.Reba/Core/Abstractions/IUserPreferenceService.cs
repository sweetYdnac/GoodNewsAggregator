namespace by.Reba.Core.Abstractions
{
    public interface IUserPreferenceService
    {
        Task CreateDefaultUserPreferenceAsync(Guid userId);
    }
}
