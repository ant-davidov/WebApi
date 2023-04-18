using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Interfaces;
using Domain.DTOs.Animal;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using WebApi.Hellpers.Filter;

namespace WebApi.Controllers
{
    public class AnimalsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        public AnimalsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<ReturnAnimalDTO>> GetById(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound();
            var animalDTO = _mapper.Map<ReturnAnimalDTO>(animal);
            return animalDTO;
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ReturnAnimalDTO>>> Search([FromQuery] AnimalParams animalParams)
        {

            var animals = await _unitOfWork.AnimalRepository.GetAnimalsWitsParamsAsync(animalParams);
            if (null == animals || animals.Count() < 1) return new List<ReturnAnimalDTO>();
            return Ok(animals);
        }

        [HttpPost]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN) + ", " + nameof(RoleEnum.CHIPPER))]
        public async Task<ActionResult<ReturnAnimalDTO>> Add([FromBody] AddAnimalDTO addAnimalDTO)
        {
            #region Validation
            if (addAnimalDTO.AnimalTypes.Any(x => x <= 0)) return BadRequest("Type id less than 1");
            if (!_unitOfWork.AnimalTypeRepository.AllTypesExistsById(addAnimalDTO.AnimalTypes)) return NotFound("One or more types of animals do not exist");
            var account = await _userManager.Users.FirstOrDefaultAsync(a => a.Id == addAnimalDTO.ChipperId);
            if (account == null) return NotFound("An account with this ID was not found");
            var location = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(addAnimalDTO.ChippingLocationId);
            if (location == null) return NotFound("There is no location with this id");
            #endregion
            var animal = _mapper.Map<Animal>(addAnimalDTO);
            var types = addAnimalDTO.AnimalTypes.Select(x => _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(x).Result);
            animal.ChippingLocation = location;
            animal.Chipper = account;
            animal.AnimalTypes = types.ToList();
            _unitOfWork.AnimalRepository.AddAnimal(animal);
            await _unitOfWork.Complete();
            var animalDTO = _mapper.Map<ReturnAnimalDTO>(animal);
            return Created("./add", animalDTO);
        }


        [HttpPut("{id?}")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN) + ", " + nameof(RoleEnum.CHIPPER))]
        public async Task<ActionResult<ReturnAnimalDTO>> Update(int? id, [FromBody] UpdateAnimalDTO updateAnimalDTO)
        {
            #region Validation
            if (id == null || id <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            var account = await _userManager.Users.FirstOrDefaultAsync(a => a.Id == updateAnimalDTO.ChipperId);
            if (account == null) return NotFound("An account with this id was not found");
            var location = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(updateAnimalDTO.ChippingLocationId);
            if (location == null) return NotFound("There is no location with this id");
            var visitedPoint = animal.VisitedLocations;
            if (visitedPoint != null && visitedPoint.Count > 0)
            {
                var first = visitedPoint.FirstOrDefault();
                if (first != null)
                    if (first.LocationPoint.Id == updateAnimalDTO.ChippingLocationId)
                        return BadRequest("Setting the chippingLocationId to the first visited point");
            }
            #endregion
            animal.ChippingLocation = location;
            animal.Chipper = account;
            _mapper.Map<UpdateAnimalDTO, Animal>(updateAnimalDTO, animal);
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            var animalDTO = _mapper.Map<ReturnAnimalDTO>(animal);
            return Ok(animalDTO);

        }


        [HttpDelete("{id?}")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN))]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            if (null != animal.VisitedLocations && animal.VisitedLocations.Count > 0
                            && animal.VisitedLocations.FirstOrDefault()?.LocationPoint.Id != animal.ChippingLocation.Id)
                return BadRequest();
            _unitOfWork.AnimalRepository.DeleteAnimal(animal);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpPost("{id?}/types/{typeId?}")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN) + ", " + nameof(RoleEnum.CHIPPER))]
        public async Task<ActionResult<ReturnAnimalDTO>> AddType(long? id, long? typeId)
        {
            #region Validation
            if (id == null || id <= 0 || typeId == null || typeId <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(typeId.Value);
            if (null == type) return NotFound("AnimalType not found");
            animal.AnimalTypes.Add(type);
            #endregion
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            var animalDTO = _mapper.Map<ReturnAnimalDTO>(animal);
            return Created("./add", animalDTO);
        }


        [HttpPut("{id?}/types")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN) + ", " + nameof(RoleEnum.CHIPPER))]
        public async Task<ActionResult<ReturnAnimalDTO>> UpdateType(long? id, [FromBody] UpdateTypeInAnimals updateTypeInAnimals)
        {
            #region Validation
            if (id == null || id <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            if (animal.AnimalTypes.FirstOrDefault(x => x.Id == updateTypeInAnimals.OldTypeId) == null) return NotFound("The animal does not have the old type with this id");
            var oldType = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(updateTypeInAnimals.OldTypeId);
            if (animal.AnimalTypes.FirstOrDefault(x => x.Id == updateTypeInAnimals.newTypeId) != null) return Conflict("The new type is already there");
            var newType = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(updateTypeInAnimals.newTypeId);
            if (null == newType) return NotFound("Not found new type");
            #endregion
            animal.AnimalTypes.Add(newType);
            animal.AnimalTypes.Remove(oldType);
            var animalDTO = _mapper.Map<ReturnAnimalDTO>(animal);
            return Ok(animalDTO);
        }


        [HttpDelete("{id?}/types/{typeId}")]
        [CustomAuthorize(roles: nameof(RoleEnum.CHIPPER) + ", " + nameof(RoleEnum.ADMIN))]
        public async Task<ActionResult<ReturnAnimalDTO>> DeleteType(long? id, long? typeId)
        {
            #region Validation
            if (id == null || id <= 0 || typeId == null || typeId <= 0) return BadRequest("Invalid id or typeId");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            if (animal.AnimalTypes.Count == 1 && animal.AnimalTypes.All(x => x.Id == typeId)) return BadRequest("This is the only type");
            if (!animal.AnimalTypes.Any(x => x.Id == typeId.Value)) return NotFound("An animal with animalId does not have a type with typeId");
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(typeId.Value);
            if (null == type) return NotFound("Type not found");
            #endregion
            animal.AnimalTypes.Remove(type);
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            var animalDTO = _mapper.Map<ReturnAnimalDTO>(animal);
            return animalDTO;
        }



    }
}
