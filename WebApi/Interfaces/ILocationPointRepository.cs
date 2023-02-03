using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface ILocationPointRepository
    {
        void AddLocationPoint(LocationPoint locationPoint);
        Task<LocationPoint> GetLocationPointAsync(int id);
        Task<bool> CheckCordinatesAsync(double latitude, double longitude);
        void UpdateLocationPoint(LocationPoint locationPoint);
        void DeleteLocationPoint(LocationPoint locationPoint);
    }
}
