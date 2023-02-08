using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;

namespace WebApi.Interfaces
{
    public interface IAccountRepository
    {
        void AddAccount(Account account);
        Task<Account> GetAccountAsync(int id);
        Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password);
        Task<Account> GetAccountByEmailAsync(string email);
        void UpdateAccount(Account account);
        void DeleteAccount(Account account);
        Task<bool> AnimalsExistAsync(long id);
        Task<bool> EmailIsFree(string email);
        Task<PageList<AccountDTO>> GetAccountsWitsParamsAsync(AccountParams accountParams);
    }
}
