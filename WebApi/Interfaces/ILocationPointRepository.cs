using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface ILocationPointRepository
    {
        void AddLocationPoint(LocationPoint locationPoint);
        Task<LocationPoint> GetLocationPointAsync(long id);
        Task<bool> CheckCordinatesAsync(LocationPoint point);
        Task<bool> VisitedLocationExistAsync(long id);
        void UpdateLocationPoint(LocationPoint locationPoint);
        void DeleteLocationPoint(LocationPoint locationPoint);
    }
}
