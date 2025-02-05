﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Commitment> Commitments { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<PetAchievement> PetAchievements { get; set; }

        public DbSet<PetImage> PetImages { get; set; }

        public DbSet<PetOwner> PetOwners { get; set; }

        public DbSet<PetRate> PetRates { get; set; }

        public DbSet<PetType> PetTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Manager> Managers{ get; set; }

    }
}
