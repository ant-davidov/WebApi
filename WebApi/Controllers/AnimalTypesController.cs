using AutoMapper;
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
        private readonly IMapper _mapper;
        public AnimalTypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{id?}")]
        public async Task<ActionResult<AnimalTypeDTO>> GetById(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(id.Value);
            if (type == null) return NotFound();
            return _mapper.Map<AnimalTypeDTO>(type) ;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AnimalTypeDTO>> Add([FromBody] AnimalTypeDTO type)
        {
            if (!ModelState.IsValid) return BadRequest();
            type.Id = 0;
            var tempAnimalType = _mapper.Map<AnimalType>(type);
            var typeInList = new List<long> { tempAnimalType.Id };
            if (_unitOfWork.AnimalTypeRepository.AllTypesExistsById(typeInList)) return Conflict();
            _unitOfWork.AnimalTypeRepository.AddAnimalType(_mapper.Map<AnimalType>(type));
            await _unitOfWork.Complete();
            return Created("./type",  await _unitOfWork.AnimalTypeRepository.GetAnimalTypeByTypeAsync(type.Type));
        }

        [HttpPut("{id?}")]
        public async Task<ActionResult<AnimalTypeDTO>> Update(int? id,[FromBody] AnimalTypeDTO updateType)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(id.Value);
            if (null == type) return NotFound();
            var tempAnimalType = _mapper.Map<AnimalType>(type);
            var typeInList = new List<long> { tempAnimalType.Id };
            if (_unitOfWork.AnimalTypeRepository.AllTypesExistsById(typeInList)) return Conflict();
            type.Type = updateType.Type;
            _unitOfWork.AnimalTypeRepository.UpdateAnimalType(type);
            await _unitOfWork.Complete();
            return _mapper.Map<AnimalTypeDTO>(type);    
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
