using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;
using WebApi.Hellpers.CreatePage;

namespace WebApi.Interfaces
{
    public interface IAccountRepository
    {
        void AddAccount(Account account);
        Task<Account> GetAccountAsync(int id);
        Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password);
        void UpdateAccount(Account account);
        void DeleteAccount(Account account);
        Task<bool> AnimalsExistAsync(long id);
        Task<bool> EmailIsFree(string email);
        Task<PageList<AccountDTO>> GetAccountsWitsParamsAsync(AccountParams accountParams);
    }
}
