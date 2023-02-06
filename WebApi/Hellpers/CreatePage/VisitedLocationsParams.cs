namespace WebApi.Hellpers
{
    public class VisitedLocationsParams : PaginationParams
    {
       public DateTime StartDateTime { get; set; } = DateTime.MinValue;
       public DateTime EndDateTime { get; set; } = DateTime.MaxValue;

    }
}
