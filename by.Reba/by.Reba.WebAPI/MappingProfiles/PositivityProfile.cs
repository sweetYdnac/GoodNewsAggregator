using AutoMapper;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.DataBase.Entities;
using by.Reba.WebAPI.Models.Requests.Positivity;

namespace by.Reba.WebAPI.MappingProfiles
{
    public class PositivityProfile : Profile
    {
        public PositivityProfile()
        {
            CreateMap<T_Positivity, PositivityDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(ent => ent.Value))
                .ReverseMap();

            CreateMap<CreatePositivityRequestModel, PositivityDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => Guid.NewGuid()))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(model => model.Value));

            CreateMap<PatchPositivityRequestModel, PositivityDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(model => model.Value));
        }
    }
}
