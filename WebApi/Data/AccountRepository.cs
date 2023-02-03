﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Hellpers;
using WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AccountRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Account> GetAccountAsync(int id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a=> a.Id == id);
        }

        public async Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }
        public void UpdateAccount(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
        }
        public void DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);
        }

        public async Task<PageList<AccountDTO>> GetAccountsWitsParamsAsync(AccountParams accountParams)
        {
            var query = _context.Accounts.AsQueryable();
            query = query.Where(a => a.FirstName.IndexOf(accountParams.FirstName, StringComparison.OrdinalIgnoreCase) > 0);
            query = query.Where(a => a.LastName.IndexOf(accountParams.LastName, StringComparison.OrdinalIgnoreCase) > 0);
            query = query.Where(a => a.Email.IndexOf(accountParams.Email, StringComparison.OrdinalIgnoreCase) > 0);
            query = query.OrderBy(a => a.Id);

            return await  PageList<AccountDTO>.CreateAsync(query.ProjectTo<AccountDTO>(_mapper.ConfigurationProvider).AsNoTracking(), accountParams.From, accountParams.Size);
           
        }

        public void AddAccount(Account account)
        {
           _context.Accounts.Add(account) ;
        }

        public async Task<bool> EmailIsFree(string email)
        {
            if (await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email) == null)
                return true;
            else
                return false;
                    
        }
    }
}
