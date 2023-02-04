using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class LocationPoint
    {
        public long Id { get; set; }
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; }
        [Range(-180.0, 180.0)]
        public double Longitude { get; set; }
    }
}
