using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class LocationsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LocationsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id?}")]
        public async Task<ActionResult<LocationPoint>> GetById(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if (point == null) return NotFound();
            return point;
        }
        [HttpPost]
        public async Task<ActionResult<LocationPoint>> Add([FromBody] LocationPoint point)
        {
            if(!_unitOfWork.LocationPointRepository.CheckCordinatesAsync(point).Result)  return Conflict();
            point.Id = 0;
            _unitOfWork.LocationPointRepository.AddLocationPoint(point);
            await _unitOfWork.Complete();
            return Created("./locations", point);
        }

        [HttpPut("{id?}")]
        public async Task<ActionResult<LocationPoint>> Update(int? id, [FromBody] LocationPoint updatePoint)
        {
            if(id == null || id <= 0) return BadRequest();
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if(null == point) return NotFound();
            if (!await _unitOfWork.LocationPointRepository.CheckCordinatesAsync(updatePoint)) return Conflict();
            point = _mapper.Map(updatePoint, point);
            _unitOfWork.LocationPointRepository.UpdateLocationPoint(point);
            await _unitOfWork.Complete();
            return point;
        }
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if (null == point) return NotFound();
            if(await _unitOfWork.LocationPointRepository.VisitedLocationExistAsync(point.Id))  return BadRequest("Have visited point");
            _unitOfWork.LocationPointRepository.DeleteLocationPoint(point);
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
