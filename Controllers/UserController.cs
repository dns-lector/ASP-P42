using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers
{
    public class UserController : Controller
    {
        // Автентифікація - перевірка логіна та паролю
        public IActionResult BasicAuth()
        {
            // Зворотні дії до стандарту RFC 7617 'Basic' HTTP Authentication Scheme
            // Перевірити, чи є заголовок автентифікації
            String authHeader = HttpContext.Request.Headers.Authorization.ToString();
            if(authHeader == String.Empty)
            {
                return Unauthorized("Missing Authorization header");
            }
            String scheme = "Basic ";
            if (!authHeader.StartsWith(scheme))
            {
                return Unauthorized("Authorization scheme must be 'Basic'");
            }
            return Json(authHeader);
        }
    }
}
