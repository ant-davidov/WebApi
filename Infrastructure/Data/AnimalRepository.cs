
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.DTOs;
using Domain.DTOs.Animal;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Data
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AnimalRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddAnimal(Animal animal)
        {
           _context.Animals.Add(animal);
        }

        public void DeleteAnimal(Animal animal)
        {
            _context.Animals.Remove(animal);
        }

        public async Task<Animal> GetAnimalAsync(long id)
        {   var query = GetAnumal();
            return await query.AsSplitQuery().FirstOrDefaultAsync(a => a.Id == id);
        }
       
        public async Task<IEnumerable<ReturnAnimalDTO>> GetAnimalsWitsParamsAsync(AnimalParams animalParams)
        {
            var query = GetAnumal().AsQueryable();
            query = query.Where(a => DateTime.Compare(a.ChippingDateTime,animalParams.StartDateTime) >= 0);
            query = query.Where(a => DateTime.Compare(a.ChippingDateTime, animalParams.EndDateTime) <= 0);
            query = query.Where(a => animalParams.ChipperId == 0 || a.Chipper.Id == animalParams.ChipperId);
            query = query.Where(a => animalParams.LifeStatus == null || a.LifeStatus == animalParams.LifeStatus);
            query = query.Where(a => animalParams.Gender == null || a.Gender == animalParams.Gender);
            query = query.Where(a =>animalParams.ChippingLocationId == 0 ||  a.ChippingLocation.Id == animalParams.ChippingLocationId);
            query = query.OrderBy(a => a.Id);
            return await query.ProjectTo<ReturnAnimalDTO>(_mapper.ConfigurationProvider).AsNoTracking().Skip(animalParams.From).Take(animalParams.Size).ToListAsync();
           
        }

        public void Update(Animal animal)
        {
            _context.Entry(animal).State = EntityState.Modified;
        }
        private IIncludableQueryable<Animal,Account> GetAnumal()
        {
           return _context.Animals
                    .Include(p=>p.AnimalTypes)
                    .Include(p=>p.ChippingLocation)
                    .Include(p=> p.VisitedLocations)
                    .ThenInclude(x=> x.LocationPoint)
                    .Include(p=>p.Chipper);            
        }
        
        

      
    }
}
