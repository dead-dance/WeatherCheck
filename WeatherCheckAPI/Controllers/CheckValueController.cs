using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
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
        //public async Task GetDataForAllDistrict(string districts)
        {

            string apiUrl = @"https://raw.githubusercontent.com/strativ-dev/technical-screening-test/main/bd-districts.json";

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.UserAgent = "Developer";
            request.Accept = "true";

            try
            {
                string json = (new WebClient()).DownloadString(apiUrl);

                var distListJson = JsonConvert.DeserializeObject<DistRoot>(json);
                var distList = distListJson.Districts;

                var returnResult = GetFromAPI(distList);

                return returnResult;
            }
            catch (Exception tex)
            {
                if (tex.Message != null)
                {
                    var response = tex.Message;
                    _logger.LogError(tex, "Generic Error getting district data.");
                }

                return tex;
            }
        }

        [HttpGet]
        public object GetFromAPI(List<DistrictDTO> dlist)
        {
            try
            {
                foreach (var u in dlist)
                {
                    string apiUrl = @"https://api.open-meteo.com/v1/forecast?latitude=" + u.Lat + "&longitude=" + u.Long + "&timezone=Asia/Dhaka&hourly=temperature_2m&timeformat=unixtime";

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                    request.UserAgent = "Developer";
                    request.Accept = "true";
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                    System.IO.Stream dataStream = response.GetResponseStream();
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                    var responseFromServer = reader.ReadToEnd();

                    FetchWebAPIDTO? lc = JsonConvert.DeserializeObject<FetchWebAPIDTO>(responseFromServer);

                    int i = 0;
                    for (i = 0; i < lc.hourly.time.Count; i++)
                    {
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(lc.hourly.time[i]));
                        double temp = lc.hourly.temperature_2m[i];
                        if (dateTimeOffset.DateTime.TimeOfDay.ToString() == "14:00:00")
                        {
                            TimeSpan time = TimeSpan.Parse(dateTimeOffset.DateTime.TimeOfDay.ToString());
                            tempList.Add(new LocalTemperatureDTO
                            {
                                TempDate = dateTimeOffset.DateTime,
                                TempTime = time,
                                Latitude = lc.latitude,
                                Longitude = lc.longitude,
                                DistName = u.Name,
                                Temperature = temp,
                            });
                        }
                    }
                }

                var result = (from m in tempList
                              where m.TempDate >= DateTime.Today && m.TempDate <= DateTime.Today.AddDays(6)
                              group m by m.DistName into g
                              orderby g.Average(i => i.Temperature) ascending
                              select new
                              {
                                  DistName = g.Key,
                                  AvgTemp = g.Average(i => i.Temperature)
                              }).Take(10);


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
            catch (Exception tex)
            {
                if (tex.Message != null)
                {
                    var response = tex.Message;
                    _logger.LogError(tex, "Generic Error, please check.");
                    return tex;
                }
            }

            return "";
        }


        [HttpGet("GetComparedData")]
        public async Task<object> GetTemperatureDifference()
        {
            
        }
    }
}
