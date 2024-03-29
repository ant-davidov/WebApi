﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class LocationPoint
    {
        [Key]
        public long Id { get; set; }
        [Range(-90F, 90F)]
        public double Latitude { get; set; }
        [Range(-180F, 180F)]
        public double Longitude { get; set; }
    }
}
