using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

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

        public Task<List<ArticlePreviewDTO>> GetByPage(IQueryable<ArticlePreviewDTO> filteredArticles)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ArticlePreviewDTO> GetByFilter(ArticleFilterDTO filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ArticlePreviewDTO> FilterByAssessment(IQueryable<ArticlePreviewDTO> filteredArticles)
        {
            return filteredArticles.OrderByDescending(a => a.Assessment);
        }

        public IQueryable<ArticlePreviewDTO> FilterByCommentsCount(IQueryable<ArticlePreviewDTO> filteredArticles)
        {
            return filteredArticles.OrderByDescending(a => a.CommentsCount);
        }

        public IQueryable<ArticlePreviewDTO> FilterByPositivityRating(IQueryable<ArticlePreviewDTO> filteredArticles)
        {
            // TODO: что-то не так, постоянно лезем в бд
            return filteredArticles.OrderByDescending(a => _db.PositivityRatings.AsNoTracking().FirstOrDefault(r => r.Id == a.RatingId).Value);
        }

        public IQueryable<ArticlePreviewDTO> FilterByPublicationDate(IQueryable<ArticlePreviewDTO> filteredArticles)
        {
            return filteredArticles.OrderByDescending(a => a.PublicationDate);
        }

        public IQueryable<ArticlePreviewDTO> GetUserPrefered(IQueryable<ArticlePreviewDTO> filteredArticles)
        {
            // TODO: выбрать у текущего юзера аартиклы по категориям и рейтингу позитивности
            throw new NotImplementedException();
        }
    }
}
