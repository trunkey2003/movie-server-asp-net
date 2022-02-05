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
    public class RapsController : Controller
    {
        private readonly movieContext _context;
        private readonly Middlewares.IAuthorization _Authorization;

        public RapsController(movieContext context, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _Authorization = Authorization;
        }

        // GET: Raps
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            var movieContext = _context.Raps.Include(r => r.MaCumRapNavigation);
            return View(await movieContext.ToListAsync());
        }

        // GET: Raps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var rap = await _context.Raps
                .Include(r => r.MaCumRapNavigation)
                .FirstOrDefaultAsync(m => m.MaRap == id);
            if (rap == null)
            {
                return NotFound();
            }

            return View(rap);
        }

        // GET: Raps/Create
        public IActionResult Create()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            ViewData["MaCumRap"] = new SelectList(_context.CumRaps, "MaCumRap", "MaCumRap");
            return View();
        }

        // POST: Raps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaRap,TenRap,MaCumRap")] Rap rap)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(rap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaCumRap"] = new SelectList(_context.CumRaps, "MaCumRap", "MaCumRap", rap.MaCumRap);
            return View(rap);
        }

        // GET: Raps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var rap = await _context.Raps.FindAsync(id);
            if (rap == null)
            {
                return NotFound();
            }
            ViewData["MaCumRap"] = new SelectList(_context.CumRaps, "MaCumRap", "MaCumRap", rap.MaCumRap);
            return View(rap);
        }

        // POST: Raps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaRap,TenRap,MaCumRap")] Rap rap)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (id != rap.MaRap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RapExists(rap.MaRap))
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
            ViewData["MaCumRap"] = new SelectList(_context.CumRaps, "MaCumRap", "MaCumRap", rap.MaCumRap);
            return View(rap);
        }

        // GET: Raps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var rap = await _context.Raps
                .Include(r => r.MaCumRapNavigation)
                .FirstOrDefaultAsync(m => m.MaRap == id);
            if (rap == null)
            {
                return NotFound();
            }

            return View(rap);
        }

        // POST: Raps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var rap = await _context.Raps.FindAsync(id);
            _context.Raps.Remove(rap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RapExists(int id)
        {
            return _context.Raps.Any(e => e.MaRap == id);
        }
    }
}
