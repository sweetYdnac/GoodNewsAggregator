using AutoMapper;
using by.Reba.Application.Models.Article;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<T_Article, ArticleDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(article => article.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(article => article.Title))
                .ForMember(dto => dto.Text, opt => opt.MapFrom(article => article.Text))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(article => article.PosterUrl))
                .ForMember(dto => dto.PublicationDate, opt => opt.MapFrom(article => article.PublicationDate))
                .ForMember(dto => dto.CategoryTitle, opt => opt.MapFrom(article => article.Category.Title))
                .ForMember(dto => dto.RatingTitle, opt => opt.MapFrom(article => article.Rating.Title))
                .ForMember(dto => dto.Source, opt => opt.MapFrom(article => article.Source))
                .ForMember(dto => dto.Comments, opt => opt.MapFrom(article => article.Comments));

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
                .ForMember(dto => dto.CategoryName,
                           opt => opt.MapFrom(article => article.Category.Title))
                .ForMember(dto => dto.RatingName,
                           opt => opt.MapFrom(article => article.Rating.Title))
                .ForMember(dto => dto.CommentsCount,
                           opt => opt.MapFrom(article => article.Comments.Count))
                .ForMember(dto => dto.SourceName,
                           opt => opt.MapFrom(article => article.Source.Name));

            CreateMap<ArticleFilterVM, ArticleFilterDTO>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(filter => filter.Categories))
                .ForMember(dto => dto.From, opt => opt.MapFrom(filter => filter.From))
                .ForMember(dto => dto.To, opt => opt.MapFrom(filter => filter.To))
                .ForMember(dto => dto.MinPositivityRating, opt => opt.MapFrom(filter => filter.MinPositivityRating))
                .ForMember(dto => dto.Sources, opt => opt.MapFrom(filter => filter.Sources));

            CreateMap<CreateArticleVM, CreateArticleDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => Guid.NewGuid()))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(dto => dto.Text, opt => opt.MapFrom(model => model.Text))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(model => model.PosterUrl))
                .ForMember(dto => dto.CategoryId, opt => opt.MapFrom(model => model.CategoryId))
                .ForMember(dto => dto.RatingId, opt => opt.MapFrom(model => model.RatingId))
                .ForMember(dto => dto.SourceId, opt => opt.MapFrom(model => model.SourceId));

            CreateMap<CreateArticleDTO, T_Article>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(dto => dto.Text, opt => opt.MapFrom(model => model.Text))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(model => model.PosterUrl))
                .ForMember(dto => dto.CategoryId, opt => opt.MapFrom(model => model.CategoryId))
                .ForMember(dto => dto.RatingId, opt => opt.MapFrom(model => model.RatingId))
                .ForMember(dto => dto.SourceId, opt => opt.MapFrom(model => model.SourceId))
                .ForMember(dto => dto.PublicationDate, opt => opt.MapFrom(model => DateTime.Now));

            CreateMap<ArticleDTO, ArticleDetailsVM>()
                .ForMember(dto => dto.Title,
                           opt => opt.MapFrom(article => article.Title))
                .ForMember(dto => dto.Text,
                           opt => opt.MapFrom(article => article.Text))
                .ForMember(dto => dto.PosterUrl,
                           opt => opt.MapFrom(article => article.PosterUrl))
                .ForMember(dto => dto.PublicationDate,
                           opt => opt.MapFrom(article => article.PublicationDate))
                .ForMember(dto => dto.CategoryTitle,
                           opt => opt.MapFrom(article => article.CategoryTitle))
                .ForMember(dto => dto.RatingTitle,
                           opt => opt.MapFrom(article => article.RatingTitle))
                .ForMember(dto => dto.Source,
                           opt => opt.MapFrom(article => article.Source))
                .ForMember(dto => dto.Comments,
                           opt => opt.MapFrom(article => article.Comments));

        }
    }
}
