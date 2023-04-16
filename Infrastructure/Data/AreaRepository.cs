using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Azure.Core.GeoJson;
using Domain.DTOs;
using Domain.Entities;
using Domain.Entities.Secondary;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Operation.OverlayNG;

namespace Infrastructure.Data
{
    public class AreaRepository : IAreaRepository
    {
        private readonly DataContext _context;
        public AreaRepository(DataContext context)
        {
            _context = context;
        }
        public void AddArea(Area area)
        {
            var writer = new WKTWriter();

            var coordinates = area.AreaPoints.Select(p => new Coordinate(p.Latitude, p.Longitude)).ToList();
            coordinates.Add(coordinates[0]);
            var polygon = new Polygon(new LinearRing(coordinates.ToArray()));
            area.PolygonString = writer.Write(polygon);
            _context.Areas.Add(area);
        }

        public void DeleteArea(Area area)
        {

            _context.Areas.Remove(area);
        }

        public Task<Area> GetAreaAsync(long id)
        {
            return _context.Areas.Include(a => a.AreaPoints).FirstOrDefaultAsync(a => a.Id == id);
        }
        public void Update(Area area)
        {
            _context.Entry(area).State = EntityState.Modified;
        }
        public async Task<bool> DoesIntersectWithExistingAreas(AreaDTO newArea, long id = 0)
        {
            double distance = -0.001; 
            var areas = await _context.Areas.Include(a => a.AreaPoints).ToListAsync();
            var intersectingAreas = areas.Where(a => a.Id != id).Where(a => (a.Polygon.Buffer(distance)).Intersects(newArea.Polygon.Buffer(distance)));
            return intersectingAreas.Any();

        }
        public async Task<bool> CheckPolygonIntersectionAsync(AreaDTO newArea, long id = 0, CancellationToken cancellationToken = default)
        {
            var reader = new WKTReader();
            var existingPolygons = await _context.Areas
                .Where(a => a.Id != id)
                .Include(a => a.AreaPoints)
                .Select(a => a.Polygon)
                .ToListAsync();
            var isInsideExistingPolygon = existingPolygons.Any(p => p.Contains(newArea.Polygon));

            var isInsideExistingPolygon2 = existingPolygons.Any(p => p.Within(newArea.Polygon));

            return isInsideExistingPolygon2 && isInsideExistingPolygon2;

        }

        public async Task<LocationPoint> GetAreaByLocations(LocationPointDTO searchParams)
        {

            var allPoints = await _context.LocationPoints
                     .Select(l => new Coordinate(l.Latitude, l.Longitude))
                     .ToListAsync();

            var searchPoint = new Coordinate(searchParams.Latitude, searchParams.Longitude);
            double maxDistance = 0.5;
            var closestPoint = allPoints
                .Where(p => p.Distance(searchPoint) <= maxDistance)
                .OrderBy(p => p.Distance(searchPoint))
                .FirstOrDefault();
            if (closestPoint == null) return null;
            var point = _context.LocationPoints
                .FirstOrDefault(x => x.Latitude == closestPoint.X && x.Longitude == closestPoint.Y);

            if (point == null) return null;
            return point;


        }

      


    }
}