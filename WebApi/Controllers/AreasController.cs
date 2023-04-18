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
using Domain.Enums;
using WebApi.Hellpers.Filter;

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
        [HttpPost]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN))]
        public async Task<ActionResult<Area>> Add([FromBody] AreaDTO addAreaDTO)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!addAreaDTO.Polygon.IsSimple) return BadRequest("non-convex polygon");
            if (await _unitOfWork.AreaRepository.DoesIntersectWithExistingAreas(addAreaDTO)) return BadRequest("another zone interferes");
            if (await _unitOfWork.AreaRepository.CheckPolygonIntersectionAsync(addAreaDTO)) return BadRequest("inside another zone");
            var area = _mapper.Map<Area>(addAreaDTO);
            _unitOfWork.AreaRepository.AddArea(area);
            await _unitOfWork.Complete();
            return Created("url", area);
        }

        [HttpPut("{id?}")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN))]
        public async Task<ActionResult<ReturnAnimalDTO>> Update(int? id, [FromBody] AreaDTO addAreaDTO)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!CheckingForConvexPolygon.IsConvexPolygon(addAreaDTO)) return BadRequest("non-convex polygon");
            if (await _unitOfWork.AreaRepository.DoesIntersectWithExistingAreas(addAreaDTO, id.Value)) return BadRequest("another zone interferes");
            if (await _unitOfWork.AreaRepository.CheckPolygonIntersectionAsync(addAreaDTO, id.Value)) return BadRequest("inside another zone");
            var area = await _unitOfWork.AreaRepository.GetAreaAsync(id.Value);
            _unitOfWork.AreaRepository.Update(area);
            await _unitOfWork.Complete();
            return Ok(area);
        }


        [HttpDelete("{id?}")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN))]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Invalid id");
            var area = await _unitOfWork.AreaRepository.GetAreaAsync(id.Value);
            if (null == area) return NotFound("Area not found");
            _unitOfWork.AreaRepository.DeleteArea(area);
            await _unitOfWork.Complete();
            return Ok();

        }
        [HttpGet("{id?}/analytics")]

        public async Task<ActionResult<AnalyticsResponse>> Analytics(long? id, [FromQuery] AnimalsAnalyticsParams searchParams)
        {
            var res = await _unitOfWork.AnimalsAnalyticsRepository.GetAnalyticsAsync(id.Value, searchParams.StartDateTime, searchParams.EndDateTime);
            return Ok(res);
        }


    }
}