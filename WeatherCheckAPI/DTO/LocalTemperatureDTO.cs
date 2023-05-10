using System.ComponentModel.DataAnnotations;

namespace WeatherCheckAPI.DTO
{
    public class LocalTemperatureDTO
    {
        public DateTime TempDate { get; set; }
        public TimeSpan TempTime { get; set; }
        [MaxLength(60)]
        public string DistName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Temperature { get; set; }
    }
}
