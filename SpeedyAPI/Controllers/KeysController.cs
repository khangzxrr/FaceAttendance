using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Data;
using SpeedyAPI.Models;
using SpeedyAPI.Extensions;

namespace SpeedyAPI.Controllers
{
    public class KeysController : Controller
    {
        private readonly MvcSpeedyAPIContext _context;

        public static string SESSION_USED_KEY = "session_key_id";

        public KeysController(MvcSpeedyAPIContext context)
        {
            _context = context;
        }

        // GET: Keys/Use
        public IActionResult Use()
        {
            return View("Use");
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Use(string keyText)
        {
            var key = await _context.keys.FirstOrDefaultAsync(m => m.keyText.Equals(keyText));

            if (key == null)
            {
                ViewBag.error = "Key not found, you can buy one at ...";
                return View("Use");
            }

            HttpContext.Session.Set(SESSION_USED_KEY, key);
            
            return RedirectToAction("Create", "SchoolAccounts");
        }


        // GET: Keys
        public async Task<IActionResult> Index()
        {
            return View(await _context.keys.ToListAsync());
        }

        // GET: Keys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var key = await _context.keys
                .FirstOrDefaultAsync(m => m.id == id);
            if (key == null)
            {
                return NotFound();
            }

            return View(key);
        }

        // GET: Keys/Create
        
        public IActionResult Create()
        {
            return View();
        }

        // POST: Keys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,keyText,isUsed,key_type,create_date,expiry_date")] Key key)
        {
            if (ModelState.IsValid)
            {
                _context.Add(key);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(key);
        }

        // GET: Keys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var key = await _context.keys.FindAsync(id);
            if (key == null)
            {
                return NotFound();
            }
            return View(key);
        }

        // POST: Keys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,keyText,isUsed,key_type,create_date,expiry_date")] Key key)
        {
            if (id != key.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(key);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyExists(key.id))
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
            return View(key);
        }

        // GET: Keys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var key = await _context.keys
                .FirstOrDefaultAsync(m => m.id == id);
            if (key == null)
            {
                return NotFound();
            }

            return View(key);
        }

        // POST: Keys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var key = await _context.keys.FindAsync(id);
            _context.keys.Remove(key);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeyExists(int id)
        {
            return _context.keys.Any(e => e.id == id);
        }
    }
}
