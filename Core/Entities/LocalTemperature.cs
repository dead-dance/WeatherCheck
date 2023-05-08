using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class LocalTemperature : BaseEntity
    {
        public DateTime TempDate { get; set; }
        public TimeOnly TempTime { get; set; }
        public string DistName { get; set; }
        public Int64 Latitude { get; set; }
        public Int64 Longitude { get; set; }
    }
}
