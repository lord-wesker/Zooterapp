using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class PetAchievementsController : Controller
    {
        private readonly DataContext _context;

        public PetAchievementsController(DataContext context)
        {
            _context = context;
        }

        // GET: PetAchievements
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.PetAchievements.Include(p => p.Achievement).Include(p => p.Pet);
            return View(await dataContext.ToListAsync());
        }

        // GET: PetAchievements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petAchievement = await _context.PetAchievements
                .Include(p => p.Achievement)
                .Include(p => p.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (petAchievement == null)
            {
                return NotFound();
            }

            return View(petAchievement);
        }

        // GET: PetAchievements/Create
        public IActionResult Create()
        {
            ViewData["AchievementId"] = new SelectList(_context.Achievements, "Id", "Id");
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Id");
            return View();
        }

        // POST: PetAchievements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PetId,AchievementId")] PetAchievement petAchievement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petAchievement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AchievementId"] = new SelectList(_context.Achievements, "Id", "Id", petAchievement.AchievementId);
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Id", petAchievement.PetId);
            return View(petAchievement);
        }

        // GET: PetAchievements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petAchievement = await _context.PetAchievements.FindAsync(id);
            if (petAchievement == null)
            {
                return NotFound();
            }
            ViewData["AchievementId"] = new SelectList(_context.Achievements, "Id", "Id", petAchievement.AchievementId);
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Id", petAchievement.PetId);
            return View(petAchievement);
        }

        // POST: PetAchievements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PetId,AchievementId")] PetAchievement petAchievement)
        {
            if (id != petAchievement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petAchievement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetAchievementExists(petAchievement.Id))
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
            ViewData["AchievementId"] = new SelectList(_context.Achievements, "Id", "Id", petAchievement.AchievementId);
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Id", petAchievement.PetId);
            return View(petAchievement);
        }

        // GET: PetAchievements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petAchievement = await _context.PetAchievements
                .Include(p => p.Achievement)
                .Include(p => p.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (petAchievement == null)
            {
                return NotFound();
            }

            return View(petAchievement);
        }

        // POST: PetAchievements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var petAchievement = await _context.PetAchievements.FindAsync(id);
            _context.PetAchievements.Remove(petAchievement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetAchievementExists(int id)
        {
            return _context.PetAchievements.Any(e => e.Id == id);
        }
    }
}
