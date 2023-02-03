using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
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
       // [HttpPost]
        //public async Task<ActionResult<Animal>> Add([FromBody] AddAnimalDTO addAnimalDTO)
        //{
        //    if(!ModelState.IsValid) return BadRequest();
        //    if (addAnimalDTO.AnimalTypes.Any(x => x == null)) return BadRequest();

        //}
    }
}
