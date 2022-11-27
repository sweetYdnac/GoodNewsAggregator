using AutoMapper;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class HistoryProfile : Profile
    {
        public HistoryProfile()
        {
            CreateMap<T_History, UserHistoryDTO>()
                .ForMember(dto => dto.ArticleId, opt => opt.MapFrom(ent => ent.ArticleId))
                .ForMember(dto => dto.ArticleTitle, opt => opt.MapFrom(ent => ent.Article.Title))
                .ForMember(dto => dto.LastVisitTime, opt => opt.MapFrom(ent => ent.LastVisitTime));
        }
    }
}
