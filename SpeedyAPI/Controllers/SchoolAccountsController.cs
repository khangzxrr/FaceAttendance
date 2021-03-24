using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Data;
using SpeedyAPI.Models;
using SpeedyAPI.Extensions;
using Microsoft.AspNetCore.Http;
using SpeedyAPI.Filters;

namespace SpeedyAPI.Controllers
{
    public class SchoolAccountsController : Controller
    {
        private readonly DBSchoolLoginContext _context;
        private readonly DBKeyContext keyContext;
        public static string SCHOOL_SESSION_ACCOUNT_ID = "school_account_id";

        public SchoolAccountsController(DBSchoolLoginContext context, DBKeyContext keyContext)
        {
            _context = context;
            this.keyContext = keyContext;
        }

        // GET: SchoolAccounts
        [SchoolAdminFilter]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(AdminController.SESSION_ADMIN_ROLE) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            if (HttpContext.Session.Get<SchoolAccount>(SCHOOL_SESSION_ACCOUNT_ID) != null)
            {
                return RedirectToAction("Manage");
            }

            return View(await _context.SchoolAccounts.ToListAsync());
        }


        [SchoolAdminFilter]
        public IActionResult Manage()
        {
            var school = HttpContext.Session.Get<SchoolAccount>(SCHOOL_SESSION_ACCOUNT_ID);

            ViewBag.schoolName = school.name;

            ViewBag.majorsCount = _context.SchoolAccounts
                                            .Where(s => s.id == school.id)
                                            .Include(s => s.Majors)
                                            .FirstOrDefault().Majors.Count;
            ViewBag.studentsCount = _context.SchoolAccounts
                                            .Where(s => s.id == school.id)
                                            .Include(s => s.Students)
                                            .FirstOrDefault().Students.Count;

            ViewBag.teachersCount = _context.SchoolAccounts
                                            .Where(s => s.id == school.id)
                                            .Include(s => s.TeacherAccounts)
                                            .FirstOrDefault().TeacherAccounts.Count;

            return View();

        }

        //GET: SchoolAccounts/Logout
        [SchoolAdminFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        // GET : SchoolAccounts/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID) == null)
            {
                return View();
            }

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var logginAccount = _context.SchoolAccounts.FirstOrDefault(
                s => s.username == username && s.password == password);

            if (logginAccount != null)
            {
                HttpContext.Session.Set(SCHOOL_SESSION_ACCOUNT_ID, logginAccount);

                return RedirectToAction("Manage");
            }
            else
            {
                ViewBag.error = "username or password is incorrect, try again";
            }

            return View();
        }


        // GET: SchoolAccounts/Details/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolAccount = await _context.SchoolAccounts
                .FirstOrDefaultAsync(m => m.id == id);
            if (schoolAccount == null)
            {
                return NotFound();
            }

            return View(schoolAccount);
        }

        // GET: SchoolAccounts/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.Get(KeysController.SESSION_USED_KEY) == null)
            {
                return RedirectToAction("Use", "Keys");
            }

            return View();

        }

        // POST: SchoolAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,username,password")] SchoolAccount schoolAccount)
        {
            if (HttpContext.Session.Get(KeysController.SESSION_USED_KEY) == null)
            {
                return RedirectToAction("Use", "Keys");
            }
            if (ModelState.IsValid)
            {
                var existAccount = _context.SchoolAccounts.FirstOrDefault(s => s.username == schoolAccount.username);

                if (existAccount == null)
                {


                    _context.Add(schoolAccount);
                    await _context.SaveChangesAsync();

                    //====== KEY REMOVE
                    var key = HttpContext.Session.Get<Key>(KeysController.SESSION_USED_KEY);
                    key.isUsed = true;

                    keyContext.Update(key);
                    await keyContext.SaveChangesAsync();

                    HttpContext.Session.Remove(KeysController.SESSION_USED_KEY);
                    //===========================

                    var schoolAdded = await _context.SchoolAccounts
                            .Where(s => s.username == schoolAccount.username)
                            .FirstOrDefaultAsync();
                    HttpContext.Session.Set(SCHOOL_SESSION_ACCOUNT_ID, schoolAdded);

                    return RedirectToAction("Login");

                }
                else
                {
                    ViewBag.error = "Username is exist, please choose another one";
                }
            }

            return View();
        }

        // GET: SchoolAccounts/Edit/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolAccount = await _context.SchoolAccounts.FindAsync(id);
            if (schoolAccount == null)
            {
                return NotFound();
            }
            return View(schoolAccount);
        }

        // POST: SchoolAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,username,password")] SchoolAccount schoolAccount)
        {
            if (id != schoolAccount.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schoolAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolAccountExists(schoolAccount.id))
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
            return View(schoolAccount);
        }

        // GET: SchoolAccounts/Delete/5
        [SchoolAdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolAccount = await _context.SchoolAccounts
                .FirstOrDefaultAsync(m => m.id == id);
            if (schoolAccount == null)
            {
                return NotFound();
            }

            return View(schoolAccount);
        }

        // POST: SchoolAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SchoolAdminFilter]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schoolAccount = await _context.SchoolAccounts.FindAsync(id);
            _context.SchoolAccounts.Remove(schoolAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolAccountExists(int id)
        {
            return _context.SchoolAccounts.Any(e => e.id == id);
        }
    }
}
