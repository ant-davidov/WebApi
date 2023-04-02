using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext : IdentityDbContext<Account, AppRole, int>
    {    
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
                Database.EnsureCreated();
        } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Account>()
            //     .HasMany(p => p.Animals)
            //     .WithOne()
            //     .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Animal>()
                .HasOne(p=> p.Chipper)
                .WithMany(t=> t.Animals)
                .HasForeignKey(p=> p.ChipperId)
                .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Animal>()
                .HasOne(p=> p.ChippingLocation)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
                 
        modelBuilder.Entity<AnimalVisitedLocation>()
                .HasOne(p=> p.LocationPoint)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);     
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Animal> Animals => Set<Animal>();
        public DbSet<AnimalType> AnimalTypes => Set<AnimalType>();
        public DbSet<LocationPoint> LocationPoints => Set<LocationPoint>();
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations => Set<AnimalVisitedLocation>();
        
    }
}
