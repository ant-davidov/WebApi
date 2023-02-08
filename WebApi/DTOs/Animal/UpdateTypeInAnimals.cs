using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs.Animal
{
    public class UpdateTypeInAnimals
    {   [Range(1, long.MaxValue)]
        public long OldTypeId {get; set;}
        [Range(1, long.MaxValue)]
        public long newTypeId {get; set;}
    }
}