using Domain.DTOs.Animal;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAnimalRepository
    {
        Task<Animal> GetAnimalAsync(long id);
        Task<IEnumerable<ReturnAnimalDTO>> GetAnimalsWitsParamsAsync(AnimalParams accountParams);
        void AddAnimal(Animal animal);
        void Update(Animal animal);
        void DeleteAnimal(Animal animal);

    }
}
