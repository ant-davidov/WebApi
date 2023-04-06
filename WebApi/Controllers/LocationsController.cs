using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain.CreatePage;
using Geohash;

namespace WebApi.Controllers
{
    public class LocationsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        public LocationsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet("{id?}")]
        public async Task<ActionResult<LocationPoint>> GetById(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if (point == null) return NotFound();
            return point;
        }
        [HttpPost]
        public async Task<ActionResult<LocationPoint>> Add([FromBody] LocationPointDTO pointDTO)
        {
            var point = _mapper.Map<LocationPoint>(pointDTO);
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            var authorizedAccount = await _userManager.FindByEmailAsync(emailAuthorizedAccount);
            //if(authorizedAccount.Role == Domain.Enums.RoleEnum.USER) return Forbid();
            if (!await _unitOfWork.LocationPointRepository.CheckCordinatesAsync(point)) return Conflict("Already have");
            _unitOfWork.LocationPointRepository.AddLocationPoint(point);
            await _unitOfWork.Complete();
            return Created("./locations", point);
        }

        [HttpPut("{id?}")]
        public async Task<ActionResult<LocationPoint>> Update(int? id, [FromBody] LocationPointDTO updatePoint)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            var authorizedAccount = await _userManager.FindByEmailAsync(emailAuthorizedAccount);
            //if(authorizedAccount.Role == Domain.Enums.RoleEnum.USER) return Forbid();
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if (null == point) return NotFound();
            var newPoint = _mapper.Map<LocationPoint>(updatePoint);
            if (!await _unitOfWork.LocationPointRepository.CheckCordinatesAsync(newPoint)) return Conflict();
            point = _mapper.Map(newPoint, point);
            _unitOfWork.LocationPointRepository.UpdateLocationPoint(point);
            await _unitOfWork.Complete();
            return point;
        }
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            var authorizedAccount = await _userManager.FindByEmailAsync(emailAuthorizedAccount);
            //if(authorizedAccount.Role != Domain.Enums.RoleEnum.ADMIN) return Forbid();
            var point = await _unitOfWork.LocationPointRepository.GetLocationPointAsync(id.Value);
            if (null == point) return NotFound();
            if (await _unitOfWork.LocationPointRepository.VisitedLocationExistAsync(point.Id)) return BadRequest("Have visited point");
            _unitOfWork.LocationPointRepository.DeleteLocationPoint(point);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<long>> GetAreaByLocations([FromQuery] GetAreaByLocationsParams searchParams)
        {
            var point = await _unitOfWork.AreaRepository.GetAreaByLocations(searchParams);
            if (point == null) return NotFound();
            return point.Id;
        }
        [HttpGet("geohash")]
        public async Task<ActionResult<string>> GetAreaByLocationsgeohash([FromQuery] GetAreaByLocationsParams searchParams)
        {
            var hasher = new Geohasher();
            return hasher.Encode(searchParams.latitude, searchParams.longitude, 12);

        }




        [HttpGet("geohashv2")]
        public async Task<ActionResult<string>> GetAreaByLocationsgeohash2([FromQuery] GetAreaByLocationsParams searchParams)
        {
           
            var hasher = new Geohasher();
            var hash = hasher.Encode(searchParams.latitude, searchParams.longitude, 12);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(hash);
            return System.Convert.ToBase64String(bytes).ToString();
        }

        [HttpGet("geohashv3")]
        public async Task<ActionResult<string>> GetAreaByLocationsgeohash3([FromQuery] GetAreaByLocationsParams searchParams)
        {
            double latitude = searchParams.latitude;
            double longitude = searchParams.longitude;

            string location = $"latitude={latitude}&longitude={longitude}";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(location);
            return System.Convert.ToBase64String(bytes);
        }



    }
}
