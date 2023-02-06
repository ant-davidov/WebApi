using WebApi.Entities;
using WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace WebApi.Data
{
    public class AnimalVisitedLocationRepository : IAnimalVisitedLocationRepository
    {
        private readonly DataContext _context;
        public AnimalVisitedLocationRepository(DataContext context)
        {
            _context = context;
        }
        public void AddAnimalVisitedLocationRepository(AnimalVisitedLocation visitedLocation)
        {
            _context.AnimalVisitedLocations.Add(visitedLocation);
        }

        public void DeleteAnimalVisitedLocationRepository(AnimalVisitedLocation visitedLocation)
        {
           _context.AnimalVisitedLocations.Remove(visitedLocation);
        }

        public async Task<AnimalVisitedLocation> GetAnimalVisitedLocationRepositoryAsync(long id)
        {
            return await _context.AnimalVisitedLocations.Include(p=>p.LocationPoint) .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void UpdateAnimalVisitedLocationRepository(AnimalVisitedLocation visitedLocation)
        {
            _context.Entry(visitedLocation).State = EntityState.Modified;
        }
    }
}
