using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TrunkeyWebApp.Middlewares
{
    public interface IAuthorization
    {
        Boolean AuthorizationAdmin(HttpRequest Request);
        void IdentifyUser(HttpRequest Request, ViewDataDictionary ViewData);
        string? GetUserName(HttpRequest Request);
    }
    public class Authorization : IAuthorization
    {
        private readonly ICookiesAction _cookiesAction;
        public Authorization(ICookiesAction cookiesAction)
        {
            _cookiesAction = cookiesAction;
        }

        public Boolean AuthorizationAdmin(HttpRequest Request)
        {
            var Authorization = _cookiesAction.ReadCookies(Request);
            if (Authorization != null && Authorization.MaLoaiNguoiDung == "admin") return true;
            return false;
        }

        public void IdentifyUser(HttpRequest Request, ViewDataDictionary ViewData)
        {
            var Authorization = _cookiesAction.ReadCookies(Request);
            if (Authorization != null)
            {
                ViewData["validated"] = "true";
                ViewData["nguoiDung"] = Authorization.TaiKhoan;
            }
        }

        public string? GetUserName(HttpRequest Request)
        {
            var Authorization = _cookiesAction.ReadCookies(Request);
            if (Authorization != null)
            {
                return Authorization.TaiKhoan;
            }
            return null;
        }
    }
}
