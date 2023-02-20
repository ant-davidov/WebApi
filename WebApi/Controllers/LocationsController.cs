using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
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
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if (point == null) return NotFound();
            return point;
        }
        [HttpPost]
        public async Task<ActionResult<LocationPoint>> Add([FromBody] LocationPoint point)
        {
            if(! await _unitOfWork.LocationPointRepository.CheckCordinatesAsync(point))  return Conflict("Already have");
            point.Id = 0;
            _unitOfWork.LocationPointRepository.AddLocationPoint(point);
            await _unitOfWork.Complete();
            return Created("./locations", point);
        }

        [HttpPut("{id?}")]
        public async Task<ActionResult<LocationPoint>> Update(int? id, [FromBody] LocationPointDTO updatePoint)
        {
            if(id == null || id <= 0) return BadRequest("Incorrect id");
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if(null == point) return NotFound();
            var newPoint = _mapper.Map<LocationPoint>(updatePoint);
            if (!await _unitOfWork.LocationPointRepository.CheckCordinatesAsync(newPoint)) return Conflict();
            point = _mapper.Map(newPoint, point);
            _unitOfWork.LocationPointRepository.UpdateLocationPoint(point);
            await _unitOfWork.Complete();
            return point;
        }
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if (null == point) return NotFound();
            if(await _unitOfWork.LocationPointRepository.VisitedLocationExistAsync(point.Id))  return BadRequest("Have visited point");
            _unitOfWork.LocationPointRepository.DeleteLocationPoint(point);
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
