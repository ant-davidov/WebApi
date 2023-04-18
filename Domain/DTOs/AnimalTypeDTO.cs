using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class AnimalTypeDTO
    {
        private string type;
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Type { get => type; set => type = value?.ToLower().Trim(); }
    }
}
