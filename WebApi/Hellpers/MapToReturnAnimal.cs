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
    public  class MapToReturnAnimal

    {    private Animal _animal;
         private ReturnAnimalDTO animalDTO;
        public MapToReturnAnimal(Animal animal)
        {
            _animal = animal;
            animalDTO = new ReturnAnimalDTO ();
        }
       public  ReturnAnimalDTO mapTo ()
        {
            if (  _animal == null ) return new ReturnAnimalDTO();
           
                animalDTO.Id = _animal.Id;
                animalDTO.ChipperId = _animal.Chipper?.Id ?? -1;
                animalDTO.Weight = _animal.Weight;
                animalDTO.Length = _animal.Length;
                animalDTO.Height= _animal.Height;
                animalDTO.Gender = _animal.Gender.ToString();
                animalDTO.LifeStatus = _animal.LifeStatus.ToString();
                animalDTO.ChippingDateTime = _animal.ChippingDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
                animalDTO.ChippingLocationId = _animal.ChippingLocation?.Id ?? -1 ;
                animalDTO.DeathDateTime = _animal.DeathDateTime?.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
               // VisitedLocations = animal.VisitedLocations.Select(x=> x.LocationPoint.Id).ToList(),
               // AnimalTypes = animal.AnimalTypes?.Select(x=> x.Id).ToList()  
               // VisitedLocations = animal.VisitedLocations.Select(x=> x.LocationPoint.Id).ToList(),
              //  animalDTO.AnimalTypes = new List<int>() ;
                return animalDTO;         
            // if(animal.VisitedLocations != null)
            //    animalDTO.VisitedLocations = animal.VisitedLocations.Where(x=> x.LocationPoint != null).Select(y=> y.Id).ToList();
            // if(animalDTO.VisitedLocations?.Count() == 0)
            //     animalDTO.VisitedLocations= null;
            
        }
       
    }

}