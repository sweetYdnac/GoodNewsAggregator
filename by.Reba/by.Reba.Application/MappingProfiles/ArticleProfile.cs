using AutoMapper;
using by.Reba.Application.Models.Article;
using by.Reba.Core.DataTransferObjects;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<T_Article, ArticlePreviewDTO>()
                .ForMember(dto => dto.Id,
                           opt => opt.MapFrom(article => article.Id))
                .ForMember(dto => dto.Title,
                           opt => opt.MapFrom(article => article.Title))
                .ForMember(dto => dto.PosterUrl,
                           opt => opt.MapFrom(article => article.PosterUrl))
                .ForMember(dto => dto.PublicationDate,
                           opt => opt.MapFrom(article => article.PublicationDate))
                .ForMember(dto => dto.Assessment,
                           opt => opt.MapFrom(article => article.Assessment))
                .ForMember(dto => dto.CategoryId,
                           opt => opt.MapFrom(article => article.CategoryId))
                .ForMember(dto => dto.RatingId,
                           opt => opt.MapFrom(article => article.RatingId)) 
                .ForMember(dto => dto.CommentsCount, 
                           opt => opt.MapFrom(article => article.Comments.Count));


            CreateMap<T_Category, CategoryDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(category => category.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(category => category.Title));

            CreateMap<ArticleFilterVM, ArticleFilterDTO>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(filter => filter.Categories))
                .ForMember(dto => dto.From, opt => opt.MapFrom(filter => filter.From))
                .ForMember(dto => dto.To, opt => opt.MapFrom(filter => filter.To))
                .ForMember(dto => dto.MinPositivityRating, opt => opt.MapFrom(filter => filter.MinPositivityRating));
        }
    }
}
