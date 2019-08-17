using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zooterapp.Web.Data.Entities;
using Zooterapp.Web.Helpers;

namespace Zooterapp.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;

        public SeedDb(DataContext context, IUserHelper userHelper, IConfiguration configuration)
        {
            _context = context;
            _userHelper = userHelper;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRoles();
            var customer = await CheckUsersAsync("1020", "Andres", "Cahona", "acahona@correo.com", "300 783 9434", "Calle Luna Calle Sol", _configuration["Roles:Customer"]);
            var petOwner = await CheckUsersAsync("2020", "Andrea", "Cahona", "acahona2@correo.com", "300 783 9435", "Calle Luna Calle Sol", _configuration["Roles:PetOwner"]);
            var manager = await CheckUsersAsync("3030", "Yeison", "Calle", "yeison.calle@hotmail.com", "310 509 7121", "Carrera 110 # 36 - 60", _configuration["Roles:Manager"]);
            await CheckPetTypesAsync();
            await CheckManagerAsync(manager);
            await CheckPetOwnersAsync(petOwner);
            await CheckCustomersAsync(customer);
            await CheckPetsAsync();
            await CheckCommitmentAsync();
            await CheckAchievements();
        }

        private async Task CheckManagerAsync(User user)
        {
            if (!_context.Managers.Any())
            {
                _context.Managers.Add(new Manager
                {
                    User = user,
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCommitmentAsync()
        {
            var petOwner = _context.PetOwners.FirstOrDefault();
            var customer = _context.Customers.FirstOrDefault();
            var pet = _context.Pets.FirstOrDefault();

            if (!_context.Commitments.Any())
            {
                _context.Commitments.Add(new Commitment
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddMonths(6),
                    IsActive = true,
                    Customer = customer,
                    PetOwner = petOwner,
                    Pet = pet,
                    Price = 1000000M,
                    Remarks = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris nec iaculis ex. Nullam gravida nunc eleifend",
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUsersAsync(string document, string firstName, string lastName, string email, string cellphone, string address, string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Name = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = cellphone,
                    Address = address,
                    Document = document
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync(_configuration["Roles:Manager"]);
            await _userHelper.CheckRoleAsync(_configuration["Roles:PetOwner"]);
            await _userHelper.CheckRoleAsync(_configuration["Roles:Customer"]);
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
                AddPet(3, "Levi", true, owner, type, "Chancol");
                AddPet(4, "Yuki", true, owner, type, "Angora");
                AddPet(2, "Firulais", true, owner, type, "Doberman");
                AddPet(5, "Kuro", true, owner, type, "Bombay");

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

        private async Task CheckPetOwnersAsync(User user)
        {
            if (!_context.PetOwners.Any())
            {
                _context.PetOwners.Add(new PetOwner
                {
                    User = user,
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCustomersAsync(User user)
        {
            if (!_context.Customers.Any())
            {
                _context.Customers.Add(new Customer
                {
                    User = user,
                });

                await _context.SaveChangesAsync();
            }
        }



        // MOCKUP METHODS

        private void AddPet(
            int age,
            string name,
            bool isAvailable,
            PetOwner owner,
            PetType type,
            string race)
        {
            _context.Pets.Add(new Pet
            {
                Age = age,
                Name = name,
                IsAvailable = isAvailable,
                Owner = owner,
                PetType = type,
                Race = race,
            });
        }
    }
}
