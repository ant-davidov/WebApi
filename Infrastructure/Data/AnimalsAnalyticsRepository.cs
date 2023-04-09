using Domain.Entities;
using Domain.Entities.Secondary;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections;
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
            var analytics = new AnalyticsResponse();
            var area = await _context.Areas.Include(a => a.AreaPoints).FirstOrDefaultAsync(a => a.Id == areaId);
            if (area == null) return analytics;
            var polygon = area.Polygon;

            var visitedAnimals = _context.Animals
                .Include(a => a.AnimalTypes)
                .Include(a => a.VisitedLocations)
                .ThenInclude(x => x.LocationPoint)
                .Include(a => a.ChippingLocation)
                .AsEnumerable()
                .Where(a => a.ChippingDateTime >= startDate && a.ChippingDateTime <= endDate && IsInsidePolygon(polygon, a.ChippingLocation) || a.VisitedLocations.Any(v => v.DateTimeOfVisitLocationPoint >= startDate && v.DateTimeOfVisitLocationPoint <= endDate && IsInsidePolygon(polygon, v.LocationPoint)))
                .ToList();

            visitedAnimals = visitedAnimals.Select(a =>
            {
                var newVisitedPoint = new AnimalVisitedLocation
                {
                    DateTimeOfVisitLocationPoint = a.ChippingDateTime,
                    LocationPoint = a.ChippingLocation
                };
                a.VisitedLocations.Add(newVisitedPoint);
                return a;
            }).ToList();

            var tupleExitsAndEntries = GetEntriesAndExits(visitedAnimals, polygon);

            var listTotalQuantityAnimals = visitedAnimals
                  .Where(x => x.VisitedLocations.Any() && IsInsidePolygon(polygon, x.VisitedLocations
                        .OrderBy(y => y.DateTimeOfVisitLocationPoint)
                        .Last().LocationPoint))
                  .ToList();

            var animalsAnalytics = visitedAnimals
                    .SelectMany(a => a.AnimalTypes, (a, t) => new { Animal = a, Type = t })
                    .GroupBy(at => at.Type.Type)
                    .Select(g =>
                    {
                        return new AnimalsAnalytics
                        {
                            AnimalType = g.Key,
                            AnimalTypeId = g.FirstOrDefault().Type.Id,
                            QuantityAnimals = listTotalQuantityAnimals.Count(x => x.AnimalTypes.Any(y => y.Type == g.FirstOrDefault().Type.Type)),
                            AnimalsGone = tupleExitsAndEntries.Exits.Count(x => x.AnimalTypes.Any(y => y.Type == g.FirstOrDefault().Type.Type)),
                            AnimalsArrived = tupleExitsAndEntries.Entries.Count(x => x.AnimalTypes.Any(y => y.Type == g.FirstOrDefault().Type.Type)),
                        };
                    })
                    .ToList();

            analytics.TotalAnimalsGone = tupleExitsAndEntries.Exits.Count;
            analytics.TotalAnimalsArrived = tupleExitsAndEntries.Entries.Count;
            analytics.AnimalsAnalytics = animalsAnalytics;
            analytics.TotalQuantityAnimals = listTotalQuantityAnimals.Count;
            return analytics;






        }

        private NetTopologySuite.Geometries.Point Convert(LocationPoint myPoint)
        {
            var coordinate = new NetTopologySuite.Geometries.Coordinate(myPoint.Latitude, myPoint.Longitude);
            return new NetTopologySuite.Geometries.Point(coordinate);

        }

        private bool IsInsidePolygon(Polygon polygon, LocationPoint point)
        {
            var newPoint = Convert(point);
            return polygon.Contains(newPoint) || polygon.Touches(newPoint);
        }


        private int GetTotalAnimalsArrived(List<Animal> visitedAnimals, Polygon polygon)
        {
            var numEntries = 0;
            foreach (var animal in visitedAnimals)
            {
                var locations = animal.VisitedLocations
                    .OrderBy(l => l.DateTimeOfVisitLocationPoint)
                    .ToList();

                if (locations.Count < 2)
                {
                    // this animal has visited fewer than 2 locations, so we can't
                    // determine whether it has entered the area or not
                    continue;
                }
                bool wasInside = IsInsidePolygon(polygon, animal.ChippingLocation);
                for (int i = 1; i < locations.Count; i++)
                {
                    bool isInside = IsInsidePolygon(polygon, locations[i].LocationPoint);
                    if (isInside != wasInside)
                    {
                        // the animal crossed the boundary of the area
                        numEntries++;
                        continue;
                    }
                    wasInside = isInside;
                }
            }
            return numEntries;


        }

        private (List<Animal> Entries, List<Animal> Exits) GetEntriesAndExits(List<Animal> visitedAnimals, Polygon polygon)
        {
            var numEntries = 0;
            var numExits = 0;

            List<Animal> Entries = new();
            List<Animal> Exits = new();

            foreach (var animal in visitedAnimals)
            {
                var locations = animal.VisitedLocations
                    .OrderBy(l => l.DateTimeOfVisitLocationPoint)
                    .ToList();

                if (locations.Count < 2)
                {
                    // this animal has visited fewer than 2 locations, so we can't
                    // determine whether it has entered the area or not
                    continue;
                }

                bool wasInside = IsInsidePolygon(polygon, locations[0].LocationPoint);

                for (int i = 1; i < locations.Count; i++)
                {
                    if (Exits.Contains(animal) && Entries.Contains(animal)) continue;
                    bool isInside = IsInsidePolygon(polygon, locations[i].LocationPoint);

                    if (isInside != wasInside)
                    {
                        // the animal crossed the boundary of the area
                        if (!wasInside && !Entries.Contains(animal))
                        {
                            // the animal entered the area
                            numEntries++;
                            Entries.Add(animal);
                        }
                        else if (!(Exits.Contains(animal)))
                        {
                            numExits++;
                            Exits.Add(animal);
                        }

                    }

                    wasInside = isInside;
                }
            }

            return (Entries, Exits);
        }



    }
}
