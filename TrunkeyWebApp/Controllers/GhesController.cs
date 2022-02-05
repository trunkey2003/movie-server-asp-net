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
    public class GhesController : Controller
    {
        private readonly movieContext _context;
        private readonly Middlewares.IAuthorization _Authorization;

        public GhesController(movieContext context, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _Authorization = Authorization;
        }

        // GET: Ghes
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            var movieContext = _context.Ghes.Include(g => g.MaLichChieuNavigation).Include(g => g.TaiKhoanNguoiDatNavigation).Take(1000);
            return View(await movieContext.ToListAsync());
        }

        // GET: Ghes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes
                .Include(g => g.MaLichChieuNavigation)
                .Include(g => g.TaiKhoanNguoiDatNavigation)
                .FirstOrDefaultAsync(m => m.MaGhe == id);
            if (ghe == null)
            {
                return NotFound();
            }

            return View(ghe);
        }

        // GET: Ghes/Create
        public IActionResult Create()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            ViewData["MaLichChieu"] = new SelectList(_context.LichChieus, "MaLichChieu", "MaLichChieu");
            ViewData["TaiKhoanNguoiDat"] = new SelectList(_context.NguoiDungs, "TaiKhoan", "TaiKhoan");
            return View();
        }

        // POST: Ghes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGhe,MaLichChieu,SttGhe,LoaiGhe,DaDat,TaiKhoanNguoiDat")] Ghe ghe)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(ghe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLichChieu"] = new SelectList(_context.LichChieus, "MaLichChieu", "MaLichChieu", ghe.MaLichChieu);
            ViewData["TaiKhoanNguoiDat"] = new SelectList(_context.NguoiDungs, "TaiKhoan", "TaiKhoan", ghe.TaiKhoanNguoiDat);
            return View(ghe);
        }

        // GET: Ghes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes.FindAsync(id);
            if (ghe == null)
            {
                return NotFound();
            }
            ViewData["MaLichChieu"] = new SelectList(_context.LichChieus, "MaLichChieu", "MaLichChieu", ghe.MaLichChieu);
            ViewData["TaiKhoanNguoiDat"] = new SelectList(_context.NguoiDungs, "TaiKhoan", "TaiKhoan", ghe.TaiKhoanNguoiDat);
            return View(ghe);
        }

        // POST: Ghes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGhe,MaLichChieu,SttGhe,LoaiGhe,DaDat,TaiKhoanNguoiDat")] Ghe ghe)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (id != ghe.MaGhe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ghe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GheExists(ghe.MaGhe))
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
            ViewData["MaLichChieu"] = new SelectList(_context.LichChieus, "MaLichChieu", "MaLichChieu", ghe.MaLichChieu);
            ViewData["TaiKhoanNguoiDat"] = new SelectList(_context.NguoiDungs, "TaiKhoan", "TaiKhoan", ghe.TaiKhoanNguoiDat);
            return View(ghe);
        }

        // GET: Ghes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes
                .Include(g => g.MaLichChieuNavigation)
                .Include(g => g.TaiKhoanNguoiDatNavigation)
                .FirstOrDefaultAsync(m => m.MaGhe == id);
            if (ghe == null)
            {
                return NotFound();
            }

            return View(ghe);
        }

        // POST: Ghes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var ghe = await _context.Ghes.FindAsync(id);
            _context.Ghes.Remove(ghe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GheExists(int id)
        {
            return _context.Ghes.Any(e => e.MaGhe == id);
        }
    }
}
