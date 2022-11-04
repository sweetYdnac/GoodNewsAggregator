using AutoMapper;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class UserHistoryProfile : Profile
    {
        public UserHistoryProfile()
        {
            CreateMap<T_UserHistory, UserHistoryDTO>()
                .ForMember(dto => dto.ArticleId, opt => opt.MapFrom(ent => ent.ArticleId))
                .ForMember(dto => dto.LastVisitTime, opt => opt.MapFrom(ent => ent.LastVisitTime));
        }
    }
}
