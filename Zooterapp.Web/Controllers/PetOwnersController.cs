using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IConfiguration _configuration;

        public PetOwnersController(DataContext context, IUserHelper userHelper, IConfiguration configuration)
        {
            _context = context;
            _userHelper = userHelper;
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
        public async Task<IActionResult> Create(AddUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var user = await AddUserAsync(view);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(view);
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
            return View(view);
        }

        private async Task<User> AddUserAsync(AddUserViewModel view)
        {
            var user = new User
            {
                Address = view.Address,
                Document = view.Document,
                Email = view.Username,
                Name = view.FirstName,
                LastName = view.LastName,
                PhoneNumber = view.PhoneNumber,
                UserName = view.Username,
            };

            var result = await _userHelper.AddUserAsync(user, view.Password);

            if (result != IdentityResult.Success) return null;

            var newUser = await _userHelper.GetUserByEmailAsync(view.Username);
            await _userHelper.AddUserToRoleAsync(newUser, _configuration["Roles:PetOwner"]);

            return newUser;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petOwner = await _context.PetOwners.FindAsync(id);
            if (petOwner == null)
            {
                return NotFound();
            }
            return View(petOwner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] PetOwner petOwner)
        {
            if (id != petOwner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petOwner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetOwnerExists(petOwner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(petOwner);
        }

        // GET: PetOwners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petOwner = await _context.PetOwners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (petOwner == null)
            {
                return NotFound();
            }

            return View(petOwner);
        }

        // POST: PetOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var petOwner = await _context.PetOwners.FindAsync(id);
            _context.PetOwners.Remove(petOwner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetOwnerExists(int id)
        {
            return _context.PetOwners.Any(e => e.Id == id);
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
        public async Task<IActionResult> AddPet(PetViewModel view)
        {
            if (ModelState.IsValid)
            {
                var pet = await ToPetAsync(view);
                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        private async Task<Pet> ToPetAsync(PetViewModel view)
        {
            return new Pet
            {
                Name = view.Name,
                Age = view.Age,
                Race = view.Race,
                PetType = await _context.PetTypes.FindAsync(view.PetTypeID),
                Owner = await _context.PetOwners.FindAsync(view.PetOwnerId),
                IsAvailable = view.IsAvailable
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
    }
}
