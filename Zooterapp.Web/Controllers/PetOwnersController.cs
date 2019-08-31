using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;
using Zooterapp.Web.Helpers;
using Zooterapp.Web.Models;

namespace Zooterapp.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class PetOwnersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConfiguration _configuration;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public PetOwnersController(DataContext context,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConfiguration configuration,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _configuration = configuration;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            return View(_context.PetOwners
                .Include(po => po.User)
                .Include(po => po.Pets)
                .Include(po => po.Commitments));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petOwner = await _context.PetOwners
                .Include(po => po.User)
                .Include(po => po.Pets)
                    .ThenInclude(p => p.PetType)
                .Include(po => po.Pets)
                    .ThenInclude(p => p.PetImages)
                .Include(po => po.Commitments)
                .Include(po => po.Pets)
                    .ThenInclude(p => p.PetAchievements)
                .FirstOrDefaultAsync(po => po.Id == id);

            if (petOwner == null)
            {
                return NotFound();
            }

            return View(petOwner);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await AddUserAsync(model);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(model);
                }

                var petOwner = new PetOwner
                {
                    Pets = new List<Pet>(),
                    Commitments = new List<Commitment>(),
                    User = user,
                };

                _context.PetOwners.Add(petOwner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petOwner = await _context.PetOwners
                .Include(po => po.User)
                .FirstOrDefaultAsync(po => po.Id == id);
            if (petOwner == null)
            {
                return NotFound();
            }

            _context.PetOwners.Remove(petOwner);
            await _context.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(petOwner.User.Email);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return NotFound();

            var petImage = await _context.PetImages
                .Include(pi => pi.Pet)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);

            if (petImage == null) return NotFound();

            _context.PetImages.Remove(petImage);
            await _context.SaveChangesAsync();

            return RedirectToAction($"{nameof(DetailsPet)}/{petImage.Pet.Id}");
        }

        public async Task<IActionResult> DeleteCommitment(int? id)
        {
            if (id == null) return NotFound();

            var commitment = await _context.Commitments
                .Include(c => c.Pet)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (commitment == null) return NotFound();

            _context.Commitments.Remove(commitment);
            await _context.SaveChangesAsync();

            return RedirectToAction($"{nameof(DetailsPet)}/{commitment.Pet.Id}");
        }

        public async Task<IActionResult> DeleteAchievement(int? id)
        {
            if (id == null) return NotFound();

            var achievement = await _context.PetAchievements
                .Include(c => c.Pet)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (achievement == null) return NotFound();

            _context.PetAchievements.Remove(achievement);
            await _context.SaveChangesAsync();

            return RedirectToAction($"{nameof(DetailsPet)}/{achievement.Pet.Id}");
        }

        public async Task<IActionResult> DeletePet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{pet.Owner.Id}");
        }

        public async Task<IActionResult> DetailsCommitment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commitment = await _context.Commitments
                .Include(c => c.PetOwner)
                    .ThenInclude(o => o.User)
                .Include(c => c.Customer)
                    .ThenInclude(o => o.User)
                .Include(c => c.Pet)
                    .ThenInclude(p => p.PetType)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (commitment == null)
            {
                return NotFound();
            }

            return View(commitment);
        }


        private async Task<User> AddUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                Name = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username,
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);

            if (result != IdentityResult.Success) return null;

            var newUser = await _userHelper.GetUserByEmailAsync(model.Username);
            await _userHelper.AddUserToRoleAsync(newUser, _configuration["Roles:PetOwner"]);

            return newUser;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petOwner = await _context.PetOwners
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (petOwner == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = petOwner.User.Address,
                Document = petOwner.User.Document,
                FirstName = petOwner.User.Name,
                Id = petOwner.Id,
                LastName = petOwner.User.LastName,
                PhoneNumber = petOwner.User.PhoneNumber
            };

            return View(view);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var petOwner = await _context.PetOwners
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == view.Id);

                petOwner.User.Document = view.Document;
                petOwner.User.Name = view.FirstName;
                petOwner.User.LastName = view.LastName;
                petOwner.User.Address = view.Address;
                petOwner.User.PhoneNumber = view.PhoneNumber;

                await _userHelper.UpdateUserAsync(petOwner.User);
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }


        public async Task<IActionResult> EditCommitment(int? id)
        {
            if (id == null) return NotFound();

            var commitment = await _context.Commitments
                .Include(c => c.PetOwner)
                .Include(c => c.Customer)
                .Include(c => c.Pet)
                .FirstOrDefaultAsync(c => c.Id == id.Value);

            if (commitment == null) return NotFound();

            return View(_converterHelper.ToCommitmentViewModel(commitment));
        }

        [HttpPost]
        public async Task<IActionResult> EditCommitment(CommitmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var commitment = await _converterHelper.ToCommitmentAsync(model, false);
                _context.Commitments.Update(commitment);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsPet)}/{model.PetOwnerId}");
            }

            return View(model);
        }

        //public async Task<IActionResult> EditAchievement(int? id)
        //{
        //    if (id == null) return NotFound();

        //    var Achievement = await _context.PetAchievements
        //        .Include(c => c.Pet)
        //        .FirstOrDefaultAsync(c => c.Id == id.Value);

        //    if (Achievement == null) return NotFound();

        //    return View(_converterHelper.ToPetAchievementViewModel(Achievement));
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditAchievement(PetAchievementViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var achievement = await _converterHelper.ToPetAchievementAsync(model, false);
        //        _context.PetAchievements.Update(achievement);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction($"{nameof(DetailsPet)}/{model.PetId}");
        //    }

        //    return View(model);
        //}
        ///

        public async Task<IActionResult> AddPet(int? id)
        {
            if (id == null) return NotFound();

            var petOwner = await _context.PetOwners.FindAsync(id.Value);

            if (petOwner == null) return NotFound();

            var view = new PetViewModel
            {
                PetOwnerId = petOwner.Id,
                PetTypes = _combosHelper.GetComboPetType(),
            };

            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> AddPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pet = await _converterHelper.ToPetAsync(model, true);
                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            model.PetTypes = _combosHelper.GetComboPetType();
            return View(model);
        }



        public async Task<IActionResult> EditPet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var pet = await _context.Pets
                .Include(p => p.Owner)
                .Include(p => p.PetType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
            {
                return NotFound();
            }

            var view = _converterHelper.ToPetViewModel(pet);
            return View(view);

        }

        [HttpPost]
        public async Task<IActionResult> EditPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pet = await _converterHelper.ToPetAsync(model, false);
                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.PetOwnerId}");
            }

            return View(model);
        }


        public async Task<IActionResult> DetailsPet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(o => o.Owner)
                    .ThenInclude(o => o.User)
                .Include(p => p.Commitments)
                    .ThenInclude(c => c.Customer)
                    .ThenInclude(c => c.User)
                .Include(o => o.PetType)
                .Include(p => p.PetImages)
                .Include(p => p.PetAchievements)
                    .ThenInclude(pa => pa.Achievement)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        //public async Task<IActionResult> AddAchievement(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);

        //    if (pet == null)
        //    {
        //        return NotFound();
        //    }

        //    var view = new PetAchievementViewModel
        //    {
        //        PetId = pet.Id,
        //        Pet = pet,
        //        PetAchievements = _combosHelper.GetComboAchievements(),
        //    };

        //    return View(view);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddAchievement(PetAchievementViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var petAchievement = await _converterHelper.ToPetAchievementAsync(model, true);
        //        _context.PetAchievements.Add(petAchievement);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction($"{nameof(DetailsPet)}/{model.PetId}");
        //    }
        //    model.PetAchievements = _combosHelper.GetComboAchievements();
        //    return View(model);
        //}

        public async Task<IActionResult> AddCommitment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id.Value);

            if (pet == null)
            {
                return NotFound();
            }

            var view = new CommitmentViewModel
            {
                PetOwnerId = pet.Owner.Id,
                PetId = pet.Id,
                Customers = _combosHelper.GetComboCustomers(),
                Price = 0,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1)
            };

            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommitment(CommitmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var commitment = await _converterHelper.ToCommitmentAsync(model, true);
                _context.Commitments.Add(commitment);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsPet)}/{model.PetId}");
            }
            model.Customers = _combosHelper.GetComboCustomers();
            return View(model);
        }


        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            var model = new PetImageViewModel
            {
                Id = pet.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(PetImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }

                var petImage = new PetImage
                {
                    ImageUrl = path,
                    Pet = await _context.Pets.FindAsync(model.Id)
                };

                _context.PetImages.Add(petImage);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsPet)}/{model.Id}");
            }

            return View(model);
        }

    }
}
