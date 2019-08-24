using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Zooterapp.Web.Data;

namespace Zooterapp.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CommitmentsController : Controller
    {
        private readonly DataContext _context;

        public CommitmentsController(DataContext context)
        {
            _context = context;
        }

        // GET: Commitments
        public IActionResult Index()
        {
            return View(_context.Commitments
                .Include(c => c.PetOwner)
                .ThenInclude(p => p.User)
                .Include(c => c.Customer)
                .ThenInclude(cu => cu.User)
                .Include(c => c.Pet)
                .ThenInclude(p => p.PetType));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commitment = await _context.Commitments
                .Include(c => c.PetOwner)
                .ThenInclude(p => p.User)
                .Include(c => c.Customer)
                .ThenInclude(cu => cu.User)
                .Include(c => c.Pet)
                .ThenInclude(p => p.PetType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commitment == null)
            {
                return NotFound();
            }

            return View(commitment);
        }
    }
}