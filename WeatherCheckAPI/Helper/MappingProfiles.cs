using AutoMapper;
using Core.Entities;
using WeatherCheckAPI.DTO;

namespace WeatherCheckAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Districts, DistrictDTO>().ReverseMap();
            CreateMap<LocalTemperature, LocalTemperatureDTO>().ReverseMap();
        }
    }
}
