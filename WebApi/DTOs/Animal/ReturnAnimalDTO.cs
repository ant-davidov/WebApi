using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Enums;

namespace WebApi.DTOs.Animal
{
    public class ReturnAnimalDTO
    {
        private string chippingDateTime;

        public int Id { get; set; }
        public List<int> AnimalTypes { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; }
        public string LifeStatus { get; set; }
        public string ChippingDateTime { get => chippingDateTime; set => chippingDateTime = value; }
        public long ChipperId { get; set; }
        public long ChippingLocationId { get; set; }
        public List<long> VisitedLocations { get; set; }
        public string DeathDateTime { get; set; }

    }
}