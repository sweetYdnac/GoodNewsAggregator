using AutoMapper;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.DataBase.Entities;

namespace by.Reba.WebAPI.MappingProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<T_Comment, CommentDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Author, opt => opt.MapFrom(ent => ent.Author))
                .ForMember(dto => dto.Content, opt => opt.MapFrom(ent => ent.Content))
                .ForMember(dto => dto.Assessment, opt => opt.MapFrom(ent => ent.UsersWithPositiveAssessment.Count() - ent.UsersWithNegativeAssessment.Count()))
                .ForMember(dto => dto.CreationTime, opt => opt.MapFrom(ent => ent.CreationTime))
                .ForMember(dto => dto.ParentCommentId, opt => opt.MapFrom(ent => ent.ParentCommentId))
                .ForMember(dto => dto.ArticleId, opt => opt.MapFrom(ent => ent.ArticleId));

            CreateMap<T_Comment, CommentShortSummaryDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Author, opt => opt.MapFrom(ent => ent.Author))
                .ForMember(dto => dto.Content, opt => opt.MapFrom(ent => ent.Content))
                .ForMember(dto => dto.Assessment, opt => opt.MapFrom(ent => ent.UsersWithPositiveAssessment.Count() - ent.UsersWithNegativeAssessment.Count()))
                .ForMember(dto => dto.CreationTime, opt => opt.MapFrom(ent => ent.CreationTime))
                .ForMember(dto => dto.ArticleId, opt => opt.MapFrom(ent => ent.ArticleId));

            CreateMap<CreateCommentDTO, T_Comment>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(ent => ent.Content, opt => opt.MapFrom(dto => dto.Content))
                .ForMember(ent => ent.CreationTime, opt => opt.MapFrom(dto => DateTime.Now))
                .ForMember(ent => ent.ArticleId, opt => opt.MapFrom(dto => dto.ArticleId))
                .ForMember(ent => ent.ParentCommentId, opt => opt.MapFrom(dto => dto.ParentCommentId))
                .ForMember(ent => ent.AuthorId, opt => opt.MapFrom(dto => dto.AuthorId));
        }
    }
}
