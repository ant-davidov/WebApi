using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class AnimalTypeDTO
    {
        private string type;
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Type { get => type; set => type = value?.ToLower().Trim(); }
    }
}
