using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using System.Text.Json;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class LocationsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public LocationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            if (!ModelState.IsValid) return BadRequest();
            if(!_unitOfWork.LocationPointRepository.CheckCordinatesAsync(point).Result)  return Conflict(JsonSerializer.Serialize(point));
            point.Id = 0;
            _unitOfWork.LocationPointRepository.AddLocationPoint(point);
            await _unitOfWork.Complete();
            return Created("./locations", point);
        }

        [HttpPut("{id?}")]
        public async Task<ActionResult<LocationPoint>> Update(int? id, [FromBody] LocationPoint updatePoint)
        {
            if(id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if(null == point) return NotFound();
            if (!await _unitOfWork.LocationPointRepository.CheckCordinatesAsync(updatePoint)) return Conflict();
            point.Latitude = updatePoint.Latitude;
            point.Longitude = updatePoint.Longitude;
            _unitOfWork.LocationPointRepository.UpdateLocationPoint(point);
            await _unitOfWork.Complete();
            return point;
        }
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if (null == point) return NotFound();
            if(await _unitOfWork.LocationPointRepository.VisitedLocationExistAsync(point.Id))  return BadRequest("Have bisited point");
            _unitOfWork.LocationPointRepository.DeleteLocationPoint(point);
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
