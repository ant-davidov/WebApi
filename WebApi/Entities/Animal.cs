using WebApi.Enums;

namespace WebApi.Entities
{
    public class Animal
    {
        private LifeStatusEnum lifeStatus = LifeStatusEnum.ALIVE;
        public int Id { get; set; }
        public ICollection<AnimalType> AnimalTypes { get; set; }
        public float Weight { get; set; }
        public float Lenght { get; set; }
        public float Height { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime ChippingDateTime { get; set; }
        public int ChipperId { get; set; }
        public AnimalVisitedLocation ChippingLocation { get; set; }
        public ICollection<AnimalVisitedLocation> VisitedLocations { get; set; }
        public DateTime DeathDateTime { get; private set; }
        public LifeStatusEnum LifeStatus
        {
            get => lifeStatus;
            set
            {
                if (value == LifeStatusEnum.DEAD) DeathDateTime = DateTime.UtcNow;
                lifeStatus = value;
            }
        }
    }
}
