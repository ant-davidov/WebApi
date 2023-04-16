using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<IdentityResult> AddAccount(Account account, string password, RoleEnum role = RoleEnum.USER);
        Task<Account> GetAccountAsync(int id);
        Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password);
        Task<bool> UpdateAccount(Account account, string newPassword);
        Task DeleteAccount(Account account);
        Task<bool> AnimalsExistAsync(long id);
        Task<bool> EmailIsFree(string email);
        Task AddRoleAsync(Account account,string role);
        Task DeleteRoleAsync(Account account, string role);
        Task<IEnumerable<AccountDTO>> GetAccountsWitsParamsAsync(AccountParams accountParams);

       
    }
}
