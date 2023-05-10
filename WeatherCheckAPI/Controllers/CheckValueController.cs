using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using WeatherCheckAPI.DTO;
using static System.Net.Mime.MediaTypeNames;

namespace WeatherCheckAPI.Controllers
{
    public class CheckValueController : BaseApiController
    {
        private List<LocalTemperatureDTO> tempList = new List<LocalTemperatureDTO>();
        private readonly ILogger<CheckValueController> _logger;
        private readonly IGenericRepository<Districts> _distRepo;

        public CheckValueController(ILogger<CheckValueController> logger, IGenericRepository<Districts> distRepo)
        {
            _logger = logger;
            _distRepo = distRepo;
        }

        [HttpGet("GetCoolestDistrict")]
        public async Task<object> GetDataForAllDistrict()
        {
            try
            {
                string apiUrl = @"https://raw.githubusercontent.com/strativ-dev/technical-screening-test/main/bd-districts.json";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Developer");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    var distListJson = JsonConvert.DeserializeObject<DistRoot>(json);
                    var distList = distListJson.Districts;

                    var returnResult = GetFromAPI(distList);

                    return returnResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Generic Error getting district data.");
                return ex;
            }
        }

        private object GetFromAPI(List<DistrictDTO> dlist)
        {
            try
            {
                var tempList = new List<LocalTemperatureDTO>();

                foreach (var district in dlist)
                {
                    string apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={district.Lat}&longitude={district.Long}&timezone=Asia/Dhaka&hourly=temperature_2m&timeformat=unixtime";

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "Developer");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = client.GetAsync(apiUrl).Result;
                        response.EnsureSuccessStatusCode();

                        var responseContent = response.Content.ReadAsStringAsync().Result;

                        var lc = JsonConvert.DeserializeObject<FetchWebAPIDTO>(responseContent);

                        for (int i = 0; i < lc.hourly.time.Count; i++)
                        {
                            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(lc.hourly.time[i]));
                            if (dateTimeOffset.DateTime.TimeOfDay == new TimeSpan(14, 0, 0))
                            {
                                var time = dateTimeOffset.DateTime.TimeOfDay;
                                var temp = lc.hourly.temperature_2m[i];

                                tempList.Add(new LocalTemperatureDTO
                                {
                                    TempDate = dateTimeOffset.DateTime,
                                    TempTime = time,
                                    Latitude = lc.latitude,
                                    Longitude = lc.longitude,
                                    DistName = district.Name,
                                    Temperature = temp,
                                });
                            }
                        }
                    }
                }

                var result = tempList
                    .Where(m => m.TempDate.Date >= DateTime.Today && m.TempDate.Date <= DateTime.Today.AddDays(6))
                    .GroupBy(m => m.DistName)
                    .OrderBy(g => g.Average(i => i.Temperature))
                    .Take(10)
                    .Select(g => new
                    {
                        DistName = g.Key,
                        AvgTemp = g.Average(i => i.Temperature)
                    });

                return result;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var response = ex.Response;
                    var dataStream = response.GetResponseStream();
                    var reader = new StreamReader(dataStream);
                    var details = reader.ReadToEnd();

                    _logger.LogError(ex, "Error fetching data from Open Meteo.");
                    return ex;
                }
            }
            catch (Exception ex)
            {
                var response = ex.Message;
                _logger.LogError(ex, "Generic Error, please check.");
                return ex;
            }

            return "";
        }


        [HttpGet("GetComparedData/{flat}/{flong}/{tlat}/{tlong}/{travelDate}")]
        public async Task<string> GetTemperatureDifference(double flat, double flong, double tlat, double tlong, DateTime travelDate)
        {
            try
            {
                double fromTemperature = await GetIndividualData(flat, flong, travelDate);
                double toTemperature = await GetIndividualData(tlat, tlong, travelDate);

                double diff = fromTemperature - toTemperature;

                string message = (diff > 1) ? "It will be a Pleasent Trip For you" : "This trip is not recommended for you";

                return message;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error processing data.");
                return ex.Message;
            }
        }

        private async Task<double> GetIndividualData(double lat, double lon, DateTime date)
        {
            try
            {
                string apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&hourly=temperature_2m&forecast_days=1&start_date={date:yyyy-MM-dd}&end_date={date:yyyy-MM-dd}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Developer");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();

                    var Loc = JsonConvert.DeserializeObject<FetchWebAPIDTO>(responseContent);

                    for (int i = 0; i < Loc.hourly.time.Count; i++)
                    {
                        if (Loc.hourly.time[i].ToString().Substring(11) == "14:00")
                        {
                            return Loc.hourly.temperature_2m[i];
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting API data.");
                return 0;
            }
        }

    }
}