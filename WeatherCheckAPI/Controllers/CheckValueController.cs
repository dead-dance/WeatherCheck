using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WeatherCheckAPI.DTO;
using static System.Net.Mime.MediaTypeNames;

namespace WeatherCheckAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CheckValueController : ControllerBase
    {
        private readonly ILogger<CheckValueController> _logger;

        public CheckValueController(ILogger<CheckValueController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task GetFromAPI()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://api.open-meteo.com/v1/forecast?latitude=23.71&longitude=90.41&timezone=Asia/Dhaka&hourly=temperature_2m&timeformat=unixtime");
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
