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
    public class LichChieuController : Controller
    {
        private readonly movieContext _context;
        private readonly Middlewares.IAuthorization _Authorization;

        public LichChieuController(movieContext context, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _Authorization = Authorization;
        }

        // GET: LichChieux
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            var movieContext = _context.LichChieus.Include(l => l.MaPhimNavigation).Include(l => l.MaRapNavigation);
            return View(await movieContext.ToListAsync());
        }

        // GET: LichChieux/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var lichChieu = await _context.LichChieus
                .Include(l => l.MaPhimNavigation)
                .Include(l => l.MaRapNavigation)
                .FirstOrDefaultAsync(m => m.MaLichChieu == id);
            if (lichChieu == null)
            {
                return NotFound();
            }

            return View(lichChieu);
        }

        // GET: LichChieux/Create
        public IActionResult Create()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            ViewData["MaPhim"] = new SelectList(_context.Phims, "MaPhim", "MaPhim");
            ViewData["MaRap"] = new SelectList(_context.Raps, "MaRap", "MaRap");
            return View();
        }

        // POST: LichChieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLichChieu,MaRap,MaPhim,NgayChieuGioChieu,GiaVe")] LichChieu lichChieu)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(lichChieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhim"] = new SelectList(_context.Phims, "MaPhim", "MaPhim", lichChieu.MaPhim);
            ViewData["MaRap"] = new SelectList(_context.Raps, "MaRap", "MaRap", lichChieu.MaRap);
            return View(lichChieu);
        }

        // GET: LichChieux/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var lichChieu = await _context.LichChieus.FindAsync(id);
            if (lichChieu == null)
            {
                return NotFound();
            }
            ViewData["MaPhim"] = new SelectList(_context.Phims, "MaPhim", "MaPhim", lichChieu.MaPhim);
            ViewData["MaRap"] = new SelectList(_context.Raps, "MaRap", "MaRap", lichChieu.MaRap);
            return View(lichChieu);
        }

        // POST: LichChieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLichChieu,MaRap,MaPhim,NgayChieuGioChieu,GiaVe")] LichChieu lichChieu)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (id != lichChieu.MaLichChieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lichChieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichChieuExists(lichChieu.MaLichChieu))
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
            ViewData["MaPhim"] = new SelectList(_context.Phims, "MaPhim", "MaPhim", lichChieu.MaPhim);
            ViewData["MaRap"] = new SelectList(_context.Raps, "MaRap", "MaRap", lichChieu.MaRap);
            return View(lichChieu);
        }

        // GET: LichChieux/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var lichChieu = await _context.LichChieus
                .Include(l => l.MaPhimNavigation)
                .Include(l => l.MaRapNavigation)
                .FirstOrDefaultAsync(m => m.MaLichChieu == id);
            if (lichChieu == null)
            {
                return NotFound();
            }

            return View(lichChieu);
        }

        // POST: LichChieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var lichChieu = await _context.LichChieus.FindAsync(id);
            _context.LichChieus.Remove(lichChieu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LichChieuExists(int id)
        {
            return _context.LichChieus.Any(e => e.MaLichChieu == id);
        }
    }
}
