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
                .ForMember(dto => dto.Author, opt => opt.MapFrom(c => c.Author))
                .ForMember(dto => dto.Content, opt => opt.MapFrom(s => s.Content))
                .ForMember(dto => dto.Assessment, opt => opt.MapFrom(s => s.Assessment))
                .ForMember(dto => dto.CreationTime, opt => opt.MapFrom(s => s.CreationTime))
                .ForMember(dto => dto.InnerComments, opt => opt.MapFrom(s => s.InnerComments));
        }
    }
}
