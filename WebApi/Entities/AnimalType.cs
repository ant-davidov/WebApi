using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Entities
{
    public class AnimalType
    {
        private string type;
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Type { get => type; set => type = value?.ToLower().Trim(); }
        [JsonIgnore]
        [ValidateNever]
        public ICollection<Animal> Animal { get; set; }
    }
}
