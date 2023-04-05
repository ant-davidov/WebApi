using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Secondary
{
    public class Coordinates
    {
        [JsonIgnore]
        public long Id { get; set; }
        [Range(-90F, 90F)]
        public double Latitude { get; set; }
        [Range(-180F, 180F)]
        public double Longitude { get; set; }
        [JsonIgnore]
        public long AreaId { get; set; }
        [JsonIgnore]
        public Area Area { get; set; }
    }
}