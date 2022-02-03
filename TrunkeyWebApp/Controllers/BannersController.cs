#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrunkeyWebApp.Models;

namespace TrunkeyWebApp.Controllers
{
    public class BannersController : Controller
    {
        private readonly movie_server_cybersoftContext _context;
        private readonly Middlewares.ICookiesAction _cookiesAction;
        private readonly Middlewares.IAuthorization _Authorization;

        public BannersController(movie_server_cybersoftContext context, Middlewares.ICookiesAction cookiesAction, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _cookiesAction = cookiesAction;
            _Authorization = Authorization;
        }

        // GET: Banners
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            var movie_server_cybersoftContext = _context.Banners;
            return View(await movie_server_cybersoftContext.ToListAsync());
        }
  
        // GET: Banners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return View("~/Views/_NotFound.cshtml");
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.MaBanner == id);
            if (banner == null)
            {
                return View("~/Views/_NotFound.cshtml");
            }

            return View(banner);
        }

        // GET: Banners/Create
        public IActionResult Create()
        {
            ViewData["MaPhim"] = new SelectList(_context.Phims, "MaPhim", "MaPhim");
            return View();
        }

        // POST: Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBanner,MaPhim,HinhAnh")] Banner banner)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhim"] = new SelectList(_context.Phims, "MaPhim", "MaPhim", banner.MaPhim);
            return View(banner);
        }

        // GET: Banners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return View("~/Views/_NotFound.cshtml");
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return View("~/Views/_NotFound.cshtml");
            }
            ViewData["MaPhim"] = new SelectList(_context.Phims, "MaPhim", "MaPhim", banner.MaPhim);
            return View(banner);
        }

        // POST: Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaBanner,MaPhim,HinhAnh")] Banner banner)
        {
            //return Json(banner);
            _Authorization.IdentifyUser(Request, ViewData);
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");

            if (id != banner.MaBanner)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException err)
                {
                    if (!BannerExists(banner.MaBanner))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw err;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhim"] = new SelectList(_context.Phims, "MaPhim", "MaPhim", banner.MaPhim);
            return View(banner);
        }

        // GET: Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .Include(b => b.MaPhimNavigation)
                .FirstOrDefaultAsync(m => m.MaBanner == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var banner = await _context.Banners.FindAsync(id);
            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.MaBanner == id);
        }
    }
}
