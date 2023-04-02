using Domain.DTOs;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.Hellpers;
using AutoMapper.QueryableExtensions;

namespace Infrastructure.Data.Hellpers.Extensions
{
    public static class CustomUserManagerExtensions
    {
        
        public static async Task<IEnumerable<AccountDTO>> GetAccountsWitsParamsAsync(this UserManager<Account> userManager, AccountParams accountParams)
        {
            IMapper _mapper = Configure();
            var query = userManager.Users.AsQueryable();
            if (accountParams.FirstName != null) query = query.Where(a => a.FirstName.Trim().ToLower().Contains(accountParams.FirstName.Trim().ToLower()));
            if (accountParams.LastName != null) query = query.Where(a => a.LastName.Trim().ToLower().Contains(accountParams.LastName.Trim().ToLower()));
            if (accountParams.Email != null) query = query.Where(a => a.Email.Contains(accountParams.Email));
            query = query.OrderBy(a => a.Id);
            return await query.ProjectTo<AccountDTO>(_mapper.ConfigurationProvider).AsNoTracking().Skip(accountParams.From).Take(accountParams.Size).ToListAsync();
        }
        public static async  Task<bool> AnimalsExistAsync(this UserManager<Account> userManager,long id)
        {
            var a = userManager.Users.Include(a => a.Animals).ToList();
            return ( (await userManager.Users.Include(u => u.Animals).Where(u => u.Id == id).FirstOrDefaultAsync()).Animals.Any());
        }
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfiles>();
            });
            return config.CreateMapper();
        }
    }
}
