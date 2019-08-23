using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;

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
        public IActionResult Index()
        {
            return View(_context.Achievements
                .Include(a => a.PetAchievements)
                .ThenInclude(pa => pa.Pet)
                .ThenInclude(p => p.Name)
                .Include(a => a.PetAchievements)
                .ThenInclude(pa => pa.Pet)
                .ThenInclude(p => p.Age)
                .Include(a => a.PetAchievements)
                .ThenInclude(pa => pa.Pet)
                .ThenInclude(p => p.Race)
                .Include(a => a.PetAchievements.Count)
                .Include(a => a.PetAchievements)
                .ThenInclude(pa => pa.Pet)
                .ThenInclude(p => p.Owner));
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
                .ThenInclude(p => p.Name)
                .Include(a => a.PetAchievements)
                .ThenInclude(pa => pa.Pet)
                .ThenInclude(p => p.Age)
                .Include(a => a.PetAchievements)
                .ThenInclude(pa => pa.Pet)
                .ThenInclude(p => p.Race)
                .Include(a => a.PetAchievements.Count)
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
    }
}
