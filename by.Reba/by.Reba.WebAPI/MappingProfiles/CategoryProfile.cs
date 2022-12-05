using AutoMapper;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.DataBase.Entities;

namespace by.Reba.WebAPI.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<T_Category, CategoryDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ReverseMap();
        }
    }
}
