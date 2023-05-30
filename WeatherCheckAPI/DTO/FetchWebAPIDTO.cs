namespace WeatherCheckAPI.DTO
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CurrentWeather
    {
        public string? time { get; set; }
        public double temperature { get; set; }
        public int weathercode { get; set; }
        public double windspeed { get; set; }
        public int winddirection { get; set; }
    }

    public class Hourly
    {
        public List<string?> time { get; set; }
        public List<double> temperature_2m { get; set; }
    }

    public class HourlyUnits
    {
        public string? temperature_2m { get; set; }
    }

    public class FetchWebAPIDTO
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double elevation { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string? timezone { get; set; }
        public string? timezone_abbreviation { get; set; }
        public required Hourly hourly { get; set; }
        public required HourlyUnits hourly_units { get; set; }
        public required CurrentWeather current_weather { get; set; }
    }


}
