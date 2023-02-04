﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Animal;
using WebApi.Entities;
using WebApi.Hellpers;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    public class AnimalsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnimalsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet("{id?}")]
        public async Task<ActionResult<Animal>> GetById(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound();
            return animal;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Animal>>> Search([FromQuery] AnimalParams animalParams)
        {
            if (animalParams.ChipperId <= 0 || animalParams.ChippingLocationId <= 0 || null == animalParams.LifeStatus || null == animalParams.Gender) return BadRequest();

            return await _unitOfWork.AnimalRepository.GetAnimalsWitsParamsAsync(animalParams);
        }
        [HttpPost]
        public async Task<ActionResult<Animal>> Add([FromBody] AddAnimalDTO addAnimalDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            if (addAnimalDTO.AnimalTypesId.Any(x => x <= 0)) return BadRequest("Type id less than 1");
            if (!_unitOfWork.AnimalTypeRepository.AllTypesExistsById(addAnimalDTO.AnimalTypesId)) return BadRequest("One or more types of animals do not exist");
            var account = await _unitOfWork.AccountRepository.GetAccountAsync(addAnimalDTO.ChipperId);
            if (account == null) return NotFound("An account with this ID was not found");
            var location = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(addAnimalDTO.ChippingLocationId);
            if (location == null) return NotFound("There is no location with this id");
            var animal = new Animal();
            animal.Weight = addAnimalDTO.Weight;
            animal.Height = addAnimalDTO.Height;
            animal.Lenght = addAnimalDTO.Lenght;
            var b = await Task.WhenAll(addAnimalDTO.AnimalTypesId.Select(async x => await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(x)));
            animal.Gender = addAnimalDTO.Gender;
            animal.ChippingLocation = location;
            animal.Chipper = account;
            animal.ChippingDateTime = DateTime.UtcNow;
            animal.AnimalTypes = b.ToList();
            var id = _unitOfWork.AnimalRepository.AddAnimal(animal);
            await _unitOfWork.Complete();
            
            return Created("./add",await _unitOfWork.AnimalRepository.GetAnimalAsync(id));
            // save
        }
        [HttpPut("{id?}")]
        public async Task<ActionResult<Animal>> Update(int? id,[FromBody] UpdateAnimalDTO updateAnimalDTO)
        {
            if (id == null || id <= 0) return BadRequest("Invalid id");
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            var account = await _unitOfWork.AccountRepository.GetAccountAsync(updateAnimalDTO.ChipperId);
            if (account == null) return NotFound("An account with this id was not found");
            var location = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(updateAnimalDTO.ChippingLocationId);
            if (location == null) return NotFound("There is no location with this id");
            animal.Weight = updateAnimalDTO.Weight;
            animal.Height = updateAnimalDTO.Height;
            animal.Lenght = updateAnimalDTO.Lenght;
            animal.Gender = updateAnimalDTO.Gender;
            animal.LifeStatus = updateAnimalDTO.LifeStatus;
            animal.ChippingLocation = location;
            animal.Chipper = account;
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            return Ok(animal);
           
        }
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Invalid id");
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            if (null != animal.AnimalTypes) return BadRequest("Has visiting points");
            _unitOfWork.AnimalRepository.DeleteAnimal(animal);
            await _unitOfWork.Complete();
            return Ok();         
        }

        [HttpDelete("{id?}/types/{typeId?}")]
        public async Task<ActionResult<Animal>> AddType(long? id, long? typeId )
        {
            if (id == null || id <= 0 || typeId == null || typeId <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(typeId.Value);
            if (null == type) return NotFound("AnimalType not found");
            animal.AnimalTypes.Add(type);
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            return Ok();
        }
        [HttpPut("{id?}/types")]
        public async Task<ActionResult<Animal>> UpdateType(long? id,[FromBody] long? oldTypeId, [FromBody] long? newTypeId)
        {
            if ((id == null || id <= 0 || oldTypeId == null || oldTypeId <= 0 || newTypeId == null || newTypeId <= 0) return BadRequest("Invalid id");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            if (animal.AnimalTypes.FirstOrDefault(x => x.Id == oldTypeId.Value) == null) return BadRequest("The animal does not have the old type with this id");
            var oldType = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(oldTypeId.Value);
            //if (null == oldType) return BadRequest("Old type has a invalid id");
            if (animal.AnimalTypes.FirstOrDefault(x => x.Id == newTypeId.Value) != null) return Conflict("The new type is already there");
            var newType = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(newTypeId.Value);
            animal.AnimalTypes.Add(newType);
            animal.AnimalTypes.Remove(oldType);
            return Ok();
        }
        [HttpDelete("{id?}/types/{typeId}")]
        public async Task<ActionResult<Animal>> DeleteType(long? id,long? typeId)
        {
            if (id == null || id <= 0 || typeId == null || typeId <= 0) return BadRequest("Invalid id or typeId");
            var animal = await _unitOfWork.AnimalRepository.GetAnimalAsync(id.Value);
            if (null == animal) return NotFound("Animal not found");
            if (animal.AnimalTypes.Count == 1 && animal.AnimalTypes.All(x => x.Id == typeId)) return BadRequest("This is the only type");
            if (!animal.AnimalTypes.Any(x => x.Id == typeId.Value)) return NotFound("An animal with animalId does not have a type with typeId");
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(typeId.Value);
            if (null == type) return NotFound("Type not found"); // нужно ли это
            animal.AnimalTypes.Remove(type);
            _unitOfWork.AnimalRepository.Update(animal);
            await _unitOfWork.Complete();
            return animal; 
        }


    }
}
