using System.Globalization;
using AutoMapper;
using WebApi.DTOs;
using WebApi.DTOs.Animal;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Hellpers
{
    public class AutoMapperProfiles : Profile
    {
  
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountDTO>();
            CreateMap<AnimalType, AnimalTypeDTO>();    
            CreateMap<AnimalTypeDTO, AnimalType>();          
           
            CreateMap<Animal, ReturnAnimalDTO>()
                .ForMember(dst => dst.VisitedLocations, opt => opt.MapFrom(
                    src => src.VisitedLocations.Select(x=>x.Id)))
                .ForMember(dst => dst.AnimalTypes, opt => opt.MapFrom(
                    src => src.AnimalTypes.Select(x=>x.Id)))           
                .ForMember(dst => dst.DeathDateTime, opt => opt.MapFrom(
                    src => (src.DeathDateTime == null ?
                            null :((DateTime)src.DeathDateTime).ToString("yyyy-MM-ddTHH:mmZ", CultureInfo.InvariantCulture))))
                .ForMember(dst => dst.ChippingDateTime, opt => opt.MapFrom(
                    src => (((DateTime)src.ChippingDateTime).ToString("yyyy-MM-ddTHH:mmZ", CultureInfo.InvariantCulture)))) ;
                //  .AfterMap((src, dest) =>
                // {
                //     if (dest.VisitedLocations?.Count() == 0) dest.VisitedLocations =null; 
                //     if (dest.AnimalTypes?.Count() == 0) dest.AnimalTypes =null; 
                // });
           
            CreateMap<Account, ReturnAnimalDTO>()
                .ForMember(dst => dst.ChipperId, opt => opt.MapFrom(
                    src => src.Id));
           
           
            CreateMap<LocationPoint, ReturnAnimalDTO>()
                .ForMember(dst => dst.ChippingLocationId, opt => opt.MapFrom(
                    src => src.Id));
           
           
            CreateMap<AddAnimalDTO, Animal>()
                .ForMember(dst => dst.AnimalTypes, opt => opt.Ignore())
                .ForMember(dst => dst.Chipper, opt => opt.Ignore())
                .ForMember(dst => dst.ChippingLocation, opt => opt.Ignore())
                .ForMember(dst => dst.ChippingDateTime, opt=> opt.MapFrom(s => DateTime.UtcNow));
             CreateMap<UpdateAnimalDTO, Animal>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
           
            CreateMap<AnimalVisitedLocation, AnimalVisitedLocationDTO>()
                 .ForMember(dst => dst.DateTimeOfVisitLocationPoint, opt => opt.MapFrom(
                    src => (((DateTime)src.DateTimeOfVisitLocationPoint).ToString("yyyy-MM-ddTHH:mmZ", CultureInfo.InvariantCulture))));        
           
                
               
        }
    }
}
