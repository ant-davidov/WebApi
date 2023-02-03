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

        public void AddAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
        }

        public void DeleteAnimal(Animal animal)
        {
            _context.Animals.Remove(animal);
        }

        public async Task<Animal> GetAnimalAsync(int id)
        {
            return await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PageList<Animal>> GetAnimalsWitsParamsAsync(AnimalParams animalParams)
        {
            var query = _context.Animals.AsQueryable();
            query = query.Where(a => DateTime.Compare(a.ChippingDateTime,animalParams.StartDateTime) >= 0);
            query = query.Where(a => DateTime.Compare(a.ChippingDateTime, animalParams.EndDateTime) <= 0);
            query = query.Where(a => a.ChipperId == animalParams.ChipperId);
            query = query.Where(a => a.LifeStatus == animalParams.LifeStatus);
            query = query.Where(a => a.Gender == animalParams.Gender);
            query = query.OrderBy(a => a.Id);

            return await PageList<Animal>.CreateAsync(query, animalParams.From, animalParams.Size);
        }

        public void Update(Animal animal)
        {
            _context.Entry(animal).State = EntityState.Modified;
        }
    }
}
