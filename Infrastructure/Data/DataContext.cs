using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Seed;
using Domain.Entities.Secondary;
using System.Xml;

namespace Infrastructure.Data
{
    public class DataContext : IdentityDbContext<
        Account, ApplicationRole, int,
        IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Animal>()
                    .HasOne(p => p.Chipper)
                    .WithMany(t => t.Animals)
                    .HasForeignKey(p => p.ChipperId)
                    .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Animal>()
                    .HasOne(p => p.ChippingLocation)
                    .WithMany()
                    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AnimalVisitedLocation>()
                    .HasOne(p => p.LocationPoint)
                    .WithMany()
                    .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Coordinates>()
                    .HasOne(c => c.Area)
                    .WithMany(a => a.AreaPoints)
                    .HasForeignKey(c => c.AreaId);

            modelBuilder.Entity<Coordinates>()
             .HasIndex(c => c.AreaId)
              .HasDatabaseName("IX_Coordinates_AreaId");


            modelBuilder.Entity<Account>(b =>
            {

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            

        }


        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Animal> Animals => Set<Animal>();
        public DbSet<AnimalType> AnimalTypes => Set<AnimalType>();
        public DbSet<LocationPoint> LocationPoints => Set<LocationPoint>();
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations => Set<AnimalVisitedLocation>();
        public DbSet<Area> Areas => Set<Area>();

    }
}
