

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AnimalVisitedLocationRepository : IAnimalVisitedLocationRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AnimalVisitedLocationRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AnimalVisitedLocationDTO>> GetAnimalVisitedLocationByParametersRepositoryAsync(long animalId, VisitedLocationsParams locationsParams)
        {
            var query = (await _context.Animals.Include(x=>x.VisitedLocations).FirstOrDefaultAsync(x=> x.Id == animalId)).VisitedLocations.AsQueryable();
            query = query.Where(x => DateTime.Compare(x.DateTimeOfVisitLocationPoint, locationsParams.StartDateTime) >= 0);
            query = query.Where(x => DateTime.Compare(x.DateTimeOfVisitLocationPoint, locationsParams.EndDateTime) <= 0);
            query = query.OrderBy(x => x.DateTimeOfVisitLocationPoint);
            return  query.ProjectTo<AnimalVisitedLocationDTO>(_mapper.ConfigurationProvider).AsNoTracking().Skip(locationsParams.From).Take(locationsParams.Size).ToList();
        }

        public void UpdateAnimalVisitedLocationRepository(AnimalVisitedLocation visitedLocation)
        {
            _context.Entry(visitedLocation).State = EntityState.Modified;
        }

        
    }
}
