using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;
using Zooterapp.Web.Helpers;
using Zooterapp.Web.Models;

namespace Zooterapp.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class AchievementsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public AchievementsController(DataContext context, 
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        // GET: Achievements
        public async Task<IActionResult> Index()
        {
            //return View(await _context.PetAchievements
            //    .Include(pa => pa.Achievement)
            //    .Include(pa => pa.Pet)
            //        .ThenInclude(p => p.Owner).ToListAsync()
            //    );
            return View(await _context.Achievements.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievements
                .Include(a => a.PetAchievements)
                .ThenInclude(pa => pa.Pet)
                .ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (achievement == null)
            {
                return NotFound();
            }

            return View(achievement);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Achievement model)
        {
            if (ModelState.IsValid)
            {
                _context.Achievements.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> EditAchievements(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var petachievement = await _context.PetAchievements
                .Include(p => p.Achievement)
                .Include(p => p.Pet)
                .FirstOrDefaultAsync(p => p.Id == id);


            if (petachievement == null)
            {
                return NotFound();
            }

            var view = _converterHelper.ToPetAchievement(petachievement);
            return View(view);

        }


        [HttpPost]
        public async Task<IActionResult> EditAchievements(PetAchievementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var petachievement = await _converterHelper.ToPetAchievementAsync(model, false);
                _context.PetAchievements.Update(petachievement);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.PetOwnerId}");
            }

            return View(model);
        }
    }
}
