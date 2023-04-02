using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.VisitLocationDTO
{
    public class UpdateVisitedLoacationDTO
    {   [Range(1, long.MaxValue)]
        public long visitedLocationPointId {get; set;}
        [Range(1, long.MaxValue)]
        public long locationPointId {get; set;}
    }
}
