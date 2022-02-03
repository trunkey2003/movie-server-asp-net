#nullable disable
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrunkeyWebApp.Models;

namespace TrunkeyWebApp.Controllers
{
    public class NguoiDungsController : Controller
    {
        private readonly movie_server_cybersoftContext _context;
        private readonly Middlewares.ICookiesAction _cookiesAction;
        private readonly Middlewares.IAuthorization _Authorization;


        public NguoiDungsController(movie_server_cybersoftContext context, Middlewares.ICookiesAction cookiesAction, Middlewares.IAuthorization Authorization)
        {
            _context = context;
            _cookiesAction = cookiesAction;
            _Authorization = Authorization;
        }

        //MY CUSTOM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ViewNguoiDungDangNhap obj)
        {
            ViewData["Login"] = "Login";
            var ExpireTime = 1;
            var nguoiDung = await _context.NguoiDungs
             .Include(n => n.MaLoaiNguoiDungNavigation)
             .FirstOrDefaultAsync(m => m.TaiKhoan == obj.TaiKhoan);
            if (nguoiDung != null && BCrypt.Net.BCrypt.Verify(obj.MatKhau, nguoiDung.MatKhau))
            {
                ModelState.Clear();
                ViewData["nguoiDung"] = nguoiDung.TaiKhoan;
                ViewData["validated"] = "true";
                if (obj.RememberMe) ExpireTime = 24;
                CookieOptions options = new()
                {
                    Expires = DateTime.Now.AddHours(ExpireTime),
                    HttpOnly = true,
                    Secure = true
                };
                string secret = Environment.GetEnvironmentVariable("TOKEN_SECRET_KEY");
                var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(secret)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("TaiKhoan", nguoiDung.TaiKhoan)
                      .AddClaim("MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung)
                      .Encode();
                Response.Cookies.Append("token", token, options);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["validated"] = "false";
                ViewData["msg"] = "Sai Tài Khoản Hoặc Mật Khẩu";
                ModelState.Clear();
                ModelState.AddModelError("LoginError", "Sai Tài Khoản Hoặc Mật Khẩu");
                return View("~/Views/Home/Index.cshtml");
            }
        }

        public async Task<IActionResult> SignUp(ViewNguoiDungDangKy obj)
        {
            ViewData["SignUp"] = "SignUp";
            var nguoiDung = await _context.NguoiDungs
             .Include(n => n.MaLoaiNguoiDungNavigation)
             .FirstOrDefaultAsync(m => m.TaiKhoan == obj.TaiKhoan);
            if (nguoiDung != null) ModelState.AddModelError("err", $"Người Dùng {obj.TaiKhoan} đã tồn tại");
            if (!ModelState.IsValid)
            {
                IEnumerable<string> allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                var _values = allErrors.AsEnumerable().Select(x => x).ToList();

                ViewData["msg"] = _values[0];
                ViewData["msg2"] = (_values.Count > 1) ? _values[1] : null;
                return View("~/Views/Home/Index.cshtml");
            }
            NguoiDung newUser = new()
            {
                TaiKhoan = obj.TaiKhoan,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(obj.MatKhau),
                MaLoaiNguoiDung = obj.MaLoaiNguoiDung,
            };
            _context.Add(newUser);
            await _context.SaveChangesAsync();
            ViewData["msg"] = "Đăng Ký Thành Công !";
            return View("~/Views/Home/Index.cshtml");
        }

        // GET: NguoiDungs
        public async Task<IActionResult> Index()
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var movie_server_cybersoftContext = _context.NguoiDungs.Include(n => n.MaLoaiNguoiDungNavigation);
            var res = await movie_server_cybersoftContext.ToListAsync();
            var TaiKhoan = _Authorization.GetUserName(Request);
            foreach (var item in res)
            {
                if (item.TaiKhoan != TaiKhoan) item.MatKhau = "*********";
            }
            return View(await movie_server_cybersoftContext.ToListAsync());
        }

        // GET: NguoiDungs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (_Authorization.GetUserName(Request) != id && !_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");

            if (id == null)
            {
                return View("~/Views/_NotFound.cshtml");
            }

            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(m => m.TaiKhoan == id);
            if (nguoiDung == null)
            {
                return View("~/Views/_NotFound.cshtml");
            }

            return View(nguoiDung);
        }

        // GET: NguoiDungs/Create
        public IActionResult Create()
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            _Authorization.IdentifyUser(Request, ViewData);
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung");
            return View();
        }

        // POST: NguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaiKhoan,HoTen,Email,SoDt,MatKhau,MaLoaiNguoiDung")] NguoiDung nguoiDung)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (ModelState.IsValid)
            {
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            _Authorization.IdentifyUser(Request, ViewData);
            return View(nguoiDung);
        }

        // GET: NguoiDungs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View(nguoiDung);
        }

        // POST: NguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TaiKhoan,HoTen,Email,SoDt,MatKhau,MaLoaiNguoiDung")] NguoiDung nguoiDung)
        {
            if (!_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            if (id != nguoiDung.TaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(nguoiDung.TaiKhoan))
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
            ViewData["MaLoaiNguoiDung"] = new SelectList(_context.LoaiNguoiDungs, "MaLoaiNguoiDung", "MaLoaiNguoiDung", nguoiDung.MaLoaiNguoiDung);
            return View(nguoiDung);
        }

        // GET: NguoiDungs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (_Authorization.GetUserName(Request) != id && !_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            _Authorization.IdentifyUser(Request, ViewData);
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
                .Include(n => n.MaLoaiNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.TaiKhoan == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // POST: NguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            _Authorization.IdentifyUser(Request, ViewData);
            if (_Authorization.GetUserName(Request) != id && !_Authorization.AuthorizationAdmin(Request)) return View("~/Views/_AuthorizationError.cshtml");
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            _context.NguoiDungs.Remove(nguoiDung);
            await _context.SaveChangesAsync();
            if (_Authorization.GetUserName(Request) == id)
            {
                //clear cookie if users delete their account
                CookieOptions options = new CookieOptions()
                {
                    Expires = DateTime.Now.AddHours(-1),
                    HttpOnly = true,
                    Secure = true
                };
                Response.Cookies.Append("token", "hello", options);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NguoiDungExists(string id)
        {
            return _context.NguoiDungs.Any(e => e.TaiKhoan == id);
        }
    }
}
