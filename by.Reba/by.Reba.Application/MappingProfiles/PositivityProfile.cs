using AutoMapper;
using by.Reba.Application.Models.Source;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class PositivityProfile : Profile
    {
        public PositivityProfile()
        {
            CreateMap<T_Positivity, PositivityDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(ent => ent.Value));

            CreateMap<PositivityDTO, T_Positivity>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(ent => ent.Title, opt => opt.MapFrom(dto => dto.Title))
                .ForMember(ent => ent.Value, opt => opt.MapFrom(dto => dto.Value));

            CreateMap<CreateOrEditPositivityVM, PositivityDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(model => model.Value))
                .ReverseMap();
        }
    }
}
