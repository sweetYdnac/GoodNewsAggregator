using AutoMapper;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.DataBase.Entities;
using by.Reba.WebAPI.Models.Requests.Sources;

namespace by.Reba.WebAPI.MappingProfiles
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
                .ForMember(dto => dto.Type, opt => opt.MapFrom(ent => ent.Type))
                .ReverseMap();

            CreateMap<CreateSourceRequestModel, CreateOrEditSourceDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => Guid.NewGuid()))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
                .ForMember(dto => dto.RssUrl, opt => opt.MapFrom(model => model.RssUrl))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(model => model.Source));

            CreateMap<PatchSourceRequestModel, CreateOrEditSourceDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(model => model.Name))
                .ForMember(dto => dto.RssUrl, opt => opt.MapFrom(model => model.RssUrl))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(model => model.Source));
        }
    }
}
