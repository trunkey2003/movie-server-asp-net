using Microsoft.AspNetCore.Mvc;

namespace TrunkeyWebApp.Controllers
{
    public class CookiesController : Controller
    {
        public IActionResult ClearCookies()
        {
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddHours(-1),
                HttpOnly = true,
                Secure = true
            };
            Response.Cookies.Append("token", "hello", options);
            return RedirectToAction("Index", "Home");
        } 
    }

    
}
