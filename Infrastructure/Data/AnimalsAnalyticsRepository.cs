using Domain.Entities;
using Domain.Entities.Secondary;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    internal class AnimalsAnalyticsRepository : IAnimalsAnalyticsRepository
    {
        private readonly DataContext _context;
        
        public AnimalsAnalyticsRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<AnalyticsResponse> GetAnalyticsAsync(long areaId, DateTime startDate, DateTime endDate)
        {
         
            var area = await _context.Areas.Include(a=>a.AreaPoints).FirstOrDefaultAsync(a => a.Id == areaId);
            if (area == null) return null;
            var polygon = area.Polygon;
            var visitedAnimals = _context.Animals.Include(a=>a.VisitedLocations)

     .Where(a => a.ChippingDateTime >= startDate && a.ChippingDateTime <= endDate)
     .Select(a => new
     {
         Animal = a,
         LocationPoint = a.ChippingLocation
     })
     
     .AsEnumerable()
     .Where(x => x.LocationPoint != null && polygon.Contains(Convert(x.LocationPoint)))
     .Select(x => x.Animal);

            // Общее количество животных, находящихся в зоне в указанный интервал времени
            var totalQuantityAnimals = visitedAnimals.Count();

            // Общее количество посещений зоны в указанный интервал времени
            var totalAnimalsArrived = visitedAnimals.SelectMany(a => a.VisitedLocations)
                .Count(vl => vl.DateTimeOfVisitLocationPoint >= startDate && vl.DateTimeOfVisitLocationPoint <= endDate);

            // Общее количество выходов из зоны в указанный интервал времени
            var totalAnimalsGone = visitedAnimals.SelectMany(a => a.VisitedLocations)
                .Count(vl => vl.DateTimeOfVisitLocationPoint >= startDate && vl.DateTimeOfVisitLocationPoint <= endDate &&
                             polygon.Contains(Convert(vl.LocationPoint)));

            // Группируем животных по типам
            var animalsAnalytics = visitedAnimals.GroupBy(a => a.AnimalTypes.FirstOrDefault().Type)
                .Select(g =>
                {
                    return new AnimalsAnalytics
                    {
                        AnimalType = g.Key,
                        AnimalTypeId = g.FirstOrDefault().AnimalTypes.FirstOrDefault().Id,
                        QuantityAnimals = g.Count(),
                        AnimalsArrived = g.SelectMany(a => a.VisitedLocations)
                            .Count(vl => vl.DateTimeOfVisitLocationPoint >= startDate && vl.DateTimeOfVisitLocationPoint <= endDate),
                        AnimalsGone = g.SelectMany(a => a.VisitedLocations)
                            .Count(vl => vl.DateTimeOfVisitLocationPoint >= startDate && vl.DateTimeOfVisitLocationPoint <= endDate &&
                                         polygon.Contains(Convert(vl.LocationPoint)))
                    };
                })
                .ToList();
            var analytics = new AnalyticsResponse();
            analytics.AnimalsAnalytics = animalsAnalytics;
            analytics.TotalAnimalsGone = totalAnimalsGone;
            analytics.TotalQuantityAnimals = totalQuantityAnimals;
            analytics.TotalAnimalsArrived = totalAnimalsArrived;
            return analytics;
        }

        private NetTopologySuite.Geometries.Point Convert(LocationPoint myPoint)
        {
            var coordinate = new NetTopologySuite.Geometries.Coordinate(myPoint.Longitude, myPoint.Latitude);
            return new NetTopologySuite.Geometries.Point(coordinate);

        }
    }
}
