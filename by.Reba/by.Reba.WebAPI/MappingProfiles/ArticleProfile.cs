using AutoMapper;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.DataBase.Entities;

namespace by.Reba.WebAPI.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<T_Article, ArticleDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ForMember(dto => dto.HtmlContent, opt => opt.MapFrom(ent => ent.HtmlContent))
                .ForMember(dto => dto.PublicationDate, opt => opt.MapFrom(ent => ent.PublicationDate))
                .ForMember(dto => dto.CategoryTitle, opt => opt.MapFrom(ent => ent.Category.Title))
                .ForMember(dto => dto.RatingTitle, opt => opt.MapFrom(ent => ent.Positivity == null ? "default" : ent.Positivity.Title))
                .ForMember(dto => dto.Assessment, opt => opt.MapFrom(ent => ent.UsersWithPositiveAssessment.Count() - ent.UsersWithNegativeAssessment.Count()))
                .ForMember(dto => dto.Source, opt => opt.MapFrom(ent => ent.Source));

            CreateMap<T_Article, ArticlePreviewDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(ent => ent.PosterUrl))
                .ForMember(dto => dto.PublicationDate, opt => opt.MapFrom(ent => ent.PublicationDate))
                .ForMember(dto => dto.Assessment, opt => opt.MapFrom(ent => ent.UsersWithPositiveAssessment.Count() - ent.UsersWithNegativeAssessment.Count()))
                .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(ent => ent.Category.Title))
                .ForMember(dto => dto.PositivityName, opt => opt.MapFrom(ent => ent.Positivity == null ? "default" : ent.Positivity.Title))
                .ForMember(dto => dto.CommentsCount, opt => opt.MapFrom(ent => ent.Comments.Count))
                .ForMember(dto => dto.SourceName, opt => opt.MapFrom(ent => ent.Source.Name));

            CreateMap<T_Article, CreateOrEditArticleDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ForMember(dto => dto.HtmlContent, opt => opt.MapFrom(ent => ent.HtmlContent))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(ent => ent.PosterUrl))
                .ForMember(dto => dto.CategoryId, opt => opt.MapFrom(ent => ent.CategoryId))
                .ForMember(dto => dto.SourceId, opt => opt.MapFrom(ent => ent.SourceId));

            CreateMap<CreateOrEditArticleDTO, T_Article>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(ent => ent.Title, opt => opt.MapFrom(dto => dto.Title))
                .ForMember(ent => ent.HtmlContent, opt => opt.MapFrom(dto => dto.HtmlContent))
                .ForMember(ent => ent.SourceUrl, opt => opt.MapFrom(dto => dto.SourceUrl))
                .ForMember(ent => ent.PosterUrl, opt => opt.MapFrom(dto => dto.PosterUrl))
                .ForMember(ent => ent.CategoryId, opt => opt.MapFrom(dto => dto.CategoryId))
                .ForMember(ent => ent.SourceId, opt => opt.MapFrom(dto => dto.SourceId))
                .ForMember(ent => ent.PositivityId, opt => opt.MapFrom(dto => dto.PositivityId))
                .ForMember(ent => ent.PublicationDate, opt => opt.MapFrom(dto => DateTime.Now));

            CreateMap<CreateArticleFromRssDTO, T_Article>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(ent => ent.Title, opt => opt.MapFrom(dto => dto.Title))
                .ForMember(ent => ent.PosterUrl, opt => opt.MapFrom(dto => dto.PosterUrl))
                .ForMember(ent => ent.PublicationDate, opt => opt.MapFrom(dto => dto.PublicationDate))
                .ForMember(ent => ent.SourceUrl, opt => opt.MapFrom(dto => dto.SourceUrl))
                .ForMember(ent => ent.SourceId, opt => opt.MapFrom(dto => dto.SourceId))
                .ForMember(ent => ent.CategoryId, opt => opt.MapFrom(dto => dto.CategoryId));
        }
    }
}
