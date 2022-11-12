using AutoMapper;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<T_Source, SourceDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(ent => ent.Name))
                .ForMember(dto => dto.Url, opt => opt.MapFrom(ent => ent.RssUrl));
        }
    }
}
