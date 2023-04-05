using Domain.DTOs;
using Domain.Entities.Secondary;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Reflection.PortableExecutable;

public static class CheckingForConvexPolygon
{
    public  static bool IsConvexPolygon(AreaDTO newArea)
    {
        var reader = new WKTReader();
        newArea.AreaPoints.Add(newArea.AreaPoints.First());
        var newPolygon = GeometryFactory.Default.CreatePolygon(
               newArea.AreaPoints.Select(p => new Coordinate(p.Longitude, p.Latitude)).ToArray());

        return newPolygon.IsSimple;
       
    }
}