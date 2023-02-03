﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Data;

#nullable disable

namespace WebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230202134840_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("AnimalAnimalType", b =>
                {
                    b.Property<int>("AnimalId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AnimalTypesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AnimalId", "AnimalTypesId");

                    b.HasIndex("AnimalTypesId");

                    b.ToTable("AnimalAnimalType");
                });

            modelBuilder.Entity("WebApi.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("WebApi.Entities.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChipperId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ChippingDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("ChippingLocationId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DeathDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Height")
                        .HasColumnType("REAL");

                    b.Property<float>("Lenght")
                        .HasColumnType("REAL");

                    b.Property<int>("LifeStatus")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ChippingLocationId");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("WebApi.Entities.AnimalType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AnimalTypes");
                });

            modelBuilder.Entity("WebApi.Entities.AnimalVisitedLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AnimalId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTimeOfVisitLocationPoint")
                        .HasColumnType("TEXT");

                    b.Property<int>("LocationPointId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("LocationPointId");

                    b.ToTable("AnimalVisitedLocations");
                });

            modelBuilder.Entity("WebApi.Entities.LocationPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("LocationPoints");
                });

            modelBuilder.Entity("AnimalAnimalType", b =>
                {
                    b.HasOne("WebApi.Entities.Animal", null)
                        .WithMany()
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.AnimalType", null)
                        .WithMany()
                        .HasForeignKey("AnimalTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Entities.Animal", b =>
                {
                    b.HasOne("WebApi.Entities.AnimalVisitedLocation", "ChippingLocation")
                        .WithMany()
                        .HasForeignKey("ChippingLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChippingLocation");
                });

            modelBuilder.Entity("WebApi.Entities.AnimalVisitedLocation", b =>
                {
                    b.HasOne("WebApi.Entities.Animal", null)
                        .WithMany("VisitedLocations")
                        .HasForeignKey("AnimalId");

                    b.HasOne("WebApi.Entities.LocationPoint", "LocationPoint")
                        .WithMany()
                        .HasForeignKey("LocationPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LocationPoint");
                });

            modelBuilder.Entity("WebApi.Entities.Animal", b =>
                {
                    b.Navigation("VisitedLocations");
                });
#pragma warning restore 612, 618
        }
    }
}
