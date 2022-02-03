using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrunkeyWebApp.Models;

namespace TrunkeyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Middlewares.ICookiesAction _cookiesAction;

        public HomeController(ILogger<HomeController> logger, Middlewares.ICookiesAction cookiesAction)
        {
            _logger = logger;
            _cookiesAction = cookiesAction; 
        }

        public IActionResult Index()
        {
            var Authorization = _cookiesAction.ReadCookies(Request);
            if (Authorization != null)
            {
                ViewData["validated"] = "true";
                ViewData["nguoiDung"] = Authorization.TaiKhoan;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}