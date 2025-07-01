using AutoMapper;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Common.Mappings
{
    public class ActivityLogMappingProfile : Profile
    {
        public ActivityLogMappingProfile()
        {
            CreateMap<ActivityLog, ActivityLogDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.GetFullName() : src.Username));
        }
    }
} 