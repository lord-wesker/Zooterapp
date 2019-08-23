using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public PetOwnersController(DataContext context,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConfiguration configuration)
        {
            _context = context;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _configuration = configuration;
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

        public async Task<IActionResult> EditCommitment(int? id)
        {
            if (id == null) return NotFound();

            var commitment = await _context.Commitments
                .Include(c => c.PetOwner)
                .Include(c => c.Customer)
                .Include(c => c.Pet)
                .FirstOrDefaultAsync(c => c.Id == id.Value);

            if (commitment == null) return NotFound();

            return View(ToCommitmentViewModel(commitment));
        }

        [HttpPost]
        public async Task<IActionResult> EditCommitment(CommitmentViewModel model)
        {
            if(ModelState.IsValid)
            {
                var commitment = await ToCommitmentAsync(model, false);
                _context.Commitments.Update(commitment);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsPet)}/{model.PetOwnerId}");
            }

            return View(model);
        }



        ///

        public async Task<IActionResult> AddPet(int? id)
        {
            if (id == null) return NotFound();

            var petOwner = await _context.PetOwners.FindAsync(id.Value);

            if (petOwner == null) return NotFound();

            var view = new PetViewModel
            {
                PetOwnerId = petOwner.Id,
                PetTypes = GetComboPets(),
            };

            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> AddPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pet = await ToPetAsync(model, true);
                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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

            var view = ToPetViewModel(pet);
            return View(view);

        }

        [HttpPost]
        public async Task<IActionResult> EditPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pet = await ToPetAsync(model, false);
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }


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
                Customers = GetComboCustomers(),
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
                var commitment = await ToCommitmentAsync(model, true);
                _context.Commitments.Add(commitment);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsPet)}/{model.PetId}");
            }

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


        /// HELPERS & CONVERTERS

        private PetViewModel ToPetViewModel(Pet pet)
        {
            return new PetViewModel
            {
                Id = pet.Id,
                Name = pet.Name,
                Race = pet.Race,
                Age = pet.Age,
                PetOwnerId = pet.Owner.Id,
                PetTypes = GetComboPets(),
                PetTypeID = pet.PetType.Id,
                IsAvailable = pet.IsAvailable,
            };
        }

        private async Task<Pet> ToPetAsync(PetViewModel model, bool isNew)
        {
            return new Pet
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Age = model.Age,
                Race = model.Race,
                PetType = await _context.PetTypes.FindAsync(model.PetTypeID),
                Owner = await _context.PetOwners.FindAsync(model.PetOwnerId),
                IsAvailable = model.IsAvailable,
                Commitments = isNew ? new List<Commitment>() : model.Commitments,
                PetImages = isNew ? new List<PetImage>() : model.PetImages,
                PetAchievements = isNew ? new List<PetAchievement>() : model.PetAchievements,
            };
        }

        private async Task<Commitment> ToCommitmentAsync(CommitmentViewModel view, bool isNew)
        {
            return new Commitment
            {
                Id = isNew ? 0 : view.Id,
                EndDate = view.EndDate,
                IsActive = view.IsActive,
                Customer = await _context.Customers.FindAsync(view.CustomerId),
                PetOwner = await _context.PetOwners.FindAsync(view.PetOwnerId),
                Price = view.Price,
                Pet = await _context.Pets.FindAsync(view.PetId),
                Remarks = view.Remarks,
                StartDate = view.StartDate,
            };
        }

        private CommitmentViewModel ToCommitmentViewModel(Commitment commitment)
        {
            return new CommitmentViewModel
            {
                Id = commitment.Id,
                Customer = commitment.Customer,
                CustomerId = commitment.Customer.Id,
                Customers = GetComboCustomers(),
                Pet = commitment.Pet,
                PetId = commitment.Pet.Id,
                PetOwner = commitment.PetOwner,
                PetOwnerId = commitment.PetOwner.Id,
                Price = commitment.Price,
                Remarks = commitment.Remarks,
                IsActive = commitment.IsActive,
                StartDate = commitment.StartDate,
                EndDate = commitment.EndDate
            };
        }

        private IEnumerable<SelectListItem> GetComboPets()
        {
            var list = _context.PetTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = pt.Id.ToString()
            }).OrderBy(pt => pt.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Pet Type ...)",
                Value = "0",
            });

            return list;
        }

        private IEnumerable<SelectListItem> GetComboCustomers()
        {
            var list = _context.Customers.Include(c => c.User).Select(p => new SelectListItem
            {
                Text = p.User.FullNameWithDocument,
                Value = p.Id.ToString()
            }).OrderBy(p => p.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a customer...)",
                Value = "0"
            });

            return list;

        }
    }
}
