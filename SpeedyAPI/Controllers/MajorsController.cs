using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Data;
using SpeedyAPI.Extensions;
using SpeedyAPI.Models;

namespace SpeedyAPI.Controllers
{
    public class MajorsController : Controller
    {
        private readonly MvcSpeedyAPIContext _context;

        public MajorsController(MvcSpeedyAPIContext context)
        {
            _context = context;
        }

        // GET: Majors
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(AdminController.SESSION_ADMIN_ROLE) != null)
            {
                return View(await _context.Major.ToListAsync());
            }
            else
            if (HttpContext.Session.GetInt32(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID) != null)
            {
                int schoolid = (int)HttpContext.Session.GetInt32(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);
                return View(await _context.Major.Where(major => major.school_id == schoolid).ToListAsync());
            }

            ViewBag.error = "Please login before manage majors";

            return RedirectToAction("Login", "SchoolAccounts");
        }

        // GET: Majors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Major
                .FirstOrDefaultAsync(m => m.id == id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // GET: Majors/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID) == null)
            {
                return RedirectToAction("Login", "SchoolAccounts");
            }

            ViewBag.schoolId = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID).id;
            return View();
        }

        // POST: Majors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,school_id,name,startDate")] Major major)
        {
            if (ModelState.IsValid)
            {
                _context.Add(major);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(major);
        }

        // GET: Majors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Major.FindAsync(id);
            if (major == null)
            {
                return NotFound();
            }
            return View(major);
        }

        // POST: Majors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,school_id,name,startDate")] Major major)
        {
            if (id != major.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(major);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MajorExists(major.id))
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
            return View(major);
        }

        // GET: Majors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Major
                .FirstOrDefaultAsync(m => m.id == id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // POST: Majors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var major = await _context.Major.FindAsync(id);
            _context.Major.Remove(major);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MajorExists(int id)
        {
            return _context.Major.Any(e => e.id == id);
        }
    }
}
