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
    public class AttendancesController : Controller
    {
        private readonly DBAttendanceContext _context;
        private readonly DBStudentContext dBStudentContext;
        public string SUBJECT_ID_COOKIE = "SUBJECT_ID_COOKIE";

        public string SUBJECT_NAME_COOKIE = "SUBJECT_NAME_COOKIE";

        public AttendancesController(DBAttendanceContext context, DBStudentContext dBStudentContext)
        {
            _context = context;
            this.dBStudentContext = dBStudentContext;
        }

        // GET: Attendances
        [SchoolAdminFilter]
        public async Task<IActionResult> Index(int? subjectId, string? subjectName)
        {
            if (subjectId == null && !HttpContext.Request.Cookies.ContainsKey(SUBJECT_ID_COOKIE))
            {
                return RedirectToAction("Index", "Subjects");
            }
            
            if (!HttpContext.Request.Cookies.ContainsKey(SUBJECT_ID_COOKIE))
            {
                HttpContext.Response.Cookies.Append(SUBJECT_ID_COOKIE, subjectId.ToString());
                HttpContext.Response.Cookies.Append(SUBJECT_NAME_COOKIE, subjectName);
                return RedirectToAction("Index");
            }

            var cookieSubjectId = int.Parse(HttpContext.Request.Cookies[SUBJECT_ID_COOKIE]);
            ViewBag.subjectName = HttpContext.Request.Cookies[SUBJECT_NAME_COOKIE];


            if ((subjectId != null && subjectId != cookieSubjectId) || ViewBag.subjectName == null)
            {
                HttpContext.Response.Cookies.Delete(SUBJECT_ID_COOKIE);
                return RedirectToAction("Index", new { subjectId = subjectId, subjectName  = subjectName });
            }

            return View(await _context.Attendances.ToListAsync());
        }

        // GET: Attendances/Details/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(m => m.id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        [SchoolAdminFilter]
        public IActionResult Create()
        {
            if (HttpContext.Request.Cookies[SUBJECT_ID_COOKIE] == null)
            {
                RedirectToAction("Index");
            }

            var attendance = new Attendance();
            attendance.id_subject = int.Parse(HttpContext.Request.Cookies[SUBJECT_ID_COOKIE]);

            var schoolID = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID).id;

            var existsStudents = _context.Attendances.Where(a => a.id_subject == attendance.id_subject).ToList();
            //get exist students in subject


            var notAddedStudents = dBStudentContext.Students
                            .Where(s => s.school_id == schoolID)
                            .ToList();

            //get all school's student
            notAddedStudents.RemoveAll(s => existsStudents.Any(es => s.id == es.id_student));

            ViewBag.notAddedStudents = notAddedStudents;


            return View(attendance);
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Create([Bind("id,id_subject,id_student,checkin,checkout")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                if(attendance.id_student == 0)
                {
                    TempData["error"] = "Please select student";
                    return RedirectToAction("Create", attendance);
                }

                attendance.checkin = new DateTime(2000,1,1);
                attendance.checkout = new DateTime(2000, 1, 1);

                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int id, [Bind("id,id_subject,id_student,checkin,checkout")] Attendance attendance)
        {
            if (id != attendance.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.id))
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
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(m => m.id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.id == id);
        }
    }
}
