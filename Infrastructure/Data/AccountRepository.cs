//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Domain;
//using Domain.DTOs;
//using Domain.Entities;
//using Domain.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Data
//{
//    public class AccountRepository : IAccountRepository
//    {
//        private readonly DataContext _context;
//        private readonly IMapper _mapper;
//        public AccountRepository(DataContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }
//        public async Task<Account> GetAccountAsync(int id)
//        {
//            return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
//        }

//        public async Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password)
//        {
//            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
//        }
//        public void UpdateAccount(Account account)
//        {

//            _context.Entry(account).State = EntityState.Modified;
//        }
//        public void DeleteAccount(Account account)
//        {
//            _context.Accounts.Remove(account);
//        }

//        public async Task<IEnumerable<AccountDTO>> GetAccountsWitsParamsAsync(AccountParams accountParams)
//        {
//            var query = _context.Accounts.AsQueryable();
//            if (accountParams.FirstName != null) query = query.Where(a => a.FirstName.Trim().ToLower().Contains(accountParams.FirstName.Trim().ToLower()));
//            if (accountParams.LastName != null) query = query.Where(a => a.LastName.Trim().ToLower().Contains(accountParams.LastName.Trim().ToLower()));
//            if (accountParams.Email != null) query = query.Where(a => a.Email.Contains(accountParams.Email));
//            query = query.OrderBy(a => a.Id);
//            return await query.ProjectTo<AccountDTO>(_mapper.ConfigurationProvider).AsNoTracking().Skip(accountParams.From).Take(accountParams.Size).ToListAsync();
//        }

//        public void AddAccount(Account account) => _context.Accounts.Add(account);

//        public async Task<bool> EmailIsFree(string email)
//        {
//            return !await _context.Accounts.AnyAsync(a => a.Email == email);
//        }
//        public async Task<bool> AnimalsExistAsync(long id)
//        {
//            return await _context.Animals.Include(p => p.Chipper).AnyAsync(p => p.Chipper.Id == id);
//        }

//    }
//}
