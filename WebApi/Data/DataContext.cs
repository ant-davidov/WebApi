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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder.Entity<Animal>()
                .HasOne(p=> p.Chipper)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Animal>()
                .HasOne(p=> p.ChippingLocation)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);       
        modelBuilder.Entity<AnimalVisitedLocation>()
                .HasOne(p=> p.LocationPoint)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);      
        }
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Animal> Animals => Set<Animal>();
        public DbSet<AnimalType> AnimalTypes => Set<AnimalType>();
        public DbSet<LocationPoint> LocationPoints => Set<LocationPoint>();
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations => Set<AnimalVisitedLocation>();
        
    }
}
