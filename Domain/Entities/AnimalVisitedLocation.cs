using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AnimalVisitedLocation
    {
        [Key]
        public long Id { get; set; }
        public DateTime DateTimeOfVisitLocationPoint { get; set; }
        public LocationPoint LocationPoint{ get; set;}
    }
}
