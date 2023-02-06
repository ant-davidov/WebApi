using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Hellpers
{
    public static class MapVisLoc
    {
        public static AnimalVisitedLocationDTO MapTo(AnimalVisitedLocation loc)
        {
            var dto = new AnimalVisitedLocationDTO
            {
                Id = 1,
                DateTimeOfVisitLocationPoint = DateTime.Now,
                LocationPointId = 1
            };
            return dto;
        }
    }
}