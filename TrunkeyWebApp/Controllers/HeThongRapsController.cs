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
    public class HeThongRapsController : Controller
    {
        private readonly movieContext _context;
        private readonly Middlewares.IAuthorization _Authorization;

        public HeThongRapsController(movieContext context, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _Authorization = Authorization;
        }

        // GET: HeThongRaps
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            return View(await _context.HeThongRaps.ToListAsync());
        }

        // GET: HeThongRaps/Details/5
        public async Task<IActionResult> Details(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var heThongRap = await _context.HeThongRaps
                .FirstOrDefaultAsync(m => m.MaHeThongRap == id);
            if (heThongRap == null)
            {
                return NotFound();
            }

            return View(heThongRap);
        }

        // GET: HeThongRaps/Create
        public IActionResult Create()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            return View();
        }

        // POST: HeThongRaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHeThongRap,TenHeThongRap,BiDanh,Logo")] HeThongRap heThongRap)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(heThongRap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(heThongRap);
        }

        // GET: HeThongRaps/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var heThongRap = await _context.HeThongRaps.FindAsync(id);
            if (heThongRap == null)
            {
                return NotFound();
            }
            return View(heThongRap);
        }

        // POST: HeThongRaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHeThongRap,TenHeThongRap,BiDanh,Logo")] HeThongRap heThongRap)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (id != heThongRap.MaHeThongRap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(heThongRap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeThongRapExists(heThongRap.MaHeThongRap))
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
            return View(heThongRap);
        }

        // GET: HeThongRaps/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var heThongRap = await _context.HeThongRaps
                .FirstOrDefaultAsync(m => m.MaHeThongRap == id);
            if (heThongRap == null)
            {
                return NotFound();
            }

            return View(heThongRap);
        }

        // POST: HeThongRaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var heThongRap = await _context.HeThongRaps.FindAsync(id);
            _context.HeThongRaps.Remove(heThongRap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeThongRapExists(string id)
        {
            return _context.HeThongRaps.Any(e => e.MaHeThongRap == id);
        }
    }
}
