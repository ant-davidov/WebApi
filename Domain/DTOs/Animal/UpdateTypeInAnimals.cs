using System.ComponentModel.DataAnnotations;


namespace Domain.DTOs.Animal
{
    public class UpdateTypeInAnimals
    {   [Range(1, long.MaxValue)]
        public long OldTypeId {get; set;}
        [Range(1, long.MaxValue)]
        public long newTypeId {get; set;}
    }
}