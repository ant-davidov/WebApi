namespace Domain
{
    public class VisitedLocationsParams : PaginationParams
    {
       public DateTime StartDateTime { get; set; } = DateTime.MinValue;
       public DateTime EndDateTime { get; set; } = DateTime.MaxValue;

    }
}
