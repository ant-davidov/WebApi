using AutoMapper;
using Domain;
using Domain.DTOs;
using Domain.DTOs.VisitLocationDTO;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("animals/")]
    public class VisitedLocationsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        public VisitedLocationsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet("{id?}/locations")]
        public async Task<ActionResult<IEnumerable<AnimalVisitedLocation>>> Search(long? id, [FromQuery] VisitedLocationsParams locationsParams)
        {
            
            if (id == null || id <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            var dtos = await _unitOfWork.AnimalVisitedLocationRepository.GetAnimalVisitedLocationByParametersRepositoryAsync(id.Value, locationsParams);
            return Ok(dtos);     
        }

        [HttpPost("{id?}/locations/{pointId?}")]
        public async Task<ActionResult<AnimalVisitedLocationDTO>> Add(long? id, long? pointId)
        {
            #region Validation
            if (id == null || id <= 0 || pointId <= 0) return BadRequest("Invalid id");
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            var authorizedAccount = await _userManager.FindByEmailAsync(emailAuthorizedAccount); 
            //if(authorizedAccount.Role == Domain.Enums.RoleEnum.USER) return Forbid();
        
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            if (LifeStatusEnum.DEAD == animal.LifeStatus) return BadRequest("Animal is dead");
            if (animal.VisitedLocations == null || animal.VisitedLocations.Count < 1)
                if (animal.ChippingLocation.Id == pointId)
                    return BadRequest("Already here");
            var lastVisitedPoint = animal.VisitedLocations.LastOrDefault();
            if (lastVisitedPoint != null)
                if (lastVisitedPoint.LocationPoint.Id == pointId) return BadRequest("Already here");
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(pointId.Value);
            if (null == point) return NotFound("Location point not found"); 
            #endregion
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

            #region Validation
            if (id == null || id <= 0) return BadRequest("Invalid id");
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            var authorizedAccount = await _userManager.FindByEmailAsync(emailAuthorizedAccount); 
            //if(authorizedAccount.Role == Domain.Enums.RoleEnum.USER) return Forbid();
        
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (animal == null) return NotFound("Animal not found");
            if (animal.VisitedLocations == null || animal.VisitedLocations?.Count() < 1) return BadRequest("Visited point is null");
            if (updatePoint.locationPointId == animal.ChippingLocation.Id) return BadRequest("Already here");
            //checking neighboring points
            var visitedPoint = animal.VisitedLocations.FirstOrDefault(p => p.Id == updatePoint.visitedLocationPointId);
            if (visitedPoint == null) return NotFound("Not found vis point");
            var listVisitedLocations = animal.VisitedLocations.ToList();
            var index = listVisitedLocations.IndexOf(visitedPoint);
            var left = listVisitedLocations.Skip(index - 1).FirstOrDefault();
            if (left != visitedPoint && left.LocationPoint.Id == updatePoint.locationPointId)
                return BadRequest("Already here");
            var right = listVisitedLocations?.Skip(index + 1).FirstOrDefault();
            if (null != right && right.LocationPoint.Id == updatePoint.locationPointId)
                return BadRequest("Already here");
            // end
            var last = listVisitedLocations.LastOrDefault();
            if (last != null && last.LocationPoint.Id == updatePoint.locationPointId)
                return BadRequest("Already here");       
            var localPoint = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(updatePoint.locationPointId);
            if (localPoint == null) return NotFound("Not found location point"); 
            #endregion
            visitedPoint.LocationPoint = localPoint;
            _unitOfWork.AnimalVisitedLocationRepository.UpdateAnimalVisitedLocationRepository(visitedPoint);
            await _unitOfWork.Complete();
            var dto = _mapper.Map<AnimalVisitedLocationDTO>(visitedPoint);
            return dto; 
        }

         [HttpDelete("{id?}/locations/{visitedPointId?}")]
         public async Task<ActionResult<AnimalVisitedLocation>> Delete (long? id, long? visitedPointId)
         {
            #region Validation
            if (id == null || id <= 0 || visitedPointId == null || visitedPointId <= 0) return BadRequest();
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            var authorizedAccount = await _userManager.FindByEmailAsync(emailAuthorizedAccount); 
            //if(authorizedAccount.Role != Domain.Enums.RoleEnum.ADMIN) return Forbid();
        
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            if (animal.VisitedLocations == null || animal.VisitedLocations?.Count() < 1) return BadRequest("Visited point is null");
            var visitedPoint = animal.VisitedLocations.FirstOrDefault(p => p.Id == visitedPointId);
            if (visitedPoint == null) return NotFound("Not found visited point");
            animal.VisitedLocations.Remove(visitedPoint);
            var first = animal.VisitedLocations.FirstOrDefault();
            if (first != null && first.LocationPoint.Id == animal.ChippingLocation.Id)
                animal.VisitedLocations.Remove(first);    
            #endregion
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            return Ok();
         }

    }
}
