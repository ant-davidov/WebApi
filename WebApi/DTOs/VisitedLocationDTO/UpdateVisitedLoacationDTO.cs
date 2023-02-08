using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs.VisitLocationDTO
{
    public class UpdateVisitedLoacationDTO
    {   [Range(1, long.MaxValue)]
        public long visitedLocationPointId {get; set;}
        [Range(1, long.MaxValue)]
        public long locationPointId {get; set;}
    }
}
