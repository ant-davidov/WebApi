using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;

namespace WebApi.Interfaces
{
    public interface IAnimalRepository
    {
        Task<Animal> GetAnimalAsync(int id);
        Task<PageList<Animal>> GetAnimalsWitsParamsAsync(AnimalParams accountParams);
        void AddAnimal(Animal animal);
        void Update(Animal animal);
        void DeleteAnimal(Animal animal);
    }
}
