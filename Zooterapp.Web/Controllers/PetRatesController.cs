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
    public class PetRatesController : Controller
    {
        private readonly DataContext _context;

        public PetRatesController(DataContext context)
        {
            _context = context;
        }

        // GET: PetRates
        public async Task<IActionResult> Index()
        {
            return View(await _context.PetRates.ToListAsync());
        }

        // GET: PetRates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petRate = await _context.PetRates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (petRate == null)
            {
                return NotFound();
            }

            return View(petRate);
        }

        // GET: PetRates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PetRates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Rate,Comment")] PetRate petRate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petRate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(petRate);
        }

        // GET: PetRates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petRate = await _context.PetRates.FindAsync(id);
            if (petRate == null)
            {
                return NotFound();
            }
            return View(petRate);
        }

        // POST: PetRates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rate,Comment")] PetRate petRate)
        {
            if (id != petRate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petRate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetRateExists(petRate.Id))
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
            return View(petRate);
        }

        // GET: PetRates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petRate = await _context.PetRates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (petRate == null)
            {
                return NotFound();
            }

            return View(petRate);
        }

        // POST: PetRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var petRate = await _context.PetRates.FindAsync(id);
            _context.PetRates.Remove(petRate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetRateExists(int id)
        {
            return _context.PetRates.Any(e => e.Id == id);
        }
    }
}
