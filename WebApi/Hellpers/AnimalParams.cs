using WebApi.Enums;

namespace WebApi.Hellpers
{
    public class AnimalParams : PaginationParams
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int ChipperId { get; set; }
        public int ChippingLocationId { get; set;}
        public LifeStatusEnum LifeStatus { get; set; }
        public GenderEnum Gender { get; set; }

    }
}
