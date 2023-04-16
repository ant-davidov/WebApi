using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAreaRepository
    {
        Task<Area> GetAreaAsync(long id);
        void AddArea(Area animal);
        void Update(Area animal);
        void DeleteArea(Area animal);
        Task<bool> DoesIntersectWithExistingAreas(AreaDTO newArea, long id = 0);
        Task<bool> CheckPolygonIntersectionAsync(AreaDTO newArea, long id = 0, CancellationToken cancellationToken = default);
        public Task<LocationPoint> GetAreaByLocations(LocationPointDTO searchParams);
    }
}