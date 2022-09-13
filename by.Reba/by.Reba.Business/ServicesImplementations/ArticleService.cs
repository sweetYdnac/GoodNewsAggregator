using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly RebaDbContext _db;
        private readonly IMapper _mapper;

        public ArticleService(
            RebaDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task<List<ArticlePreviewDTO>> GetByPage(int page, int countOnPage, ArticleFilterDTO filter)
        {
            return null;
        }

        public async Task<IQueryable<ArticlePreviewDTO>> GetUserPrefered(Guid userId)
        {
            var user = await _db.Users.AsNoTracking()
                                      .Include(u => u.Preference)
                                      .ThenInclude(p => p.MinPositivityRating)
                                      .Where(u => u.Id.Equals(userId))
                                      .FirstOrDefaultAsync();

            return _db.Articles.AsNoTracking()
                               .Where(a => user.Preference.Categories.Contains(a.Category) &&
                                           user.Preference.MinPositivityRating.Value >= a.Rating.Value)
                               .Select(a => _mapper.Map<ArticlePreviewDTO>(a));
        }
    }
}
