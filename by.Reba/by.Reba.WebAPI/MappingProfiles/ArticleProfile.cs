using AutoMapper;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using by.Reba.WebAPI.Models.Requests.Article;

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

            CreateMap<ArticleFilterDTO, GetArticlesQueryByFilterQuery>()
                .ForMember(query => query.CategoriesId, opt => opt.MapFrom(dto => dto.CategoriesId))
                .ForMember(query => query.From, opt => opt.MapFrom(dto => dto.From))
                .ForMember(query => query.To, opt => opt.MapFrom(dto => dto.To))
                .ForMember(query => query.MinPositivity, opt => opt.MapFrom(dto => dto.MinPositivity))
                .ForMember(query => query.SourcesId, opt => opt.MapFrom(dto => dto.SourcesId));

            CreateMap<CreateArticleRequestModel, CreateOrEditArticleDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => Guid.NewGuid()))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(model => model.PosterUrl))
                .ForMember(dto => dto.HtmlContent, opt => opt.MapFrom(model => model.HtmlContent))
                .ForMember(dto => dto.SourceUrl, opt => opt.MapFrom(model => model.SourceUrl))
                .ForMember(dto => dto.CategoryId, opt => opt.MapFrom(model => model.CategoryId))
                .ForMember(dto => dto.SourceId, opt => opt.MapFrom(model => model.SourceId))
                .ForMember(dto => dto.PositivityId, opt => opt.MapFrom(model => model.PositivityId));

            CreateMap<PatchArticleRequestModel, CreateOrEditArticleDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(model => model.PosterUrl))
                .ForMember(dto => dto.HtmlContent, opt => opt.MapFrom(model => model.HtmlContent))
                .ForMember(dto => dto.SourceUrl, opt => opt.MapFrom(model => model.SourceUrl))
                .ForMember(dto => dto.CategoryId, opt => opt.MapFrom(model => model.CategoryId))
                .ForMember(dto => dto.SourceId, opt => opt.MapFrom(model => model.SourceId))
                .ForMember(dto => dto.PositivityId, opt => opt.MapFrom(model => model.PositivityId));

            CreateMap<RateArticleRequestModel, RateEntityDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.IsLike, opt => opt.MapFrom(model => model.IsLike));
        }
    }
}
