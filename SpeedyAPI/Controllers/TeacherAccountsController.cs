using System.Collections.Generic;
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
    public class TeacherAccountsController : Controller
    {
        private readonly DBTeacherContext _context;

        public TeacherAccountsController(DBTeacherContext context)
        {
            _context = context;
        }

        [SchoolAdminFilter]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherAccount>>> GetTeacherAccounts()
        {
            return await _context.TeacherAccounts.ToListAsync();
        }

        // GET: TeacherAccounts
        [SchoolAdminFilter]
        public async Task<IActionResult> Index()
        {
            var school = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);

            if (school != null)
            {
                return View(await _context.TeacherAccounts.Where(t => t.teach_in_school == school.id).ToListAsync());
            }

            return View(await _context.TeacherAccounts.ToListAsync());
        }

        // GET: TeacherAccounts/Details/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherAccount = await _context.TeacherAccounts
                .FirstOrDefaultAsync(m => m.id == id);
            if (teacherAccount == null)
            {
                return NotFound();
            }

            return View(teacherAccount);
        }

        // GET: TeacherAccounts/Create
        [SchoolAdminFilter]
        public IActionResult Create()
        {
            var school = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);
            ViewBag.schoolID = school.id;

            return View();
        }

        // POST: TeacherAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Create([Bind("id,email,password,teach_in_school")] TeacherAccount teacherAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacherAccount);
        }

        // GET: TeacherAccounts/Edit/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherAccount = await _context.TeacherAccounts.FindAsync(id);
            if (teacherAccount == null)
            {
                return NotFound();
            }
            return View(teacherAccount);
        }

        // POST: TeacherAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int id, [Bind("id,email,password,teach_in_school")] TeacherAccount teacherAccount)
        {
            if (id != teacherAccount.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherAccountExists(teacherAccount.id))
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
            return View(teacherAccount);
        }

        // GET: TeacherAccounts/Delete/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherAccount = await _context.TeacherAccounts
                .FirstOrDefaultAsync(m => m.id == id);
            if (teacherAccount == null)
            {
                return NotFound();
            }

            return View(teacherAccount);
        }

        // POST: TeacherAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherAccount = await _context.TeacherAccounts.FindAsync(id);
            _context.TeacherAccounts.Remove(teacherAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherAccountExists(int id)
        {
            return _context.TeacherAccounts.Any(e => e.id == id);
        }
    }
}
