using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
   
    public class AccountsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
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
            if(emailAuthorizedAccount != null) return Forbid();
            if (!await _unitOfWork.AccountRepository.EmailIsFree(registrationAccount.Email)) return Conflict("Email is not free");
            var account =  _mapper.Map<Account>(registrationAccount);
            _unitOfWork.AccountRepository.AddAccount(account);
            await _unitOfWork.Complete();
            return Created("./registration", _mapper.Map<AccountDTO>(account));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> Search([FromQuery] AccountParams accountParams)
        {
             return await _unitOfWork.AccountRepository.GetAccountsWitsParamsAsync(accountParams);
        }
       
        [HttpGet("{id?}")]
        public async Task<ActionResult<AccountDTO>> GetById(int? id)
        {      
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var account =  await _unitOfWork.AccountRepository.GetAccountAsync(id.Value);
            if (account == null) return NotFound();
            return _mapper.Map<AccountDTO>(account);
        }
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
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if (emailAuthorizedAccount != account.Email) return Forbid(); 
            #endregion
            account = _mapper.Map(accountUpdate, account);
            _unitOfWork.AccountRepository.UpdateAccount(account);
            if (await _unitOfWork.Complete())
                return _mapper.Map<AccountDTO>(account);

            return BadRequest("Update Error");
        }
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            #region Validation
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var account = await _unitOfWork.AccountRepository.GetAccountAsync(id.Value);
            if (account == null) return Forbid();
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if (emailAuthorizedAccount != account.Email) return Forbid();
            if (await _unitOfWork.AccountRepository.AnimalsExistAsync(account.Id)) return BadRequest("There are animals"); 
            #endregion
            _unitOfWork.AccountRepository.DeleteAccount(account);
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
