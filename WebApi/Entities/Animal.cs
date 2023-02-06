using System.Diagnostics.CodeAnalysis;
using WebApi.Enums;

namespace WebApi.Entities
{
    public class Animal
    {
        private LifeStatusEnum lifeStatus = LifeStatusEnum.ALIVE;
        public int Id { get; set; }
        public ICollection<AnimalType> AnimalTypes { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime ChippingDateTime { get; set; }
        public Account Chipper { get; set; }
        public LocationPoint ChippingLocation { get; set; }
        public ICollection<AnimalVisitedLocation> VisitedLocations { get; set; } = null;
        public DateTime? DeathDateTime { get; private set; } = null;
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
