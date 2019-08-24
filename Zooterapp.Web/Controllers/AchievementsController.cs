using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;
using Zooterapp.Web.Helpers;

namespace Zooterapp.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class AchievementsController : Controller
    {
        private readonly DataContext _context;

        public AchievementsController(DataContext context)
        {
            _context = context;
        }

        // GET: Achievements
        public async Task<IActionResult> Index()
        {
            return View(await _context.Achievements.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var achievement = await _context.Achievements.FindAsync(id);


            if (achievement == null)
            {
                return NotFound();
            }

            return View(achievement);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Achievement model)
        {
            if (ModelState.IsValid)
            {
                _context.Achievements.Update(model);
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

            var achievement = await _context.Achievements.FindAsync(id);

            if (achievement == null)
            {
                return NotFound();
            }

            _context.Achievements.Remove(achievement);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
