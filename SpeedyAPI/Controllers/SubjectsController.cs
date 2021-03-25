using System;
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
    public class SubjectsController : Controller
    {
        private readonly DBSubjectContext _context;
        private readonly DBTeacherContext teacherContext;
        public string MAJOR_DETAIL_COOKIE = "MAJOR_DETAIL_COOKIE";

        public SubjectsController(DBSubjectContext context, DBTeacherContext teacherContext)
        {
            _context = context;
            this.teacherContext = teacherContext;
        }

        // GET: Subjects
        [SchoolAdminFilter]
        public async Task<IActionResult> Index(int? id, string? majorName)
        {
            //id majorName != null  => setcookie
            //id majorName == null => getFromCookie
            //id majorName != null but different from cookies => re-setcookie

            if ((id == null || majorName == null) && HttpContext.Request.Cookies[MAJOR_DETAIL_COOKIE] == null)
            {
                return RedirectToAction("Index", "Majors");
            }

            if (HttpContext.Request.Cookies[MAJOR_DETAIL_COOKIE] == null)
            {
                HttpContext.Response.Cookies.Append(MAJOR_DETAIL_COOKIE, id + "|" + majorName,
                                               new CookieOptions()
                                               {
                                                   Expires = DateTime.Now.AddMinutes(30)
                                               });
                return RedirectToAction("Index", new { id = id, majorName = majorName });
            }

            var majorDataStr = HttpContext.Request.Cookies[MAJOR_DETAIL_COOKIE].Split("|");
            var cookieMajorId = int.Parse(majorDataStr[0]);
            var cookieMajorName = majorDataStr[1];

            ViewBag.majorId = int.Parse(majorDataStr[0]);
            ViewBag.majorName = majorDataStr[1];
            
            if (id != null && id != int.Parse(majorDataStr[0])) //if different id from cookie and parameter
            {
                HttpContext.Response.Cookies.Delete(MAJOR_DETAIL_COOKIE);
                return RedirectToAction("Index", new { id = id, majorName = majorName });
            }


            return View(await _context.Subjects.Where(s => s.major_id == cookieMajorId).Include(s => s.teacherAccount).ToListAsync());
        }

        // GET: Subjects/Details/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        [SchoolAdminFilter]
        public IActionResult Create(int? majorId)
        {
            var school = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);
            ViewBag.majorId = majorId;
            ViewBag.teachers = teacherContext.TeacherAccounts
                                                .Where(t => t.teach_in_school == school.id)
                                                .ToList();

            var initModel = new Subject();
            initModel.major_id = (int)majorId;
            return View(initModel);
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Create([Bind("id,name,room,major_id,teacher_observer")] Subject subject)
        {

            if (ModelState.IsValid)
            {

                var school = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);
                ViewBag.teachers = teacherContext.TeacherAccounts
                                                    .Where(t => t.teach_in_school == school.id)
                                                    .ToList();

                if (subject.teacher_observer == -1)
                {
                    ViewBag.error = "Please select observer teacher";
                    return View(subject);
                }

                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = subject.major_id });
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,room,major_id,teacher_observer")] Subject subject)
        {
            if (id != subject.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.id))
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
            return View(subject);
        }

        // GET: Subjects/Delete/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [SchoolAdminFilter]
        public IActionResult AddStudents(int? id, string? name)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Attendances", new { subjectId = id, subjectName = name });
        }


        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.id == id);
        }
    }
}
