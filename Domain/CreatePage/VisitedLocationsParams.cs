namespace Domain
{
    public class VisitedLocationsParams : PaginationParams
    {
        private DateTime endDateTime = DateTime.MaxValue;

        public DateTime StartDateTime { get; set; } = DateTime.MinValue;
        public DateTime EndDateTime { get => endDateTime; set => endDateTime = value.AddMinutes(1); }
    }
}
