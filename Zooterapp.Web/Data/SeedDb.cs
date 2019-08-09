using System;
using System.Linq;
using System.Threading.Tasks;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPetTypesAsync();
            await CheckPetOwnersAsync();
            await CheckUsersAsync();
            await CheckPetsAsync();
            //await CheckAchievements();
        }

        private async Task CheckAchievements()
        {
            if (!_context.Achievements.Any())
            {
                _context.Achievements.Add(new Achievement { Name = "Obediencia" });
                _context.Achievements.Add(new Achievement { Name = "Sociabilidad" });
                _context.Achievements.Add(new Achievement { Name = "Tranquilidad" });
                _context.Achievements.Add(new Achievement { Name = "Didactica" });
                _context.Achievements.Add(new Achievement { Name = "Acompañamiento" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPetsAsync()
        {
            var owner = _context.PetOwners.FirstOrDefault();
            var type = _context.PetTypes.FirstOrDefault();

            if (!_context.Pets.Any())
            {
                AddPet(3, "Levi", true, owner, type);
                AddPet(4, "Yuki", true, owner, type);
                AddPet(2, "Firulais", true, owner, type);
                AddPet(5, "Kuro", true, owner, type);

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPetTypesAsync()
        {
            if (!_context.PetTypes.Any())
            {
                 _context.PetTypes.Add(new PetType { Name = "Canino" });
                 _context.PetTypes.Add(new PetType { Name = "Felino" });
                 _context.PetTypes.Add(new PetType { Name = "Equino" });
                 _context.PetTypes.Add(new PetType { Name = "Ave" });
                 _context.PetTypes.Add(new PetType { Name = "Bovino" });
                 _context.PetTypes.Add(new PetType { Name = "Porcino" });
                 _context.PetTypes.Add(new PetType { Name = "Ovino" });
                 _context.PetTypes.Add(new PetType { Name = "Caprino" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPetOwnersAsync()
        {
            if (!_context.PetOwners.Any())
            {
                AddPetOwner("8989898", "Pedro", "Perez", "234 3232", "310 322 3221", "Calle Luna Calle Sol");
                AddPetOwner("7655544", "Jose", "Cardona", "343 3226", "300 322 3221", "Calle 77 #22 21");
                AddPetOwner("6565555", "Maria", "López", "450 4332", "350 322 3221", "Carrera 56 #22 21");

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckUsersAsync()
        {
            if (!_context.Users.Any())
            {
                AddUser("876543", "Ramon", "Gamboa", "234 3232", "310 322 3221", "Calle Luna Calle Sol");
                AddUser("654565", "Julian", "Martinez", "343 3226", "300 322 3221", "Calle 77 #22 21");
                AddUser("214231", "Carmenza", "Ruiz", "450 4332", "350 322 3221", "Carrera 56 #22 21");

                await _context.SaveChangesAsync();
            }
        }



        // MOCKUP METHODS

        private void AddPetOwner(
            string document,
            string name,
            string lastName,
            string phone,
            string cellPhone,
            string address)
        {
            _context.PetOwners.Add(new PetOwner
            {
                Document = document,
                Name = name,
                LastName = lastName,
                Phone = phone,
                CellPhone = cellPhone,
                Address = address
            });
        }

        private void AddUser(
            string document,
            string name,
            string lastName,
            string phone,
            string cellPhone,
            string address)
        {
            _context.Users.Add(new User
            {
                Document = document,
                Name = name,
                LastName = lastName,
                Phone = phone,
                CellPhone = cellPhone,
                Address = address
            });
        }

        private void AddPet(
            int age,
            string name,
            bool isAvailable,
            PetOwner owner,
            PetType type)
        {
            _context.Pets.Add(new Pet
            {
                Age = age,
                Name = name,
                IsAvailable = isAvailable,
                Owner = owner,
                Type = type
            });
        }
    }
}
