using by.Reba.Core.Abstractions;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class HistoryService : IHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HistoryService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<int> AddOrUpdateArticleInHistoryAsync(Guid articleId, string userEmail)
        {
            var user = await _unitOfWork.Users
                .Get()
                .Include(u => u.History)
                .FirstOrDefaultAsync(u => u.Email.Equals(userEmail));

            if (user is null)
            {
                throw new ArgumentException($"User with email = {userEmail} is not exist", nameof(userEmail));
            }

            var lastVisitedArticle = user.History
                .FirstOrDefault(h => h.ArticleId.Equals(articleId)
                                     && DateTime.Now.Day.Equals(h.LastVisitTime.Day));

            if (lastVisitedArticle is null)
            {
                lastVisitedArticle = new T_History()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    ArticleId = articleId,
                    LastVisitTime = DateTime.Now,
                };

                await _unitOfWork.Histories.AddAsync(lastVisitedArticle);
            }
            else
            {
                lastVisitedArticle.LastVisitTime = DateTime.Now;
            }

            return await _unitOfWork.Commit();
        }
    }
}
