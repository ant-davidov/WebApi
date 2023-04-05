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
            area.PolygonString = writer.Write(area.Polygon);
            _context.Areas.Add(area);
        }

        public void DeleteArea(Area area)
        {

            _context.Areas.Remove(area);
        }

        public Task<Area> GetAreaAsync(long id)
        {
            return _context.Areas.Include(a=>a.AreaPoints).FirstOrDefaultAsync(a => a.Id == id);
        }
        public void Update(Area area)
        {
            _context.Entry(area).State = EntityState.Modified;
        }
        public async Task<bool> DoesIntersectWithExistingAreas(AreaDTO newArea, long id = 0)
        {
            double distance = -0.001; // Расстояние для буфера
            var areas = await _context.Areas.Include(a=>a.AreaPoints).ToListAsync();
            var intersectingAreas = areas.Where(a=>a.Id != id).Where(a => (a.Polygon.Buffer(distance)).Intersects(newArea.Polygon.Buffer(distance)));
            return intersectingAreas.Any();

        }
        public async Task<bool> CheckPolygonIntersectionAsync(AreaDTO newArea,long id = 0 ,CancellationToken cancellationToken = default)
        {
            //var polygons = await _context.Areas
            //    .Select(a => a.PolygonString)
            //    .ToListAsync(cancellationToken);

            //var options = new ParallelOptions { CancellationToken = cancellationToken };
            //var foundIntersection = false;

            //Parallel.ForEach(polygons, options, (polygonString, loopState) =>
            //{
            //    var existingPolygon = new WKTReader().Read(polygonString) as Polygon;
            //    if (existingPolygon.Intersects(newPolygon))
            //    {
            //        foundIntersection = true;
            //        loopState.Stop();
            //    }
            //});

            //return foundIntersection;
            var reader = new WKTReader();
            var existingPolygons = await _context.Areas
                .Where(a => a.Id != id)
                .Include(a=> a.AreaPoints)
                .Select(a => a.Polygon)
                .ToListAsync();

           


            // Проверяем, находится ли новый полигон внутри существующих зон
            var isInsideExistingPolygon = existingPolygons.Any(p => p.Contains(newArea.Polygon));

            var isInsideExistingPolygon2 = existingPolygons.Any(p => p.Within(newArea.Polygon));

            return isInsideExistingPolygon2 && isInsideExistingPolygon2;

        }


    }
}