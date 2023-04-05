
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly UserManager<Account> _userManager;
        public UnitOfWork(DataContext context, IMapper mapper, UserManager<Account> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager; 
        }
        public IAnimalRepository AnimalRepository => new AnimalRepository(_context, _mapper);
        public IAnimalTypeRepository AnimalTypeRepository => new AnimalTypeRepository(_context);
        public IAnimalVisitedLocationRepository AnimalVisitedLocationRepository => new AnimalVisitedLocationRepository(_context,_mapper);
        public ILocationPointRepository LocationPointRepository => new LocationPointRepository(_context);
        public IAreaRepository AreaRepository => new AreaRepository(_context);
        public IAnimalsAnalyticsRepository AnimalsAnalyticsRepository => new AnimalsAnalyticsRepository(_context);
        public IAccountRepository AccountRepository => new AccountRepository(_userManager,_mapper);
        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
