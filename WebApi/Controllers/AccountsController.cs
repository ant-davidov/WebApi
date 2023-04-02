using AutoMapper;
using Domain;
using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Hellpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Hellpers;
using WebApi.Hellpers.Filter;

namespace WebApi.Controllers
{
   
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<Account> _userManager;
        private readonly IMapper _mapper;
        public AccountsController(UserManager<Account> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/registration")]
        public async Task<ActionResult<AccountDTO>> Registration([FromBody] RegistrationDTO registrationAccount)
        {
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if(emailAuthorizedAccount != null) return Forbid();
            registrationAccount.Email = _userManager.NormalizeEmail(registrationAccount.Email);
            if (await _userManager.FindByEmailAsync(registrationAccount.Email) != null) return Conflict("Email is not free"); 
            var account =  _mapper.Map<Account>(registrationAccount);
            var res = await _userManager.CreateAsync(account,registrationAccount.Password);
            if (res.Succeeded)
            {
                //await _userManager.AddToRoleAsync(account, RoleEnum.CHIPPER);
                return Created("./registration", _mapper.Map<AccountDTO>(account));
            }
            else
                return BadRequest(res.Errors.First());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> Search([FromQuery] AccountParams accountParams)
        {
            return Ok(await _userManager.GetAccountsWitsParamsAsync(accountParams));
        }
       
        [HttpGet("{id?}")]
        public async Task<ActionResult<AccountDTO>> GetById(int? id)
        {
            var a = _userManager.Users.ToList();
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var account = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (account == null) return NotFound();
            return _mapper.Map<AccountDTO>(account);
        }
        [HttpPut("{id?}")]
        public async Task<ActionResult<AccountDTO>> Update(int? id, [FromBody] RegistrationDTO accountUpdate)
        {
            try
            {

           
            #region Validation
            if (id == null || id <= 0) return BadRequest("Incorrect id");
            var account = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (account == null) return Forbid();
            if (accountUpdate.Email != account.Email)
                if (accountUpdate.Email != account.Email && await _userManager.FindByEmailAsync(accountUpdate.Email) != null)
                    return Conflict();
            var emailAuthorizedAccount = HttpContext.User.Identity.Name;
            if (emailAuthorizedAccount != account.Email) return Forbid(); 
            #endregion
            account = _mapper.Map(accountUpdate, account);

          
            if ((await _userManager.UpdateAsync(account)).Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(accountUpdate.Password) && !await _userManager.CheckPasswordAsync(account,accountUpdate.Password))
                    {
                        await _userManager.RemovePasswordAsync(account);
                        await _userManager.AddPasswordAsync(account, accountUpdate.Password);
                    }  
                    return _mapper.Map<AccountDTO>(account);
                }
               
            return BadRequest("Update Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + " " + ex.Message);
                return BadRequest("Update Error error");
            }
        }
        [HttpDelete("{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            #region Validation
            try
            {
                if (id == null || id <= 0) return BadRequest("Incorrect id");
                var account = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (account == null) return Forbid();
                var emailAuthorizedAccount = HttpContext.User.Identity.Name;
                if (emailAuthorizedAccount != account.Email) return Forbid();
                if (await _userManager.AnimalsExistAsync(account.Id)) return BadRequest("There are animals");
                #endregion
                await _userManager.DeleteAsync(account);
                return Ok();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.ToString() + " " + ex.Message);

            }
        }
    }
}
