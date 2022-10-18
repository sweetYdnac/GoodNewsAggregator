using AutoMapper;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
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
                .ForMember(dto => dto.ParentCommentId, opt => opt.MapFrom(ent => ent.ParentCommentId));
        }
    }
}
