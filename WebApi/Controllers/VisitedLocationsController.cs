using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.DTOs.VisitLocationDTO;
using WebApi.Entities;
using WebApi.Enums;
using WebApi.Hellpers;
using WebApi.Interfaces;

namespace WebApi.Controllers
{   [Route("animals/")]
    public class VisitedLocationsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public VisitedLocationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("{id?}/locations")]
        public async Task<ActionResult<IEnumerable<AnimalVisitedLocation>>> Search(long? id, [FromQuery] VisitedLocationsParams locationsParams)
        {
            if (id == null || id <= 0) return BadRequest("Invalid id");
            if (!ModelState.IsValid) return BadRequest("Invalid data in query");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");

            var query = animal.VisitedLocations.AsQueryable();
            query = query.Where(x => DateTime.Compare(x.DateTimeOfVisitLocationPoint, locationsParams.StartDateTime) >= 0);
            query = query.Where(x => DateTime.Compare(x.DateTimeOfVisitLocationPoint, locationsParams.StartDateTime) <= 0);
            query = query.OrderBy(x => x.DateTimeOfVisitLocationPoint);
            var points =  PageList<AnimalVisitedLocation>.Create(query, locationsParams.From, locationsParams.Size); // map
            var dtos = new List<AnimalVisitedLocationDTO>();
            foreach (var i in points)
                dtos.Add(MapVisLoc.MapTo(i));
            

            return Ok(dtos);
        }

        [HttpPost("{id?}/locations/{pointId?}")]
        public async Task<ActionResult<object>> Add(long? id, long? pointId)
        {   try{
            if (id == null || id <= 0 || pointId <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            if (LifeStatusEnum.DEAD == animal.LifeStatus) return BadRequest("Animal is dead");
            if (animal.VisitedLocations != null || animal.VisitedLocations?.Count() > 0) 
                if (animal.VisitedLocations.Any(x=> x?.LocationPoint?.Id == pointId)) return BadRequest("Attempt to add twice");
           // if (animal.ChippingLocation.Id == pointId) return BadRequest("Conflict with location point ");
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(pointId.Value);
            if (null == point) return NotFound("Location point not found");
            var visitedPoint = new AnimalVisitedLocation
            {
                DateTimeOfVisitLocationPoint = DateTime.UtcNow,
                LocationPoint = point
            };
            animal.VisitedLocations.Add(visitedPoint);
            _unitOfWork.AnimalRepository.Update(animal);
            
            await _unitOfWork.Complete();
            var user = new { id = visitedPoint.Id, dateTimeOfVisitLocationPoint = DateTime.UtcNow, LocationPointId = visitedPoint.LocationPoint.Id };
            return Created("/create",user);
        }
        catch(Exception e)
        {
            return Ok(e.ToString() + " " + e.Data.ToString() + " "+ e.Message.ToString());
        }
           
            
            

        }
        [HttpPut("{id?}/locations")]
        public async Task<ActionResult<AnimalVisitedLocation>> Update(long? id, [FromBody] UpdateVisitedLoacationDTO updatePoint)
        {
            if (id == null || id <= 0 ) return BadRequest("Invalid id");
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            if (animal.VisitedLocations == null || animal.VisitedLocations?.Count() < 1) return BadRequest("vis loc is null");
            if (animal.VisitedLocations.FirstOrDefault(p=> p.Id == updatePoint.visitedLocationPointId) == null) return BadRequest("Not found vis point");
            var visitedPoint = await _unitOfWork.AnimalVisitedLocationRepository.GetAnimalVisitedLocationRepositoryAsync(updatePoint.visitedLocationPointId);
            var localPoint = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(updatePoint.locationPointId);
            if(localPoint == null ) return BadRequest("Not found location point");
            visitedPoint.LocationPoint = localPoint;
            _unitOfWork.AnimalVisitedLocationRepository.UpdateAnimalVisitedLocationRepository(visitedPoint);
            await _unitOfWork.Complete();
            var user = new { id = visitedPoint.Id, dateTimeOfVisitLocationPoint = visitedPoint.DateTimeOfVisitLocationPoint, LocationPointId = visitedPoint.LocationPoint.Id };
            return Ok(user);
        }

         [HttpDelete("{id?}/locations/{visitedPointId?}")]
         public async Task<ActionResult<AnimalVisitedLocation>> Delete (long? id, long? visitedPointId)
         {
            if(id == null || id <= 0 || visitedPointId == null || visitedPointId <= 0 )return BadRequest();
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if(null == animal) return NotFound("Animal not found");
            if (animal.VisitedLocations == null || animal.VisitedLocations?.Count() < 1) return BadRequest("vis loc is null");
            var visitedPoint = animal.VisitedLocations.FirstOrDefault(p=> p.Id == visitedPointId);
            if (visitedPoint == null) return BadRequest("Not found vis point");
            animal.VisitedLocations.Remove(visitedPoint);
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            return Ok();

         }

    }
}
