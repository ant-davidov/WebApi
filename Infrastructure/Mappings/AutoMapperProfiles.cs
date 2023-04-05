using System.Globalization;
using AutoMapper;
using Domain.DTOs;
using Domain.DTOs.Animal;
using Domain.Entities;
using Domain.Entities.Secondary;

namespace WebApi.Hellpers
{
    public class AutoMapperProfiles : Profile
    {
  
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountDTO>();
            CreateMap<AreaDTO, Area>();

            CreateMap<AnimalType, AnimalTypeDTO>();

            CreateMap<AnimalTypeDTO, AnimalType>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(
                    src => 0));

            CreateMap<Animal, ReturnAnimalDTO>()
                .ForMember(dst => dst.ChipperId, opt => opt.MapFrom(
                    src => (src.Chipper).Id))
                .ForMember(dst => dst.VisitedLocations, opt => opt.MapFrom(
                    src => src.VisitedLocations.Select(x=>x.Id)))
                .ForMember(dst => dst.VisitedLocations, opt => opt.MapFrom(
                    src => src.VisitedLocations.Select(x => x.Id)))
                .ForMember(dst => dst.AnimalTypes, opt => opt.MapFrom(
                    src => src.AnimalTypes.Select(x=>x.Id)))           
                .ForMember(dst => dst.DeathDateTime, opt => opt.MapFrom(
                    src => (src.DeathDateTime == null ?
                            null :((DateTime)src.DeathDateTime).ToString("yyyy-MM-ddTHH:mmZ", CultureInfo.InvariantCulture))))
                .ForMember(dst => dst.ChippingDateTime, opt => opt.MapFrom(
                    src => (((DateTime)src.ChippingDateTime).ToString("yyyy-MM-ddTHH:mmZ", CultureInfo.InvariantCulture)))) ;

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

            CreateMap<Account, Account>()
                .ForMember(dst => dst.Id, opt => opt.Ignore());

            CreateMap<LocationPoint, LocationPoint>()
                .ForMember(dst => dst.Id, opt => opt.Ignore());
            
            CreateMap<LocationPointDTO, LocationPoint>()
                .ForMember(dst => dst.Id, opt => opt.Ignore());





            CreateMap<RegistrationDTO, Account>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(
                    src => src.Email));
         

            CreateMap<Account, AccountDTO>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault().Role.Name));








        }
    }
}
