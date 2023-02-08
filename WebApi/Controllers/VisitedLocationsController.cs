using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
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
           private readonly IMapper _mapper;
        public VisitedLocationsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
             _mapper = mapper;
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
            query = query.Where(x => DateTime.Compare(x.DateTimeOfVisitLocationPoint, locationsParams.EndDateTime) <= 0);
            query = query.OrderBy(x => x.DateTimeOfVisitLocationPoint);
            var points =  PageList<AnimalVisitedLocation>.Create(query, locationsParams.From, locationsParams.Size); 
            var dtos = new List<AnimalVisitedLocationDTO>();
            foreach (var i in points)
               dtos.Add(_mapper.Map<AnimalVisitedLocationDTO>(i));
            return Ok(dtos);
           
        }

        [HttpPost("{id?}/locations/{pointId?}")]
        public async Task<ActionResult<AnimalVisitedLocationDTO>> Add(long? id, long? pointId)
        {   
            if (id == null || id <= 0 || pointId <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            if (LifeStatusEnum.DEAD == animal.LifeStatus) return BadRequest("Animal is dead");
            if (animal.VisitedLocations == null || animal.VisitedLocations.Count < 1)
                if(animal.ChippingLocation.Id == pointId) 
                    return BadRequest("Already here");
            var lastVisitedPoint =animal.VisitedLocations.LastOrDefault();
            if (lastVisitedPoint != null)
                if(lastVisitedPoint.LocationPoint.Id == pointId) return BadRequest("Already here");     
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
            var dto = _mapper.Map<AnimalVisitedLocationDTO>(visitedPoint);
            return Created("/create",dto);
       
       
           
            
            

        }
        [HttpPut("{id?}/locations")]
        public async Task<ActionResult<AnimalVisitedLocationDTO>> Update(long? id, [FromBody] UpdateVisitedLoacationDTO updatePoint)
        {
            
            if (id == null || id <= 0 ) return BadRequest("Invalid id");
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            if (animal.VisitedLocations == null || animal.VisitedLocations?.Count() < 1) return BadRequest("vis loc is null");
            if(updatePoint.locationPointId == animal.ChippingLocation.Id) return BadRequest("Already here");

            var visitedPoint = animal.VisitedLocations.FirstOrDefault(p=> p.Id == updatePoint.visitedLocationPointId);
            if (visitedPoint == null) return NotFound("Not found vis point");
            var listVisitedLocations = animal.VisitedLocations.ToList();
            var index = listVisitedLocations.IndexOf(visitedPoint);
            var first = listVisitedLocations.Skip(index-1).FirstOrDefault();
            if(first != visitedPoint && first.LocationPoint.Id == updatePoint.locationPointId)
                    return BadRequest("Already here");
            var second = listVisitedLocations?.Skip(index+1).FirstOrDefault();
            if(null != second && second.LocationPoint.Id == updatePoint.locationPointId)
                    return BadRequest("Already here");
            var last =listVisitedLocations.LastOrDefault();
            if (last != null && last.LocationPoint.Id == updatePoint.locationPointId)
                    return BadRequest("Already here");


            var localPoint = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(updatePoint.locationPointId);
            if(localPoint == null ) return NotFound("Not found location point");
            visitedPoint.LocationPoint = localPoint;
            _unitOfWork.AnimalVisitedLocationRepository.UpdateAnimalVisitedLocationRepository(visitedPoint);
            await _unitOfWork.Complete();
            var dto = _mapper.Map<AnimalVisitedLocationDTO>(visitedPoint);
            return dto;
           
          
            
        }

         [HttpDelete("{id?}/locations/{visitedPointId?}")]
         public async Task<ActionResult<AnimalVisitedLocation>> Delete (long? id, long? visitedPointId)
         {
            if(id == null || id <= 0 || visitedPointId == null || visitedPointId <= 0 )return BadRequest();
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if(null == animal) return NotFound("Animal not found");
            if (animal.VisitedLocations == null || animal.VisitedLocations?.Count() < 1) return BadRequest("vis loc is null");
            var visitedPoint = animal.VisitedLocations.FirstOrDefault(p=> p.Id == visitedPointId);
            if (visitedPoint == null) return NotFound("Not found vis point");
            animal.VisitedLocations.Remove(visitedPoint);
            var first = animal.VisitedLocations.FirstOrDefault();
            if(first != null && first.LocationPoint.Id == animal.ChippingLocation.Id)
            animal.VisitedLocations.Remove(first);           
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            return Ok();
         }

    }
}
