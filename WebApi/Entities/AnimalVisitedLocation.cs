namespace WebApi.Entities
{
    public class AnimalVisitedLocation
    {
        public long Id { get; set; }
        public DateTime DateTimeOfVisitLocationPoint { get; set; }
        public LocationPoint LocationPoint{ get; set;}
    }
}
