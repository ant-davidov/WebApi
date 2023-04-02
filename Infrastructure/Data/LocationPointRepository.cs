using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class LocationPointRepository : ILocationPointRepository
    {
        private readonly DataContext _context;
        public LocationPointRepository(DataContext context)
        {
            _context = context;
        }

        public void AddLocationPoint(LocationPoint locationPoint)
        {
            _context.LocationPoints.Add(locationPoint);
        }

        public void DeleteLocationPoint(LocationPoint locationPoint)
        {
          _context.LocationPoints.Remove(locationPoint);
        }

        public async Task<LocationPoint> GetLocationPointAsync(long id)
        {
           return await _context.LocationPoints.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<bool> CheckCordinatesAsync(LocationPoint point)
        {
          return  !await _context.LocationPoints.AnyAsync(x => x.Latitude == point.Latitude && x.Longitude == point.Longitude);

        }
        public async Task<bool> VisitedLocationExistAsync(long id)
        {
           return await _context.Animals
           .Include(p=>p.ChippingLocation)
           .Include(p=> p.VisitedLocations)
           .ThenInclude(p=> p.LocationPoint)
           .AnyAsync(p=>p.ChippingLocation.Id == id || p.VisitedLocations.Any(x=> x.LocationPoint.Id == id));
        }
        public void UpdateLocationPoint(LocationPoint locationPoint)
        {
            _context.Entry(locationPoint).State = EntityState.Modified;
        }

       
    }
}
