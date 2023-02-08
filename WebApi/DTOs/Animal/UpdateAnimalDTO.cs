using System.ComponentModel.DataAnnotations;
using WebApi.Enums;

namespace WebApi.DTOs.Animal
{
    public class UpdateAnimalDTO
    {
       
        [Range(float.Epsilon, float.PositiveInfinity)]
        public float Weight { get; set; }
        [Range(float.Epsilon, float.PositiveInfinity)]
        public float Length { get; set; }
        [Range(float.Epsilon, float.PositiveInfinity)]
        public float Height { get; set; }
        public LifeStatusEnum LifeStatus { get; set; }
        public GenderEnum Gender { get; set; }
        public int ChipperId { get; set; }
        [Range(1, long.MaxValue)]
        public DateTime? DeathDateTime { get; set; }
        public long ChippingLocationId { get; set; }
    }
}
