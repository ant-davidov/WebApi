using AutoMapper;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Hellpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountDTO>();
            CreateMap<AnimalType, AnimalTypeDTO>();

        }
    }
}
