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
    public class PhimsController : Controller
    {
        private readonly movieContext _context;
        private readonly Middlewares.IAuthorization _Authorization;

        public PhimsController(movieContext context, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _Authorization = Authorization; 
        }

        // GET: Phims
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            return View(await _context.Phims.ToListAsync());
        }

        // GET: Phims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims
                .FirstOrDefaultAsync(m => m.MaPhim == id);
            if (phim == null)
            {
                return NotFound();
            }

            return View(phim);
        }

        // GET: Phims/Create
        public IActionResult Create()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            return View();
        }

        // POST: Phims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhim,TenPhim,BiDanh,Trailer,HinhAnh,MoTa,MaNhom,NgayKhoiChieu,DanhGia,Hot,DangChieu,SapChieu")] Phim phim)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(phim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phim);
        }

        // GET: Phims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims.FindAsync(id);
            if (phim == null)
            {
                return NotFound();
            }
            return View(phim);
        }

        // POST: Phims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhim,TenPhim,BiDanh,Trailer,HinhAnh,MoTa,MaNhom,NgayKhoiChieu,DanhGia,Hot,DangChieu,SapChieu")] Phim phim)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (id != phim.MaPhim)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhimExists(phim.MaPhim))
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
            return View(phim);
        }

        // GET: Phims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims
                .FirstOrDefaultAsync(m => m.MaPhim == id);
            if (phim == null)
            {
                return NotFound();
            }

            return View(phim);
        }

        // POST: Phims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var phim = await _context.Phims.FindAsync(id);
            _context.Phims.Remove(phim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhimExists(int id)
        {
            return _context.Phims.Any(e => e.MaPhim == id);
        }
    }
}
