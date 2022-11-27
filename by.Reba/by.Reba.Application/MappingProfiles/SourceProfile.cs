using AutoMapper;
using by.Reba.Application.Models.Source;
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
                .ForMember(dto => dto.RssUrl, opt => opt.MapFrom(ent => ent.RssUrl))
                .ForMember(dto => dto.SourceType, opt => opt.MapFrom(ent => ent.Type));

            CreateMap<T_Source, CreateOrEditSourceDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(ent => ent.Name))
                .ForMember(dto => dto.RssUrl, opt => opt.MapFrom(ent => ent.RssUrl))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(ent => ent.Type));

            CreateMap<CreateOrEditSourceDTO, T_Source>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => Guid.NewGuid()))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
                .ForMember(dto => dto.RssUrl, opt => opt.MapFrom(model => model.RssUrl))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(model => model.Type));

            CreateMap<CreateOrEditSourceVM, CreateOrEditSourceDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
                .ForMember(dto => dto.RssUrl, opt => opt.MapFrom(model => model.RssUrl))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(model => model.Source))
                .ReverseMap();
        }
    }
}
