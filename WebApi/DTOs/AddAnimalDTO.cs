using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
using WebApi.Enums;

namespace WebApi.DTOs
{
    public class AddAnimalDTO
    {
        [Required, MinLength(1)]
        public ICollection<AnimalType> AnimalTypes { get; set; }
        [Range(0, float.PositiveInfinity)]
        public float Weight { get; set; }
        [Range(0, float.PositiveInfinity)]
        public float Lenght { get; set; }
        [Range(0, float.PositiveInfinity)]
        public float Height { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime ChippingDateTime { get; set; }
        [MinLength(1)]
        public int ChipperId { get; set; }
        [MinLength(1)]
        public long ChippingLocationId { get; set; }     
    }
}
