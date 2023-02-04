using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Enums;
using WebApi.Hellpers;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class VisitedLocationsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public VisitedLocationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
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
            var points = await PageList<AnimalVisitedLocation>.CreateAsync(query, locationsParams.From, locationsParams.Size); // map
            return Ok();
        }

        [HttpPost("{id?}/locations/{pointId?}")]
        public async Task<ActionResult<AnimalVisitedLocation>> Add(long? id, long? pointId)
        {
            if (id == null || id <= 0 || pointId == null || pointId <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            if (LifeStatusEnum.DEAD == animal.LifeStatus) return BadRequest("Animal is dead");
            if (animal.ChippingLocation.Id == pointId) return BadRequest("Conflict with location point ");
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
            return (await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value)).VisitedLocations.FirstOrDefault(x => x.LocationPoint.Id == pointId);
        }
        [HttpPut("{id?}/locations}")]
        public async Task<ActionResult<IEnumerable<AnimalVisitedLocation>>> Update(long? id, [FromBody] long? visitedLocationPointId, [FromBody] long? locationPointId)
        {
            if (id == null || id <= 0 || visitedLocationPointId == null || visitedLocationPointId <= 0 || locationPointId == null || locationPointId <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            if (LifeStatusEnum.DEAD == animal.LifeStatus) return BadRequest("Animal is dead");
            return Ok();
        }

    }
}
