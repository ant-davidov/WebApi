using AutoMapper;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public IAccountRepository AccountRepository  => new AccountRepository(_context,_mapper);
        public IAnimalRepository AnimalRepository => new AnimalRepository(_context);
        public IAnimalTypeRepository AnimalTypeRepository => new AnimalTypeRepository(_context);
        public IAnimalVisitedLocationRepository AnimalVisitedLocationRepository => new AnimalVisitedLocationRepository(_context);
        public ILocationPointRepository LocationPointRepository => new LocationPointRepository(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
