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
    public class LoaiNguoiDungsController : Controller
    {
        private readonly movie_server_cybersoftContext _context;
        private readonly Middlewares.IAuthorization _Authorization;

        public LoaiNguoiDungsController(movie_server_cybersoftContext context, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _Authorization = Authorization;
        }

        // GET: LoaiNguoiDungs
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            return View(await _context.LoaiNguoiDungs.ToListAsync());
        }

        // GET: LoaiNguoiDungs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var loaiNguoiDung = await _context.LoaiNguoiDungs
                .FirstOrDefaultAsync(m => m.MaLoaiNguoiDung == id);
            if (loaiNguoiDung == null)
            {
                return NotFound();
            }

            return View(loaiNguoiDung);
        }

        // GET: LoaiNguoiDungs/Create
        public IActionResult Create()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            return View();
        }

        // POST: LoaiNguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoaiNguoiDung,TenLoai")] LoaiNguoiDung loaiNguoiDung)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(loaiNguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiNguoiDung);
        }

        // GET: LoaiNguoiDungs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var loaiNguoiDung = await _context.LoaiNguoiDungs.FindAsync(id);
            if (loaiNguoiDung == null)
            {
                return NotFound();
            }
            return View(loaiNguoiDung);
        }

        // POST: LoaiNguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaLoaiNguoiDung,TenLoai")] LoaiNguoiDung loaiNguoiDung)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (id != loaiNguoiDung.MaLoaiNguoiDung)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiNguoiDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiNguoiDungExists(loaiNguoiDung.MaLoaiNguoiDung))
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
            return View(loaiNguoiDung);
        }

        // GET: LoaiNguoiDungs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var loaiNguoiDung = await _context.LoaiNguoiDungs
                .FirstOrDefaultAsync(m => m.MaLoaiNguoiDung == id);
            if (loaiNguoiDung == null)
            {
                return NotFound();
            }

            return View(loaiNguoiDung);
        }

        // POST: LoaiNguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var loaiNguoiDung = await _context.LoaiNguoiDungs.FindAsync(id);
            _context.LoaiNguoiDungs.Remove(loaiNguoiDung);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiNguoiDungExists(string id)
        {
            return _context.LoaiNguoiDungs.Any(e => e.MaLoaiNguoiDung == id);
        }
    }
}
