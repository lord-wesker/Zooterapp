using Microsoft.EntityFrameworkCore;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<PetOwner> PetOwners { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Commitment> Commitments { get; set; }

        public DbSet<PetImage> PetImages { get; set; }

        public DbSet<PetType> PetTypes { get; set; }
    }
}
