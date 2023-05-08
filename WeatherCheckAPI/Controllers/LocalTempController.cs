using AutoMapper;
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
        private readonly IMapper _mapper;

        public LocalTempController(IGenericRepository<LocalTemperature> localTempRepo, IMapper mapper)
        {
            _localTempRepo = localTempRepo;
            _mapper = mapper;
        }

        [HttpPost("CreateTemperature")]
        public ActionResult<LocalTemperatureDTO> SaveLocalTemp(LocalTemperatureDTO setDto)
        {
            try
            {
                var lt = _mapper.Map<LocalTemperature>(setDto);
                lt.SetOn = DateTimeOffset.Now;

                _localTempRepo.Add(lt);
                _localTempRepo.Savechange();

                var result = _mapper.Map<LocalTemperatureDTO>(lt);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error occurred while creating an order.");
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
