using WebApi.Enums;

namespace WebApi.Hellpers
{
    public class AnimalParams : PaginationParams
    {
        public DateTime StartDateTime { get; set; } = DateTime.MinValue;
        public DateTime EndDateTime { get; set; } = DateTime.MaxValue;
        public int ChipperId { get; set; } = 0 ;
        public int ChippingLocationId { get; set;} = 0;
        public LifeStatusEnum? LifeStatus { get; set; }
        public GenderEnum? Gender { get; set; }
    }
}
