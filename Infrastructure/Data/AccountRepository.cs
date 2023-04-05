using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> _userManager;
        private readonly IMapper _mapper;
        public AccountRepository(UserManager<Account> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Account> GetAccountAsync(int id)
        {
            return await _userManager.Users.Include(u => u.UserRoles).ThenInclude(u => u.Role).Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Account> GetAccountByEmailAndPasswordAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<bool> UpdateAccount(Account account, string newPassword)
        {

            if ((await _userManager.UpdateAsync(account)).Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(newPassword) && !await _userManager.CheckPasswordAsync(account, newPassword))
                {
                    await _userManager.RemovePasswordAsync(account);
                    await _userManager.AddPasswordAsync(account, newPassword);
                  
                }
                return true;
            }
            return false;
        }
        public async Task DeleteAccount(Account account)
        {
            await _userManager.DeleteAsync(account);
        }

        public async Task<IEnumerable<AccountDTO>> GetAccountsWitsParamsAsync(AccountParams accountParams)
        {
            var query = _userManager.Users.AsQueryable();
            if (accountParams.FirstName != null) query = query.Where(a => a.FirstName.Trim().ToLower().Contains(accountParams.FirstName.Trim().ToLower()));
            if (accountParams.LastName != null) query = query.Where(a => a.LastName.Trim().ToLower().Contains(accountParams.LastName.Trim().ToLower()));
            if (accountParams.Email != null) query = query.Where(a => a.Email.Contains(accountParams.Email));
            query = query.OrderBy(a => a.Id);
            return await query.ProjectTo<AccountDTO>(_mapper.ConfigurationProvider).AsNoTracking().Skip(accountParams.From).Take(accountParams.Size).ToListAsync();
        }

        public async Task<IdentityResult> AddAccount(Account account, string password)
        {
            return await _userManager.CreateAsync(account, password);
        }

        public async Task<bool> EmailIsFree(string email)
        {
            return (await _userManager.FindByEmailAsync(email)) == null;
        }
        public async Task<bool> AnimalsExistAsync(long id)
        {
            var a = _userManager.Users.Include(a => a.Animals).ToList();
            return ((await _userManager.Users.Include(u => u.Animals).Where(u => u.Id == id).FirstOrDefaultAsync()).Animals.Any());
        }

        public async Task AddRoleAsync(Account account, string role)
        {
            await _userManager.AddToRoleAsync(account, role);
        }
        public async Task DeleteRoleAsync(Account account, string role)
        {
            await _userManager.RemoveFromRoleAsync(account, role);
        }

        public Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            throw new NotImplementedException();
        }




    }
}
