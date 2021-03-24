using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Data;
using SpeedyAPI.Extensions;
using SpeedyAPI.Filters;
using SpeedyAPI.Models;

namespace SpeedyAPI.Controllers
{
    public class MajorsController : Controller
    {
        private readonly DBMajorContext _context;

        public MajorsController(DBMajorContext context)
        {
            _context = context;
        }

        // GET: Majors
        [SchoolAdminFilter]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(AdminController.SESSION_ADMIN_ROLE) != null)
            {
                return View(await _context.Majors.ToListAsync());
            }
            else
            if (HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID) != null)
            {
                var school = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);
                return View(await _context.Majors.Where(major => major.school_id == school.id).ToListAsync());
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Majors/Details/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Majors
                .FirstOrDefaultAsync(m => m.id == id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // GET: Majors/Create
        [SchoolAdminFilter]
        public IActionResult Create()
        {
            ViewBag.schoolId = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID).id;
            return View();
        }

        // POST: Majors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
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
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Majors.FindAsync(id);
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
        [SchoolAdminFilter]
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
        [SchoolAdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Majors
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
        [SchoolAdminFilter]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var major = await _context.Majors.FindAsync(id);
            _context.Majors.Remove(major);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [SchoolAdminFilter]
        public async Task<IActionResult> AddStudents(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Majors
               .FirstOrDefaultAsync(m => m.id == id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        private bool MajorExists(int id)
        {
            return _context.Majors.Any(e => e.id == id);
        }
    }
}
