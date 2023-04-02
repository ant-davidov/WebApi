using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AnimalTypeRepository : IAnimalTypeRepository
    {
        private readonly DataContext _context;
        public AnimalTypeRepository(DataContext context)
        {
            _context = context;
        }
        public void AddAnimalType(AnimalType animalType)
        {
           _context.AnimalTypes.Add(animalType);   
        }

        public void DeleteAnimalType(AnimalType animalType)
        {
            _context.AnimalTypes.Remove(animalType);
        }

        public async Task<AnimalType> GetAnimalTypeAsync(long id)
        {
           return await  _context.AnimalTypes.FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<AnimalType> GetAnimalTypeByTypeAsync(string type)
        {
            return  await _context.AnimalTypes.FirstOrDefaultAsync(a => a.Type == type.ToLower().Trim());
        }
        

        public void UpdateAnimalType(AnimalType animalType)
        {
            _context.Entry(animalType).State = EntityState.Modified;
        }
        public bool AllTypesExistsById(IEnumerable<long> types)
        {
            return types.All(t=> _context.AnimalTypes.Any(a=> a.Id == t));
        }

        public async Task<bool> AnimalsExistAsync(long id)
        {
           return  await _context.Animals.Include(x=>x.AnimalTypes).AnyAsync(a=> a.AnimalTypes.Any(p=> p.Id == id));
        }
    }
}
