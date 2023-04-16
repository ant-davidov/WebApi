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
                var reader = new WKTReader();
                return reader.Read(PolygonString) as Polygon;
            }
            
        }


    }



}