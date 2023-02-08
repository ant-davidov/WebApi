using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class AnimalVisitedLocationDTO
    {
        public long Id { get; set; }
        public string DateTimeOfVisitLocationPoint { get; set; }
        public long LocationPointId{ get; set;}
    }
}