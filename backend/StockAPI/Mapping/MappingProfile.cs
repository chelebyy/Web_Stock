using AutoMapper;
using StockAPI.Models;
using StockAPI.Models.DTOs;

namespace StockAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<ITEquipment, ITEquipmentDto>();
            CreateMap<CreateITEquipmentDto, ITEquipment>();
            CreateMap<UpdateITEquipmentDto, ITEquipment>();
        }
    }
}
