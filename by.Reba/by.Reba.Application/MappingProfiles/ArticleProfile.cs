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
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ForMember(dto => dto.Text, opt => opt.MapFrom(ent => ent.Text))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(ent => ent.PosterUrl))
                .ForMember(dto => dto.PublicationDate, opt => opt.MapFrom(ent => ent.PublicationDate))
                .ForMember(dto => dto.CategoryTitle, opt => opt.MapFrom(ent => ent.Category.Title))
                .ForMember(dto => dto.RatingTitle, opt => opt.MapFrom(ent => ent.Rating.Title))
                .ForMember(dto => dto.Assessment, opt => opt.MapFrom(ent => ent.UsersWithPositiveAssessment.Count() - ent.UsersWithNegativeAssessment.Count()))
                .ForMember(dto => dto.Source, opt => opt.MapFrom(ent => ent.Source));

            CreateMap<T_Article, ArticlePreviewDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ForMember(dto => dto.PosterUrl, opt => opt.MapFrom(ent => ent.PosterUrl))
                .ForMember(dto => dto.PublicationDate, opt => opt.MapFrom(ent => ent.PublicationDate))
                .ForMember(dto => dto.Assessment, opt => opt.MapFrom(ent => ent.UsersWithPositiveAssessment.Count() - ent.UsersWithNegativeAssessment.Count()))
                .ForMember(dto => dto.CategoryName,opt => opt.MapFrom(ent => ent.Category.Title))
                .ForMember(dto => dto.RatingName,opt => opt.MapFrom(ent => ent.Rating.Title))
                .ForMember(dto => dto.CommentsCount,opt => opt.MapFrom(ent => ent.Comments.Count))
                .ForMember(dto => dto.SourceName,opt => opt.MapFrom(ent => ent.Source.Name));

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
                .ForMember(model => model.Title,opt => opt.MapFrom(dto => dto.Title))
                .ForMember(model => model.Text,opt => opt.MapFrom(dto => dto.Text))
                .ForMember(model => model.PosterUrl,opt => opt.MapFrom(dto => dto.PosterUrl))
                .ForMember(model => model.PublicationDate,opt => opt.MapFrom(dto => dto.PublicationDate))
                .ForMember(model => model.Assessment,opt => opt.MapFrom(dto => dto.Assessment))
                .ForMember(model => model.CategoryTitle,opt => opt.MapFrom(dto => dto.CategoryTitle))
                .ForMember(model => model.RatingTitle,opt => opt.MapFrom(dto => dto.RatingTitle))
                .ForMember(model => model.Source,opt => opt.MapFrom(dto => dto.Source))
                .ForMember(model => model.Comments,opt => opt.MapFrom(dto => dto.CommentTrees));

        }
    }
}
