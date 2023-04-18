using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Domain.Enums;
using WebApi.Hellpers.Filter;

namespace WebApi.Controllers
{   
    [Route("animals/types")]
    public class AnimalTypesController :BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        public AnimalTypesController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet("{id?}")]
        public async Task<ActionResult<AnimalTypeDTO>> GetById(long? id)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(id.Value);
            if (type == null) return NotFound();
            return _mapper.Map<AnimalTypeDTO>(type) ;
        }
        [HttpPost]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN) + ", " + nameof(RoleEnum.CHIPPER))]
        public async Task<ActionResult<AnimalTypeDTO>> Add([FromBody] AnimalTypeDTO type)
        {
            if (await _unitOfWork.AnimalTypeRepository.GetAnimalTypeByTypeAsync(type.Type) != null) return Conflict("Already have");
            _unitOfWork.AnimalTypeRepository.AddAnimalType(_mapper.Map<AnimalType>(type));
            await _unitOfWork.Complete();   
            return Created("./type", await _unitOfWork.AnimalTypeRepository.GetAnimalTypeByTypeAsync(type.Type));
        }

        [HttpPut("{id?}")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN) + ", " + nameof(RoleEnum.CHIPPER))]
        public async Task<ActionResult<AnimalTypeDTO>> Update(int? id,[FromBody] AnimalTypeDTO updateType)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var typeNow = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(id.Value);
            if (null == typeNow) return NotFound();
            if (typeNow.Type != updateType.Type)
                if (await _unitOfWork.AnimalTypeRepository.GetAnimalTypeByTypeAsync(updateType.Type) != null) return Conflict("Already have");
            typeNow.Type = updateType.Type;
            _unitOfWork.AnimalTypeRepository.UpdateAnimalType(typeNow);
            await _unitOfWork.Complete();
            return _mapper.Map<AnimalTypeDTO>(typeNow);    
        }

        [HttpDelete("{id?}")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN))]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var type = await _unitOfWork.AnimalTypeRepository.GetAnimalTypeAsync(id.Value);
            if (null == type) return NotFound();
            if(await _unitOfWork.AnimalTypeRepository.AnimalsExistAsync(type.Id)) return  BadRequest("There are animals");
            _unitOfWork.AnimalTypeRepository.DeleteAnimalType(type);
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
