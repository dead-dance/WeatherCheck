using System.ComponentModel.DataAnnotations;

namespace WeatherCheckAPI.DTO
{
    public class LocalTemperatureDTO
    {
        public int Id { get; set; }
        public DateTime TempDate { get; set; }
        public TimeOnly TempTime { get; set; }
        [MaxLength(60)]
        public string DistName { get; set; }
        public Int64 Latitude { get; set; }
        public Int64 Longitude { get; set; }
    }
}
