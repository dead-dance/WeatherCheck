using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class LocalTemperature : BaseEntity
    {
        public DateTime TempDate { get; set; }
        public TimeOnly TempTime { get; set; }
        [MaxLength(60)]
        public string DistName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Temperature { get; set; }
    }
}
