using WebApi.Entities;
using WebApi.Hellpers;
using WebApi.Hellpers.CreatePage;

namespace WebApi.Interfaces
{
    public interface IAnimalRepository
    {
        Task<Animal> GetAnimalAsync(long id);
        Task<PageList<Animal>> GetAnimalsWitsParamsAsync(AnimalParams accountParams);
        void AddAnimal(Animal animal);
        void Update(Animal animal);
        void DeleteAnimal(Animal animal);

    }
}
