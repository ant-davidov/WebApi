using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAnimalVisitedLocationRepository
    {

        Task<IEnumerable<AnimalVisitedLocationDTO>> GetAnimalVisitedLocationByParametersRepositoryAsync(long animalId, VisitedLocationsParams locationsParams);   
        void UpdateAnimalVisitedLocationRepository(AnimalVisitedLocation visitedLocation);
          
    }
}
