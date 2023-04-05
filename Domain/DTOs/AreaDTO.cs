using Domain.Entities.Secondary;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Domain.DTOs
{
    public class AreaDTO
    {
        [Required]
        public string Name { get; set; }
        [Required, MinLength(3)]
        public ICollection<Entities.Secondary.Coordinates> AreaPoints { get; set; }
        [JsonIgnore]
        public Polygon Polygon
        {
            get
            {
                var coordinates = AreaPoints.Select(p => new Coordinate(p.Latitude, p.Longitude)).ToList();
                coordinates.Add(coordinates[0]);
                return new Polygon(new LinearRing(coordinates.ToArray()));

            }

        }

    }
  
}
