
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class AnimalType
    {
        private string type;

        [Key]
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Type { get => type; set => type = value?.ToLower().Trim(); }
        [JsonIgnore]
        public ICollection<Animal> Animal { get; set; }
    }
}
