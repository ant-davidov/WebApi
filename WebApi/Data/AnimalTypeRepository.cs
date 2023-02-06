using AutoMapper;
using WebApi.Entities;
using WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebApi.Data
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
           return  await _context.AnimalTypes.FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<AnimalType> GetAnimalTypeByTypeAsync(string type)
        {
            return await _context.AnimalTypes.FirstOrDefaultAsync(a => a.Type == type);
        }

        public void UpdateAnimalType(AnimalType animalType)
        {
            _context.Entry(animalType).State = EntityState.Modified;
        }
        public bool AllTypesExistsById(IEnumerable<long> types)
        {

            //return  await _context.AnimalTypes.AnyAsync(a=> types.Any(t=> a.Type == t.Type));
            //return types.All(t => _context.AnimalTypes.Any(a=> t == a.Id)); 
            return types.All(t=> _context.AnimalTypes.Any(a=> a.Id == t));
        }
    }
}
