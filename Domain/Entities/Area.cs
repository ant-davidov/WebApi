using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Entities.Secondary;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Coordinates = Domain.Entities.Secondary.Coordinates;

namespace Domain.Entities
{
    public class Area
    {
        private Polygon polygon;

        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Coordinates> AreaPoints { get; set; }
        [JsonIgnore]
        public string PolygonString { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Polygon Polygon
        {
            get
            {
                //var coordinates = AreaPoints.Select(p => new Coordinate(p.Latitude, p.Longitude)).ToList();
                //coordinates.Add(coordinates[0]);
                //return new Polygon(new LinearRing(coordinates.ToArray()));
                var reader = new WKTReader();

                // преобразуем строку WKT в полигон
                return reader.Read(PolygonString) as Polygon;
            }
            
        }


    }



}