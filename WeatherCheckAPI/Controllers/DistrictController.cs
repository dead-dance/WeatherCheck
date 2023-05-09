using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WeatherCheckAPI.DTO;

namespace WeatherCheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IGenericRepository<Districts> _distRepo;
        private readonly ILogger<DistrictController> _logger;
        private readonly IMapper _mapper;

        public DistrictController(IGenericRepository<Districts> distRepo, IMapper mapper, ILogger<DistrictController> logger)
        {
            _distRepo = distRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("CreateDistrict")]
        public ActionResult<DistrictDTO> SaveDistrict(DistrictDTO setDto)
        {
            try
            {
                var dst = _mapper.Map<Districts>(setDto);
                dst.SetOn = DateTimeOffset.Now;

                _distRepo.Add(dst);
                _distRepo.Savechange();

                var result = _mapper.Map<DistrictDTO>(dst);
                return Ok(result);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while creating district.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("GetAllDistrict")]
        public async Task<IReadOnlyList<Districts>> GetDistricts()
        {
            var data = await _distRepo.ListAllAsync();
            return data;
        }
    }
}
