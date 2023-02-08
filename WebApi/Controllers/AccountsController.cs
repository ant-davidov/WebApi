using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<AccountDTO>> Registration([FromBody] Account account)
        {   
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if(emailAuthorizedAccount != null) return Forbid();
            if (!await _unitOfWork.AccountRepository.EmailIsFree(account.Email)) return Conflict("Email is not free");
            account.Id = 0;
            _unitOfWork.AccountRepository.AddAccount(account);
            await _unitOfWork.Complete();
            var newAccount = await _unitOfWork.AccountRepository.GetAccountByEmailAndPasswordAsync(account.Email, account.Password);
            return Created("./registration", _mapper.Map<AccountDTO>(newAccount));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> Search([FromQuery] AccountParams accountParams)
        {
             return await _unitOfWork.AccountRepository.GetAccountsWitsParamsAsync(accountParams);
        }
       
        [HttpGet("{id?}")]
        public async Task<ActionResult<AccountDTO>> GetById(int? id)
        {      
            if (id == null || id <= 0) return BadRequest();
            var account =  await _unitOfWork.AccountRepository.GetAccountAsync(id.Value);
            if (account == null) return NotFound();
            return _mapper.Map<AccountDTO>(account);
        }
        [HttpPut("{id?}")]
        public async Task<ActionResult<AccountDTO>> Update(int? id, [FromBody] Account accountUpdate)
        {
            #region Validation
            if (id == null || id <= 0) return BadRequest();
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
            if (id == null || id <= 0) return BadRequest();
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
