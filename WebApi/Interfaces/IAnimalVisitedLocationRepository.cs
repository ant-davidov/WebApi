using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface IAnimalVisitedLocationRepository
    {
        void AddAnimalVisitedLocationRepository(AnimalVisitedLocation visitedLocation);
        Task<AnimalVisitedLocation> GetAnimalVisitedLocationRepositoryAsync(long id);   
        void UpdateAnimalVisitedLocationRepository(AnimalVisitedLocation visitedLocation);
        void DeleteAnimalVisitedLocationRepository(AnimalVisitedLocation visitedLocation);

       
    }
}
