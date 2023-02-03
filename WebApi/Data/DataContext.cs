

using WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    public class DataContext : DbContext
    {
       

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
                Database.EnsureCreated();
        }  
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Animal> Animals => Set<Animal>();
        public DbSet<AnimalType> AnimalTypes => Set<AnimalType>();
        public DbSet<LocationPoint> LocationPoints => Set<LocationPoint>();
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations => Set<AnimalVisitedLocation>();
        
    }
}
