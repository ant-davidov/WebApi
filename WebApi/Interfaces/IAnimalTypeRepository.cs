using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;

namespace WebApi.Interfaces
{
    public interface IAnimalTypeRepository
    {
        void AddAnimalType(AnimalType animalType);
        Task<AnimalType> GetAnimalTypeAsync(int id);
        Task<AnimalType> GetAnimalTypeByTypeAsync(string type);
        void UpdateAnimalType(AnimalType animalType);
        void DeleteAnimalType(AnimalType animalType);
        bool AllTypesExists(IEnumerable<AnimalType> types);
    }
}
