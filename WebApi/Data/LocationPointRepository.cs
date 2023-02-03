using WebApi.Entities;
using WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace WebApi.Data
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

        public async Task<LocationPoint> GetLocationPointAsync(int id)
        {
           return await _context.LocationPoints.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> CheckCordinatesAsync(double latitude, double longitude)
        {
            return await _context.LocationPoints.FirstOrDefaultAsync(x => x.Latitude == latitude && x.Longitude == longitude) == null ;
        }
        

        public void UpdateLocationPoint(LocationPoint locationPoint)
        {
            _context.Entry(locationPoint).State = EntityState.Modified;
        }

       
    }
}
