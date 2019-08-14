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
    public class CommitmentsController : Controller
    {
        private readonly DataContext _context;

        public CommitmentsController(DataContext context)
        {
            _context = context;
        }

        // GET: Commitments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Commitments.ToListAsync());
        }

        // GET: Commitments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commitment = await _context.Commitments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commitment == null)
            {
                return NotFound();
            }

            return View(commitment);
        }

        // GET: Commitments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Commitments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Remarks,Price,StartDate,EndDate,IsActive")] Commitment commitment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commitment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commitment);
        }

        // GET: Commitments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commitment = await _context.Commitments.FindAsync(id);
            if (commitment == null)
            {
                return NotFound();
            }
            return View(commitment);
        }

        // POST: Commitments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Remarks,Price,StartDate,EndDate,IsActive")] Commitment commitment)
        {
            if (id != commitment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commitment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommitmentExists(commitment.Id))
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
            return View(commitment);
        }

        // GET: Commitments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commitment = await _context.Commitments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commitment == null)
            {
                return NotFound();
            }

            return View(commitment);
        }

        // POST: Commitments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commitment = await _context.Commitments.FindAsync(id);
            _context.Commitments.Remove(commitment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommitmentExists(int id)
        {
            return _context.Commitments.Any(e => e.Id == id);
        }
    }
}
