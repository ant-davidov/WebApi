using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApi.DTOs.Animal;
using WebApi.Entities;

namespace WebApi.Hellpers
{
    public static class MapToReturnAnimal
    {
       public static ReturnAnimalDTO mapTo (Animal animal)
        {
            if ( animal == null ) return new ReturnAnimalDTO();
             var animalDTO = new ReturnAnimalDTO
            {
                Id = animal.Id,
                ChipperId = animal.Chipper.Id,
                Weight = animal.Weight,
                Length = animal.Length,
                Height= animal.Height,
                Gender = animal.Gender.ToString(),
                LifeStatus = animal.LifeStatus.ToString(),
                ChippingDateTime = mapDate(animal.ChippingDateTime),
                ChippingLocationId = animal.ChippingLocation.Id,
                DeathDateTime = mapDate(animal.DeathDateTime),
               // VisitedLocations = animal.VisitedLocations.Select(x=> x.LocationPoint.Id).ToList(),
                AnimalTypes = animal.AnimalTypes?.Select(x=> x.Id).ToList()            
            };
            if(animal.VisitedLocations != null)
               animalDTO.VisitedLocations = animal.VisitedLocations.Where(x=> x.LocationPoint != null).Select(y=> y.Id).ToList();
            if(animalDTO.VisitedLocations?.Count() == 0)
                animalDTO.VisitedLocations= null;
            return animalDTO;
        }
        private static string mapDate(DateTime? d)
        {
            if(null == d )
                return null;
           return d?.ToUniversalTime().ToString("s") +"Z";
        }
    }

}