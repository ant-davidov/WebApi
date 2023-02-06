using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;

namespace WebApi.Interfaces
{
    public interface IAnimalRepository
    {
        Task<Animal> GetAnimalAsync(long id);
        Task<PageList<Animal>> GetAnimalsWitsParamsAsync(AnimalParams accountParams);
        long AddAnimal(Animal animal);
        void Update(Animal animal);
        void DeleteAnimal(Animal animal);

    }
}
