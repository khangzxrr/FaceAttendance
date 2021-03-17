using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Data;
using SpeedyAPI.Models;

namespace SpeedyAPI.Controllers
{
    public class SchoolAccountsController : Controller
    {
        private readonly MvcSpeedyAPIContext _context;

        public SchoolAccountsController(MvcSpeedyAPIContext context)
        {
            _context = context;
        }

        // GET: SchoolAccounts
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(AdminController.SESSION_ADMIN_ROLE) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(await _context.SchoolAccounts.ToListAsync());
        }


        // GET : SchoolAccounts/Login
        public IActionResult Login()
        {
            return View();
        }

        // GET: SchoolAccounts/Details/5
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
            if (HttpContext.Session.GetInt32(KeysController.SESSION_KEY_ID) != null ||
                HttpContext.Session.GetString(AdminController.SESSION_ADMIN_ROLE) != null)
            {
                return View();
            }

            return RedirectToAction("index", "home");
        }

        // POST: SchoolAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,username,password")] SchoolAccount schoolAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schoolAccount);
                await _context.SaveChangesAsync();

                if (HttpContext.Session.GetString(KeysController.SESSION_KEY_ID) != null)
                {
                    //remove key id if user using key to create school account
                    HttpContext.Session.SetString(KeysController.SESSION_KEY_ID, null);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(schoolAccount);
        }

        // GET: SchoolAccounts/Edit/5
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
