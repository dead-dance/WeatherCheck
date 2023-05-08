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

        public DistrictController(IGenericRepository<Districts> distRepo)
        {
            _distRepo = distRepo;
        }

        [HttpPost("CreateDistrict")]
        public ActionResult<DistrictDTO> SaveBcds(DistrictDTO setDto)
        {
            var dst = new Districts
            {
                DivisionId = setDto.DivisionId,
                Name = setDto.Name,
                BnName = setDto.BnName,
                Latitude = setDto.Latitude,
                Longitude = setDto.Longitude,
                SetOn = DateTimeOffset.Now
            };

            _distRepo.Add(dst);
            _distRepo.Savechange();

            return new DistrictDTO
            {
                Id = dst.Id,
                DivisionId = dst.DivisionId,
                Name = dst.Name,
                BnName = dst.BnName,
                Latitude = dst.Latitude,
                Longitude = dst.Longitude
            };
        }

        [HttpPost("GetAllDistrict")]
        public async Task<IReadOnlyList<Districts>> GetDistricts()
        {
            var data = await _distRepo.ListAllAsync();
            return data;
        }
    }
}
