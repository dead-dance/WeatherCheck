using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherCheckAPI.DTO;

namespace WeatherCheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalTempController : ControllerBase
    {

        private readonly IGenericRepository<LocalTemperature> _localTempRepo;

        public LocalTempController(IGenericRepository<LocalTemperature> localTempRepo)
        {
            _localTempRepo = localTempRepo;
        }

        [HttpPost("CreateTemperature")]
        public ActionResult<LocalTemperatureDTO> SaveLocalTemp(LocalTemperature setDto)
        {
            var lt = new LocalTemperature
            {
                TempDate = setDto.TempDate,
                TempTime = setDto.TempTime,
                DistName = setDto.DistName,
                Latitude = setDto.Latitude,
                Longitude = setDto.Longitude,
                SetOn = DateTimeOffset.Now
            };

            _localTempRepo.Add(lt);
            _localTempRepo.Savechange();

            return new LocalTemperatureDTO
            {
                Id = lt.Id,
                TempDate = lt.TempDate,
                TempTime = lt.TempTime,
                DistName = lt.DistName,
                Latitude = lt.Latitude,
                Longitude = lt.Longitude,
            };
        }

    }
}
