using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAnimalTypeRepository
    {
        void AddAnimalType(AnimalType animalType);
        Task<AnimalType> GetAnimalTypeAsync(long id);
        Task<AnimalType> GetAnimalTypeByTypeAsync(string type);
        void UpdateAnimalType(AnimalType animalType);
        void DeleteAnimalType(AnimalType animalType);
        bool AllTypesExistsById(IEnumerable<long> types);
        Task<bool> AnimalsExistAsync(long id);
    }
}
