using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserPreferenceService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateDefaultUserPreferenceAsync(Guid userId)
        {
            var entity = new T_UserPreference()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PositivityRatingId = await _unitOfWork.PositivityRatings
                                            .Get()
                                            .AsNoTracking()
                                            .OrderBy(r => r.Value)
                                            .Select(r => r.Id)
                                            .FirstAsync(),
                Categories = await _unitOfWork.Categories
                                            .Get()
                                            .AsNoTracking()
                                            .ToListAsync(),
            };

            await _unitOfWork.UsersPreference.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
    }
}
