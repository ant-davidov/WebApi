using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class LocationPointDTO
    {
        [Range(-90F, 90F)]
        public double Latitude { get; set; }
        [Range(-180F, 180F)]
        public double Longitude { get; set; }
    }
}