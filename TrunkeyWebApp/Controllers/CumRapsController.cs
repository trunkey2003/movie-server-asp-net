#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrunkeyWebApp.Models;

namespace TrunkeyWebApp.Controllers
{
    public class CumRapsController : Controller
    {
        private readonly movie_server_cybersoftContext _context;
        private readonly Middlewares.IAuthorization _Authorization;

        public CumRapsController(movie_server_cybersoftContext context, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _Authorization = Authorization;
        }

        // GET: CumRaps
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            var movie_server_cybersoftContext = _context.CumRaps.Include(c => c.MaHeThongRapNavigation);
            return View(await movie_server_cybersoftContext.ToListAsync());
        }

        // GET: CumRaps/Details/5
        public async Task<IActionResult> Details(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var cumRap = await _context.CumRaps
                .Include(c => c.MaHeThongRapNavigation)
                .FirstOrDefaultAsync(m => m.MaCumRap == id);
            if (cumRap == null)
            {
                return NotFound();
            }

            return View(cumRap);
        }

        // GET: CumRaps/Create
        public IActionResult Create()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            ViewData["MaHeThongRap"] = new SelectList(_context.HeThongRaps, "MaHeThongRap", "MaHeThongRap");
            return View();
        }

        // POST: CumRaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCumRap,TenCumRap,DiaChi,MaHeThongRap")] CumRap cumRap)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(cumRap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHeThongRap"] = new SelectList(_context.HeThongRaps, "MaHeThongRap", "MaHeThongRap", cumRap.MaHeThongRap);
            return View(cumRap);
        }

        // GET: CumRaps/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var cumRap = await _context.CumRaps.FindAsync(id);
            if (cumRap == null)
            {
                return NotFound();
            }
            ViewData["MaHeThongRap"] = new SelectList(_context.HeThongRaps, "MaHeThongRap", "MaHeThongRap", cumRap.MaHeThongRap);
            return View(cumRap);
        }

        // POST: CumRaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaCumRap,TenCumRap,DiaChi,MaHeThongRap")] CumRap cumRap)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (id != cumRap.MaCumRap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cumRap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CumRapExists(cumRap.MaCumRap))
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
            ViewData["MaHeThongRap"] = new SelectList(_context.HeThongRaps, "MaHeThongRap", "MaHeThongRap", cumRap.MaHeThongRap);
            return View(cumRap);
        }

        // GET: CumRaps/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var cumRap = await _context.CumRaps
                .Include(c => c.MaHeThongRapNavigation)
                .FirstOrDefaultAsync(m => m.MaCumRap == id);
            if (cumRap == null)
            {
                return NotFound();
            }

            return View(cumRap);
        }

        // POST: CumRaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var cumRap = await _context.CumRaps.FindAsync(id);
            _context.CumRaps.Remove(cumRap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CumRapExists(string id)
        {
            return _context.CumRaps.Any(e => e.MaCumRap == id);
        }
    }
}
