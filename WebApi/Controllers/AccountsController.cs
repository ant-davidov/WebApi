using AutoMapper;
using Domain;
using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Hellpers.Filter;

namespace WebApi.Controllers
{

    public class AccountsController : BaseApiController
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public AccountsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/registration")]
        public async Task<ActionResult<AccountDTO>> Registration([FromBody] RegistrationDTO registrationAccount)
        {
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if (emailAuthorizedAccount != null) return Forbid();
            if (!await _unitOfWork.AccountRepository.EmailIsFree(registrationAccount.Email)) return Conflict("Email is not free");
            var account = _mapper.Map<Account>(registrationAccount);
            var res = await _unitOfWork.AccountRepository.AddAccount(account, registrationAccount.Password);
            if (res.Succeeded)
                return Created("./registration", _mapper.Map<AccountDTO>(account));
            else
                return BadRequest(res.Errors.First());
        }
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN))]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> Search([FromQuery] AccountParams accountParams)
        {
            return Ok(await _unitOfWork.AccountRepository.GetAccountsWitsParamsAsync(accountParams));
        }


        [CustomAuthorize(adminPrivileges: true)]
        [HttpGet("{id?}")]
        public async Task<ActionResult<AccountDTO>> GetById(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var searchAccount = await _unitOfWork.AccountRepository.GetAccountAsync(id.Value);
            if (searchAccount == null) return NotFound();
            return _mapper.Map<AccountDTO>(searchAccount);
        }

        [CustomAuthorize(true)]
        [HttpPut("{id?}")]

        public async Task<ActionResult<AccountDTO>> Update(int? id, [FromBody] RegistrationDTO accountUpdate)
        {
           
                #region Validation
                if (id == null || id <= 0) return BadRequest("Incorrect id");
                var account = await _unitOfWork.AccountRepository.GetAccountAsync(id.Value);
                if (account == null) return Forbid();
                if (accountUpdate.Email != account.Email)
                    if (!await _unitOfWork.AccountRepository.EmailIsFree(accountUpdate.Email))
                        return Conflict();
                #endregion
                account = _mapper.Map(accountUpdate, account);
                await _unitOfWork.AccountRepository.DeleteRoleAsync(account, account.UserRoles.FirstOrDefault().Role.Name);
                await _unitOfWork.AccountRepository.AddRoleAsync(account, accountUpdate.Role.ToString());
                if (await _unitOfWork.AccountRepository.UpdateAccount(account, accountUpdate.Password))
                    return Ok(_mapper.Map<AccountDTO>(account));

                return BadRequest("Update Error");
          
        }
        [CustomAuthorize(true)]
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            #region Validation
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var accountForDelete = await _unitOfWork.AccountRepository.GetAccountAsync(id.Value);
            if (accountForDelete == null) return NotFound();
            if (await _unitOfWork.AccountRepository.AnimalsExistAsync(accountForDelete.Id)) return BadRequest("There are animals");
            #endregion
            await _unitOfWork.AccountRepository.DeleteAccount(accountForDelete);
            return Ok();
        }

        [HttpPost]
        [Route("/accounts")]
        [CustomAuthorize(roles: nameof(RoleEnum.ADMIN))]
        public async Task<ActionResult<AccountDTO>> Accounts([FromBody] RegistrationDTO registrationAccount)
        {
            if (!await _unitOfWork.AccountRepository.EmailIsFree(registrationAccount.Email)) return Conflict("Email is not free");
            var newAccount = _mapper.Map<Account>(registrationAccount);
            var res = await _unitOfWork.AccountRepository.AddAccount(newAccount, registrationAccount.Password, registrationAccount.Role);
            if (res.Succeeded)       
                return Created("./registration", _mapper.Map<AccountDTO>(newAccount));          
            else
                return BadRequest(res.Errors.First());
        }




    }
}
