using ASP_P42.Data;
using ASP_P42.Data.Entities;
using ASP_P42.Services.Kdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ASP_P42.Controllers
{
    public class UserController(
        DataContext dataContext,
        IKdfService kdfService
    ) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IKdfService _kdfService = kdfService;

        // Автентифікація - перевірка логіна та паролю
        public IActionResult BasicAuth()
        {
            // Зворотні дії до стандарту RFC 7617 'Basic' HTTP Authentication Scheme
            // Перевірити, чи є заголовок автентифікації
            String authHeader = HttpContext.Request.Headers.Authorization.ToString();
            if (authHeader == String.Empty)
            {
                return Unauthorized("Missing Authorization header");
            }
            String scheme = "Basic ";
            if (!authHeader.StartsWith(scheme))
            {
                return Unauthorized("Authorization scheme must be 'Basic'");
            }
            String credentials = authHeader[scheme.Length..];
            byte[] rawData;
            try
            {
                rawData = Convert.FromBase64String(credentials);
            }
            catch
            {
                return Unauthorized(
                    "Authorization credentials must be valid Base64::section 4");
            }
            String userPass;
            try
            {
                userPass = Encoding.UTF8.GetString(rawData);
            }
            catch
            {
                return Unauthorized(
                    "User-pass must be valid UTF8 string");
            }
            String[] parts = userPass.Split(':', 2);
            if (parts.Length != 2)
            {
                return Unauthorized(
                    "User-pass must be concatenated by ':'");
            }
            String login = parts[0];
            String password = parts[1];
            // оскільки пароль у БД не зберігається, більш того,
            // засобами БД неможна обрахувати DK, перевірка 
            // здійснюється у два етапи:
            // 1. шукаємо у БД користувача за логіном (він унікальний)
            // 2. вилучаємо сіль даного користувача і запускаємо
            //    обчислення DK із переданим паролем та сіллю
            //    Результат обчислення має збігатись зі збереженим DK у БД
            if (_dataContext
                .UserAccesses
                .FirstOrDefault(ua => ua.Login == login)
                is UserAccess userAccess)
            {
                String dk = _kdfService.Dk(password, userAccess.Salt);
                if (dk == userAccess.Dk)
                {
                    // точка позитивного рішення про автентифікацію
                    // тут слід переходити до авторизації. Розглянемо
                    // два способи: автоматичний через сесії та
                    // окремий через токени.

                    // Серверна сесія (сеанс) - спосіб збереження даних 
                    // про запити з боку сервера із встановленням для 
                    // запиту Cookie, що грає роль токена доступу до
                    // збережених даних. Від клієнта не вимагається 
                    // особливих дій, лише забезпечити стандартну роботу
                    // cookie, якщо застосунок поза браузером.
                    // Дані зберігаємо у сесію (налаштування - Program.cs)
                    HttpContext.Session.SetString(
                        "userAccessId",
                        userAccess.Id.ToString()
                    );
                    // відповідь може бути порожньою
                    return Ok();
                }
            }
            return Unauthorized(
                "Credentials rejected: check login and password");
        }
    }
}
