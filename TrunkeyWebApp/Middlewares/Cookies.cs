using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;

namespace TrunkeyWebApp.Middlewares
{
    public class Cookies
    {
        public String? TaiKhoan { set; get; }
        public String? MaLoaiNguoiDung { set; get; }
    }
    public interface ICookiesAction
    {
        Cookies? ReadCookies(HttpRequest Request);
    }
    public class CookiesAction : ICookiesAction
    {
        public Cookies? ReadCookies(HttpRequest Request)
        {
            var token = Request.Cookies["token"];
            string? secret = Environment.GetEnvironmentVariable("TOKEN_SECRET_KEY");
            if (token == null) return null;
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token, secret, verify: true);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<Cookies>(json);
            }
            catch (TokenExpiredException)
            {
                return null;
            }
            catch (SignatureVerificationException)
            {
                return null;
            }
        }
    }
}
