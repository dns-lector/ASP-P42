using ASP_P42.Data;
using ASP_P42.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ASP_P42.Middleware.AuthSession
{
    // Класи Middleware повинні мати певну структуру:
    // - конструктор має приймати посилання на наступний Middleware - next
    // - для подальшого використання посилання має бути збережене
    // - за умовами (або безумовно) робота має передаватись далі
    public class AuthSessionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(
            HttpContext context,       // інжекція через метод
            DataContext dataContext    // порядок ролі не грає, тільки тип
        )
        {
            String authKey = "userAccessId";
            // Спочатку перевіряємо чи не запитано вихід (з авториз. режиму)
            // про це свідчить наявність query-параметра "logout"
            if (context.Request.Query.ContainsKey("logout"))
            {
                // видаляємо з сесії збережені дані
                context.Session.Remove(authKey);
                // переадресовуємо відповідь на ту ж адресу, з
                // якої прийшов запит
                // з метою "прибирання" наявного query-параметра
                context.Response.Redirect(context.Request.Path);
                // зупиняємо подальшу обробку даного запиту
                return;
            }

            // context, що передається параметром, це той самий 
            // HttpContext, що доступний з контролерів
            // Відповідно, до нього можна закласти дані, що можуть
            // бути використані як у контролерах, так і у представленнях
            context.Items.Add("itemKey", "Item Value");

            // перевіряємо, чи є у сесії елемент з ключем "userAccessId"
            if (context.Session.Keys.Contains(authKey))
            {
                String userAccessId = context.Session.GetString(authKey)!;
                // це має бути валідний рядок з БД - перевіряємо...
                UserAccess? userAccess = dataContext
                    .UserAccesses
                    .Include(ua => ua.UserData)  // інструкція для заповнення
                    .Include(ua => ua.UserRole)  // навігаційних властивостей
                    .AsNoTracking()              // Вимкнення стеження змін
                    .FirstOrDefault(ua => ua.Id.ToString() == userAccessId);
                if (userAccess != null)
                {
                    // знайдено підтвердження допуску, передаємо до контексту
                    // context.Items.Add(authKey, userAccess);
                    // Даний підхід не є рекомендованим, оскільки 
                    // прив'язується до типів даних сутності.
                    // Рекомендовано використати уніфікований інтерфейс
                    // за допомогою Claims - набору атрибутів типового призначення
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            [
                                new Claim(ClaimTypes.Name, userAccess.UserData.FullName),
                                new(ClaimTypes.Email, userAccess.UserData.Email),
                                new(ClaimTypes.NameIdentifier, userAccess.Login),
                                new(ClaimTypes.Sid, userAccess.Id.ToString()),
                            ],
                            nameof(AuthSessionMiddleware)
                        )
                    );
                }
            }

            // передача роботи до наступного Middleware
            await _next(context);
        }
    }
}
/* Д.З. Вивести на головній сторінці сайту дані,
 * що закладені до контексту запиту в AuthSessionMiddleware
 * (context.Items.Add("itemKey", "Item Value");)
 * * відключити midleware, переконатись, що дані зникають
 */