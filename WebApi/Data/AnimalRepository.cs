using AutoMapper.QueryableExtensions;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;
using WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace WebApi.Data
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly DataContext _context;
        public AnimalRepository(DataContext context)
        {
            _context = context;
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
            return await query.FirstOrDefaultAsync(a => a.Id == id);
        }
       
        public async Task<PageList<Animal>> GetAnimalsWitsParamsAsync(AnimalParams animalParams)
        {
            var query = GetAnumal().AsQueryable();
            query = query.Where(a => DateTime.Compare(a.ChippingDateTime,animalParams.StartDateTime) >= 0);
            query = query.Where(a => DateTime.Compare(a.ChippingDateTime, animalParams.EndDateTime) <= 0);
            query = query.Where(a => animalParams.ChipperId == 0 || a.Chipper.Id == animalParams.ChipperId);
            query = query.Where(a => animalParams.LifeStatus == null || a.LifeStatus == animalParams.LifeStatus);
            query = query.Where(a => animalParams.Gender == null || a.Gender == animalParams.Gender);
            query = query.Where(a =>animalParams.ChippingLocationId ==0 ||  a.ChippingLocation.Id == animalParams.ChippingLocationId);
            query = query.OrderBy(a => a.Id);

            return await PageList<Animal>.CreateAsync(query, animalParams.From, animalParams.Size);
        }

        public void Update(Animal animal)
        {
            _context.Entry(animal).State = EntityState.Modified;
        }
        private IIncludableQueryable<Animal,Account> GetAnumal ()
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
