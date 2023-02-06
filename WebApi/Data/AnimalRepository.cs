using AutoMapper.QueryableExtensions;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;
using WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly DataContext _context;
        public AnimalRepository(DataContext context)
        {
            _context = context;
        }

        public long AddAnimal(Animal animal)
        {
           _context.Animals.Add(animal);
           return animal.Id;
        }

        public void DeleteAnimal(Animal animal)
        {
            _context.Animals.Remove(animal);
        }

        public async Task<Animal> GetAnimalAsync(long id)
        {
            return await _context.Animals.Include(x=> x.AnimalTypes)
            .Include(x=> x.VisitedLocations)
            .Include(x=>x.Chipper)
            .Include(x=>x.ChippingLocation)
            .FirstOrDefaultAsync(a => a.Id == id);
        }
       
        public async Task<PageList<Animal>> GetAnimalsWitsParamsAsync(AnimalParams animalParams)
        {
            var query = _context.Animals
            .Include(x=> x.AnimalTypes)
            .Include(x=> x.VisitedLocations)
            .ThenInclude(x=> x.LocationPoint)
            .Include(x=>x.Chipper)
            .Include(x=>x.ChippingLocation)
            .AsQueryable();
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

      
    }
}
