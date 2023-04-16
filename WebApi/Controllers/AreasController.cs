using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WebApi.Hellpers;
using Domain.DTOs.Animal;
using Microsoft.EntityFrameworkCore;
using Domain.CreatePage;
using Domain.Entities.Secondary;

namespace WebApi.Controllers
{
    public class AreasController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        public AreasController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<Area>> GetById(long? id)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var point = await _unitOfWork.AreaRepository.GetAreaAsync(id.Value);
            if (point == null) return NotFound();
            return point;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Area>> Add([FromBody] AreaDTO addAreaDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (!addAreaDTO.Polygon.IsSimple) return BadRequest("ti peresek grannsu");
                if (await _unitOfWork.AreaRepository.DoesIntersectWithExistingAreas(addAreaDTO)) return BadRequest("mishaet drugay zona");
                if (await _unitOfWork.AreaRepository.CheckPolygonIntersectionAsync(addAreaDTO)) return BadRequest("you inside");
                var area = _mapper.Map<Area>(addAreaDTO);
                _unitOfWork.AreaRepository.AddArea(area);
                await _unitOfWork.Complete();
                return Created("url",area);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message +" " + ex.ToString());
            }

        }

        [HttpPut("{id?}")]
        public async Task<ActionResult<ReturnAnimalDTO>> Update(int? id, [FromBody] AreaDTO addAreaDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (!CheckingForConvexPolygon.IsConvexPolygon(addAreaDTO)) return BadRequest("ti peresek grannsu");
                if (await _unitOfWork.AreaRepository.DoesIntersectWithExistingAreas(addAreaDTO,id.Value)) return BadRequest("mishaet drugay zona");
                if (await _unitOfWork.AreaRepository.CheckPolygonIntersectionAsync(addAreaDTO,id.Value)) return BadRequest("you inside");
                var area = await _unitOfWork.AreaRepository.GetAreaAsync(id.Value);
                _unitOfWork.AreaRepository.Update(area);
                await _unitOfWork.Complete();
                return Ok( area);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.ToString());
            }

        }


        [HttpDelete("{id?}")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Invalid id");
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            var authorizedAccount = await _userManager.FindByEmailAsync(emailAuthorizedAccount);
            var area = await _unitOfWork.AreaRepository.GetAreaAsync(id.Value);
            if (null == area) return NotFound("Area not found");
            _unitOfWork.AreaRepository.DeleteArea(area);
            await _unitOfWork.Complete();
            return Ok();

        }
        [HttpGet("{id?}/analytics")]
        
        public async Task<ActionResult<AnalyticsResponse>> Analytics(long? id, [FromQuery] AnimalsAnalyticsParams searchParams)
        {
            try
            {
                var r = await _unitOfWork.AnimalsAnalyticsRepository.GetAnalyticsAsync(id.Value, searchParams.StartDateTime, searchParams.EndDateTime);
                return Ok(r);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.ToString());
            }

        }

       
    }
}