using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
   // [Authorize]
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
        public async Task<ActionResult<AccountDTO>> Registration([FromBody] Account account)
        {
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if(emailAuthorizedAccount != null) return Forbid();
            if (!ModelState.IsValid) return BadRequest();
            if (!await _unitOfWork.AccountRepository.EmailIsFree(account.Email)) return Conflict();
            account.Id = 0;
            _unitOfWork.AccountRepository.AddAccount(account);
            await _unitOfWork.Complete();
            var newAccount = await _unitOfWork.AccountRepository.GetAccountByEmailAndPasswordAsync(account.Email, account.Password);
            var url = Url.Action("Post", "AccountsController", new { id = newAccount.Id }, Request.Scheme);
            return Created("./registration", _mapper.Map<AccountDTO>(newAccount));
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> Search([FromQuery] AccountParams accountParams)
        {
            return await _unitOfWork.AccountRepository.GetAccountsWitsParamsAsync(accountParams);
        }

        [AllowAnonymous]
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
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            if (!await _unitOfWork.AccountRepository.EmailIsFree(accountUpdate.Email)) return Conflict();
            var account = await _unitOfWork.AccountRepository.GetAccountAsync(id.Value);
            if (account == null) return Forbid();
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if (emailAuthorizedAccount != account.Email) return Forbid();
            //найти способ с automapper
            account.Email = accountUpdate.Email;
            account.FirstName = accountUpdate.FirstName;
            account.LastName = accountUpdate.LastName;
            account.Password = accountUpdate.Password;
            _unitOfWork.AccountRepository.UpdateAccount(account);
            if (await _unitOfWork.Complete())
                return _mapper.Map<AccountDTO>(account);
            else
                return BadRequest("Update Error");
        }
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var account = await _unitOfWork.AccountRepository.GetAccountAsync(id.Value);
            if (account == null) return Forbid();
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if (emailAuthorizedAccount != account.Email) return Forbid();
            _unitOfWork.AccountRepository.DeleteAccount(account);
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
