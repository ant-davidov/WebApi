using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Controllers
{   
    public class AnimalTypesController :BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnimalTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet("{id?}")]
        public async Task<ActionResult<AnimalType>> GetById(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var account = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(id.Value);
            if (account == null) return NotFound();
            return account;
        }
        [HttpPost]
        public async Task<ActionResult<AnimalType>> Add([FromBody] AnimalType type)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (await _unitOfWork.AnimalTypeRepository.TypeExists(type.Type)) return Conflict();
            type.Id = 1;
            _unitOfWork.AnimalTypeRepository.AddAnimalType(type);
            await _unitOfWork.Complete();
            return Created("./type",  await _unitOfWork.AnimalTypeRepository.GetAnimalTypeByTypeAsync(type.Type));
        }

        [HttpPut("{id?}")]
        public async Task<ActionResult<AnimalType>> Update(int? id,[FromBody] AnimalType updateType)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(id.Value);
            if (null == type) return NotFound();
            if (await _unitOfWork.AnimalTypeRepository.TypeExists(type.Type)) return Conflict();
            type.Type = updateType.Type;
            _unitOfWork.AnimalTypeRepository.UpdateAnimalType(type);
            await _unitOfWork.Complete();
            return type;    
        }

        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(id.Value);
            if (null == type) return NotFound();
            _unitOfWork.AnimalTypeRepository.DeleteAnimalType(type);
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
