using System.ComponentModel.DataAnnotations;
using WebApi.Enums;

namespace WebApi.DTOs.Animal
{
    public class AddAnimalDTO
    {
        [Required, MinLength(1)]
        public ICollection<long> AnimalTypes { get; set; }
        [Range(float.Epsilon, float.PositiveInfinity)]
        public float Weight { get; set; }
        [Range(float.Epsilon, float.PositiveInfinity)]
        public float Length { get; set; }
        [Range(float.Epsilon, float.PositiveInfinity)]
        public float Height { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime ChippingDateTime { get; set; }
        [Range(1, int.MaxValue)]
        public int ChipperId { get; set; }
        [Range(1, long.MaxValue)]
        public long ChippingLocationId { get; set; }
    }
}
