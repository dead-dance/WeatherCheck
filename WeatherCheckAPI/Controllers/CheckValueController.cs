using Core.Entities;
using Core.Interfaces;
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
        private readonly ILogger<CheckValueController> _logger;
        private readonly IGenericRepository<Districts> _distRepo;

        public CheckValueController(ILogger<CheckValueController> logger, IGenericRepository<Districts> distRepo)
        {
            _logger = logger;
            _distRepo = distRepo;
        }

        [HttpPost("GetCoolestDistrict/{districts}")]
         public async Task GetDataForAllDistrict(List<DistrictDTO> districts)
        //public async Task GetDataForAllDistrict(string districts)
        {
            //var distList = await _distRepo.ListAllAsync();

            //GetFromAPI(distList[0].Latitude, distList[0].Longitude, distList[0].Name);
            GetFromAPI(24.76, 90.41, "Mymensingh");

            //foreach (var u in distList) {
            //    GetFromAPI(u.Latitude, u.Longitude, u.Name);
            //}
        }


        [HttpGet]
        public void GetFromAPI(double lati, double longi, string distName)
        {

            string apiUrl = @"https://api.open-meteo.com/v1/forecast?latitude=" + lati + "&longitude=" + longi + "&timezone=Asia/Dhaka&hourly=temperature_2m&timeformat=unixtime";

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.UserAgent = "Developer";
            request.Accept = "true";

            try
            {
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream dataStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                var responseFromServer = reader.ReadToEnd();

                FetchWebAPIDTO? lc = JsonConvert.DeserializeObject<FetchWebAPIDTO>(responseFromServer);

                int i = 0;
                int cnt = 0;
                for (i = 0; i < lc.hourly.time.Count; i++)
                {
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(lc.hourly.time[i]));
                    double temp = lc.hourly.temperature_2m[i];
                    if(dateTimeOffset.DateTime.TimeOfDay.ToString() == "14:00:00")
                    {
                        cnt++;
                        //Console.WriteLine(dateTimeOffset.DateTime + "," + temp);
                    }                  
                }

                Console.WriteLine(cnt); ;
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
                }
            }
            catch (Exception tex)
            {
                if (tex.Message != null)
                {
                    var response = tex.Message;
                    _logger.LogError(tex, "Generic Error, please check.");
                }
            }
        }
    }
}
