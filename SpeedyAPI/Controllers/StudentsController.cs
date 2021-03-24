using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Data;
using SpeedyAPI.Extensions;
using SpeedyAPI.Filters;
using SpeedyAPI.Models;

namespace SpeedyAPI.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DBStudentContext _context;

        private IWebHostEnvironment webHostEnvironment;

        public StudentsController(DBStudentContext context, IWebHostEnvironment environment)
        {
            _context = context;
            webHostEnvironment = environment;
        }

        // GET: Students
        [SchoolAdminFilter]
        public async Task<IActionResult> Index()
        {
            var school = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);

            if (school != null)
            {
                return View(await _context.Students.Where(s => s.school_id == school.id).ToListAsync());
            }
            else
            if (HttpContext.Session.GetString(AdminController.SESSION_ADMIN_ROLE) != null)
            {
                return View(await _context.Students.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "SchoolAccounts");
            }

            
        }

        // GET: Students/Details/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [SchoolAdminFilter]
        public IActionResult Create()
        {
            var school = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);

            ViewBag.schoolId = school.id;

            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Create([Bind("id,name,imageFile,date_of_birth,school_id")] Student student)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(student.imageFile.FileName);
                string extension = Path.GetExtension(student.imageFile.FileName);

                string imageName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = webHostEnvironment.WebRootPath + "/upload/" + imageName;
                using(var fileStream = new FileStream(path, FileMode.Create))
                {
                    await student.imageFile.CopyToAsync(fileStream);
                }

                student.image_url = path;

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [SchoolAdminFilter]
        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,imageFile,date_of_birth,school_id")] Student student)
        {
            if (id != student.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(student.imageFile.FileName);
                    string extension = Path.GetExtension(student.imageFile.FileName);

                    string imageName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = webHostEnvironment.WebRootPath + "/upload/" + imageName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await student.imageFile.CopyToAsync(fileStream);
                    }

                    student.image_url = imageName;

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.id == id);
        }
    }
}
