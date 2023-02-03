using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class AnimalType
    {
        private string type;
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Type { get => type; set => type = value?.ToLower().Trim(); }
        public ICollection<Animal> Animal { get; set; }
    }
}
