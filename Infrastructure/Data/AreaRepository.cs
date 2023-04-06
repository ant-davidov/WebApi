using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Azure.Core.GeoJson;
using Domain.CreatePage;
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
            area.PolygonString = writer.Write(area.Polygon);
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
            double distance = -0.001; // Расстояние для буфера
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

        public async Task<LocationPoint> GetAreaByLocations(GetAreaByLocationsParams searchParams)
        {

            //var allPoints = _context.LocationPoints
            //             .Select(l => new Coordinate(l.Longitude, l.Latitude))
            //             .ToList();

            //var searchPoint = new Coordinate(searchParams.longitude, searchParams.latitude);
            //var closestPoint = allPoints.OrderBy(p => p.Distance(searchPoint) < 0.0).OrderBy(p => p.Distance(searchPoint)).FirstOrDefault();
            //var point =_context.LocationPoints.FirstOrDefault(x => x.Longitude == closestPoint.X && x.Latitude == closestPoint.Y);

            //return point;


            var allPoints = _context.LocationPoints
                     .Select(l => new Coordinate(l.Longitude, l.Latitude))
                     .ToList();

            var searchPoint = new Coordinate(searchParams.longitude, searchParams.latitude);

            var maxDistance = 0.0;

            var closestPoint = allPoints
                .Where(p => p.Distance(searchPoint) <= maxDistance)
                .OrderBy(p => p.Distance(searchPoint))
                .FirstOrDefault();

            if (closestPoint == null)
                return null;

            var point = _context.LocationPoints
                .FirstOrDefault(x => x.Longitude == closestPoint.X && x.Latitude == closestPoint.Y);

            if (point == null)
                return null;


            return point;


        }

        private NetTopologySuite.Geometries.Point Convert(GetAreaByLocationsParams searchParams)
        {
            var coordinate = new NetTopologySuite.Geometries.Coordinate(searchParams.longitude, searchParams.latitude);
            return new NetTopologySuite.Geometries.Point(coordinate);

        }


    }
}